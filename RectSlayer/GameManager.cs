

﻿using RectSlayer.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public List<PowerUp> PowerUps { get; set; }

        public int left { get; set; }
        public int top { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int Level { get; set; }
        private int rectWidth = 40;
        private int rectHeight = 40;

        private static readonly int objectsToGenerate = 6;
        private PowerUpsFactory powerUpFactory;
        private static Random random = new Random(DateTime.Now.Millisecond);

        private Timer shootTimer;
        bool canStartLevel;
        bool canStartLevelIndicator;

        bool movedShooter;

        Point newPlayerPosition;

        private static readonly int PlayerClampDistance = 3;

        private static readonly int yGameOverCheck = 420;

        public bool isGameOver { get; set; }

        private static readonly float shootAngleLimitation = 0.08f;

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
            powerUpFactory = new PowerUpsFactory();
            StartGame();
        }

        public void StartGame()
        {
            Level = 1;
            Player = new Shooter(new Point(left + width / 2, top + height - 20));
            Balls = new List<Ball>();
            Rectangles = new List<Rectangle>();
            PowerUps = new List<PowerUp>();
            GenerateObjects();
            Player.CanShoot = true;
            isGameOver = false;
        }

        /*
        private Image getImage(string v)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("RectSlayer.Resources."+v);
            Bitmap bmp = new Bitmap(myStream);

            return bmp;
        }
        */

        public void GameOver()
        {
            isGameOver = true;
        }

        public void HandleLogic()
        {
            foreach (PowerUp powerUp in PowerUps)
                powerUp.IsActive = false;

            foreach (Ball ball in Balls)
            {
                CheckRectangleCollision(ball);

                CheckPowerUpCollision(ball);
				
				ball.Move(left, top, width, height); 
            }

            CheckDeadBalls();

            if (Balls.Count <= 0)
            {
                if(canStartLevelIndicator)
                {
                    canStartLevel = true;
                    canStartLevelIndicator = false;
                    Player.CanShoot = true;
                    IncreaseLevel();
                }
                
            }
        }

        private void CheckDeadBalls()
        {
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
        }

        private void CheckRectangleCollision(Ball ball)
        {
            for (int i = Rectangles.Count - 1; i >= 0; --i)
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
        }

        private void CheckPowerUpCollision(Ball ball)
        {
            for (int i = PowerUps.Count - 1; i >= 0; i--)
            {
                var powerUp = PowerUps.ElementAt(i);
                if (powerUp.CheckCollision(ball))
                {
                    if (powerUp.ActivatePowerUp(ball))
                    {
                        if (powerUp.GetType() == typeof(PlusPowerUp))
                        {
                            Player.IncreaseCBalls();
                            PowerUps.RemoveAt(i);
                        }

                        if (powerUp.GetType() == typeof(RandomDirectionPowerUp))
                        {
                            ((RandomDirectionPowerUp)powerUp).RandomDirection(ball);
                        }

                        if (powerUp.GetType() == typeof(HorizontalHitPowerUp))
                        {
                            powerUp.IsActive = true;
                            ((HorizontalHitPowerUp)powerUp).Hit(Rectangles);
                        }
                        

                        if (powerUp.GetType() == typeof(VerticalHitPowerUp))
                        {
                            powerUp.IsActive = true;
                            ((VerticalHitPowerUp)powerUp).Hit(Rectangles);
                        }
                        
                    }

                }
                else
                {
                    powerUp.FilterBalls();
                }
            }
        }

        private void IncreaseLevel()
        {
            movedShooter = false;
            if (!canStartLevel) return;
            canStartLevel = false;
            ++Level;
            MoveRectangles();
            MovePowerUps();
            GenerateObjects();

            if (newPlayerPosition != null)
            {
                Player.Relocate(newPlayerPosition);
            }

            canStartLevel = false;

            for (int i = PowerUps.Count - 1; i >= 0; --i)
            {
                if (PowerUps.ElementAt(i).LeftTopPoint.Y > yGameOverCheck)
                {
                    PowerUps.RemoveAt(i);
                }
            }

            if (GameOverCheck())
            {
                GameOver();
            }
        }

        private void MovePowerUps()
        {
            for(int i=PowerUps.Count - 1; i >= 0; i--)
            {
                var powerUp = PowerUps.ElementAt(i);
                if(powerUp.IsUsed)
                {
                    PowerUps.RemoveAt(i);
                }
                else
                {
                    powerUp.LeftTopPoint = new Point(powerUp.LeftTopPoint.X, powerUp.LeftTopPoint.Y + rectHeight + 2);
                    powerUp.CalculateCenter();
                }
            }
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
            foreach(PowerUp powerUp in PowerUps)
            {
                powerUp.Draw(g);
                if(powerUp.IsActive)
                {
                    DrawActivePowerUp(g, powerUp);
                }
            }
            Player.Draw(g);
        }

        private void DrawActivePowerUp(Graphics g, PowerUp powerUp)
        {
            Brush brush = new SolidBrush(Color.White);
            if(powerUp.GetType() == typeof(HorizontalHitPowerUp))
            {
                g.FillRectangle(brush, left, powerUp.LeftTopPoint.Y, width, powerUp.PowerUpImage.Height);
            }
            else
            {
                g.FillRectangle(brush, powerUp.LeftTopPoint.X, top, powerUp.PowerUpImage.Width, height);
            }
            brush.Dispose();
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

            int dx = mouseLocation.X - Player.Position.X;
            int dy = mouseLocation.Y - Player.Position.Y;
            float alphaRadians = (float)Math.Atan2(dy, dx);

            if (alphaRadians > - shootAngleLimitation) return;

            if (alphaRadians < -Math.PI + shootAngleLimitation) return;

            Console.WriteLine(alphaRadians);


            Player.CanShoot = false;

            shootTimer.Start();


            Player.xBallVelocity = (float)Math.Cos(alphaRadians) * Ball.VELOCITY;
            Player.yBallVelocity = (float)Math.Sin(alphaRadians) * Ball.VELOCITY;
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

        public bool GameOverCheck()
        {
            foreach(Rectangle rect in Rectangles)
            {
                if (rect.LeftTopPoint.Y > yGameOverCheck)
                {
                    return true;
                }
            }
            return false;
        }
        public void GenerateObjects()
        {
            int height = top + rectHeight + 3;
            int step = 80;
            int extraStep = step / 5;
            int startingPoint = left + 6;
            List<int> positions = new List<int>();

            for (int i = 0; i < objectsToGenerate; i++)
                positions.Add(i);

            //plus powerUp - generate position
            int rnd = random.Next(objectsToGenerate);
            positions.Remove(rnd);
            PowerUps.Add(powerUpFactory.GeneratePowerUp(new Point(startingPoint + rnd * step + extraStep, height), 10)); //plus powerUp

            foreach(int pos in positions)
            {
                rnd = random.Next(11);
                
                if (rnd < 7)
                {
                    Point point = new Point(startingPoint + pos * step, height);
                    Rectangles.Add(new Rectangle(point, rectWidth, rectHeight, Color.Blue, Level));
                }
                else if (rnd < 9)
                {
                    Point point = new Point(startingPoint + pos * step + extraStep, height);
                    PowerUps.Add(powerUpFactory.GeneratePowerUp(point, random.Next(10)));
                }
                //else -> empty position
            }
        }
    }
}
