using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RectSlayer
{
    public class GameManager
    {
        public Shooter Player{ get; set; }
        public List<Ball> Balls { get; set; }
        public List<Rectangle> Rectangles { get; set; }

        public int left { get; set; }
        public int top { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int Level { get; set; }
        private int rectWidth = 40;
        private int rectHeight = 40;

        private static readonly int objectsToGenerate = 6;

        private Timer shootTimer;
        bool canStartLevel;
        bool canStartLevelIndicator;

        bool movedShooter;

        Point newPlayerPosition;

        private static readonly int PlayerClampDistance = 3;

        public GameManager(int left, int top, int width, int height, Timer shootTimer)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            this.shootTimer = shootTimer;
            rectWidth = width / 6 - 2;
            canStartLevel = false;
            canStartLevelIndicator = false;
            movedShooter = false;
            StartGame();
        }

        public void StartGame()
        {
            Level = 1;
            Player = new Shooter(new Point(left + width / 2, top + height - 20));
            Balls = new List<Ball>();
            Rectangles = new List<Rectangle>();
            GenerateRectangles();
            Player.CanShoot = true;
        }

        public void GameOver()
        {

        }

        public void HandleLogic()
        {
            foreach (Ball ball in Balls)
            {

                for (int i=Rectangles.Count-1;i>=0;--i)
                {
                    if (ball.CheckCollision(Rectangles.ElementAt(i)))
                    {
                        if (Rectangles.ElementAt(i).HitsRemaining <= 0)
                        {
                            Rectangles.RemoveAt(i);
                        }
                        break;
                    }
                }
                ball.Move(left, top, width, height); 
            }
            
            for (int i = Balls.Count - 1; i >= 0; i--)
            {
                if (Balls.ElementAt(i).IsDead)
                {
                    if (!movedShooter)
                    {
                        int xNewPos = Balls.ElementAt(i).Center.X;
                        if (xNewPos < left + PlayerClampDistance)
                        {
                            xNewPos = left + PlayerClampDistance;
                        }
                        // 30 is player width, it is hard coded for now
                        else if (xNewPos > left + width - PlayerClampDistance - 30)     
                        {
                            xNewPos = left + width - PlayerClampDistance - 30;
                        }
                        newPlayerPosition = new Point(xNewPos, Player.Position.Y);
                        movedShooter = !movedShooter;
                    }
                    Balls.RemoveAt(i);
                }
            }

            if (Balls.Count <= 0)
            {
                if(canStartLevelIndicator)
                {
                    canStartLevel = true;
                    canStartLevelIndicator = false;
                }
                IncreaseLevel();
                Player.CanShoot = true;
            }
        }

        private void IncreaseLevel()
        {
            movedShooter = false;
            if (!canStartLevel) return;
            canStartLevel = false;
            ++Level;
            MoveRectangles();
            GenerateRectangles();

            if (newPlayerPosition != null)
            {
                Player.Relocate(newPlayerPosition);
            }

            canStartLevel = false;
        }

        private void MoveRectangles()
        {
            foreach(Rectangle rect in Rectangles)
            {
                rect.LeftTopPoint = new Point(rect.LeftTopPoint.X, rect.LeftTopPoint.Y + rectHeight+2);
            }
        }

        public void Draw(Graphics g)
        {
            foreach (Ball ball in Balls)
            {
                ball.Draw(g);
            }
            foreach(Rectangle rect in Rectangles)
            {
                rect.Draw(g);
            }
            Player.Draw(g);
        }

        public void DrawIndicatorLine(Graphics g, Point mouseLocation)
        {
            int mouseX = mouseLocation.X;
            int mouseY = mouseLocation.Y;

            if (mouseLocation.X > left + width)
                mouseX = left + width;
            else if (mouseLocation.X < left)
                mouseX = left;

            if (mouseLocation.Y > top + height)
                mouseY = top + height;
            else if (mouseLocation.Y < top)
                mouseY = top;

            Point newMouseLocation = new Point(mouseX, mouseY);

            Player.DrawLine(g, newMouseLocation);
        }

        public void StartShooting(Point mouseLocation)
        {
            if (!Player.CanShoot) return;

            Player.BallsToShoot = Level;

            Player.CanShoot = false;

            shootTimer.Start();
            int dx = mouseLocation.X - Player.Position.X;
            int dy = mouseLocation.Y - Player.Position.Y;
            float alpha_radians = (float)Math.Atan2(dy, dx);

            Player.xBallVelocity = (float)Math.Cos(alpha_radians) * Ball.VELOCITY;
            Player.yBallVelocity = (float)Math.Sin(alpha_radians) * Ball.VELOCITY;
        }

        public void ShootBall()
        {
            Ball b = Player.ShootBall(Color.Red);
            if (b != null)
                Balls.Add(b);
            if (Player.BallsShot == Player.BallsToShoot)
            {
                Player.BallsShot = 0;
                Player.CanShoot = false;
                canStartLevelIndicator = true;
                shootTimer.Stop();
            }
        }

        public void GenerateRectangles()
        {
            int step = 80;
            int currentStartingPoint = left + 6;

            for (int i = 0; i<objectsToGenerate-1; ++i)
            {
                Rectangle newRect = new Rectangle(new Point(currentStartingPoint, top + rectHeight + 3), rectWidth, rectHeight,
                    Color.Blue, 1);
                newRect.HitsRemaining = Level;
                Rectangles.Add(newRect);
                currentStartingPoint += step;
            }
        }
    }
}
