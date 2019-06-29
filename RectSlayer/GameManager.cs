using RectSlayer.Properties;
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
        public int HighScore { get; set; }

        private int rectWidth;
        private int objectHeight;

        private static readonly int objectsToGenerate = 6;
        private static PowerUpsFactory powerUpFactory;
        private static Random random = new Random(DateTime.Now.Millisecond);

        private Timer shootTimer;
        private Timer gameStuckTimer;

        private bool canStartLevel;
        private bool canStartLevelIndicator;

        private bool movedShooter;

        private Point newPlayerPosition;

        private static readonly int PlayerClampDistance = 3;

        private static readonly int yGameOverCheck = 420;

        public bool isGameOver { get; set; }

        private static readonly float shootAngleLimitation = 0.066f;

        private static readonly float minAllowedVerticalVelocity = 0.3f;
        private static int ticksAllowedBeforeGameIsStuck = 10;
        private int currentTicks = 0;

        private BallColorFactory bcFactory;

        public GameManager(int left, int top, int width, int height, Timer shootTimer, Timer gameStuckTimer)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            this.shootTimer = shootTimer;
            this.gameStuckTimer = gameStuckTimer;
            rectWidth = (width / 6) - 2;
            objectHeight = 40;
            movedShooter = false;
            powerUpFactory = new PowerUpsFactory();
            bcFactory = new BallColorFactory();
            HighScore = 1;
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
            canStartLevel = false;
            canStartLevelIndicator = false;
            Player.CanShoot = true;
            isGameOver = false;
            ticksAllowedBeforeGameIsStuck = 10;
        }

        // Calculates the angle between a clicked position and the position of the player.
        private float CalculateAngle(Point mouseLocation)
        {
            int dx = mouseLocation.X - Player.Position.X;
            int dy = mouseLocation.Y - Player.Position.Y;
            float alphaRadians = (float)Math.Atan2(dy, dx);

            if (alphaRadians > -shootAngleLimitation)
                return -1;

            if (alphaRadians < -Math.PI + shootAngleLimitation)
                return -1;

            return alphaRadians;
        }

        // Calculates the shooting angle and starts the shooting timer.
        // Shooting is not allowed if the player tries to shoot down(or not too high).
        public void StartShooting(Point mouseLocation)
        {
            if (!Player.CanShoot)
                return;

            float alphaRadians = CalculateAngle(mouseLocation);

            if (alphaRadians == -1)
                return;

            Player.CanShoot = false;

            shootTimer.Start();
            gameStuckTimer.Start();
            currentTicks = 0;


            Player.xBallVelocity = (float)Math.Cos(alphaRadians) * Ball.VELOCITY;
            Player.yBallVelocity = (float)Math.Sin(alphaRadians) * Ball.VELOCITY;
        }

        // Called by the shootTimer tick method.
        // Shoots one ball on every tick, when the last ball is shot it stops the shootTimer.
        public void ShootBall()
        {
            int clrRand = random.Next(0, 60);
            Ball b = Player.ShootBall(bcFactory.GenerateColor(clrRand));
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

        // Handles ball movement, collision checking, level ending, etc.
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

        // Checks for ball-rectangles collision and removes dead rectangles.
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

        //Checks for powerUp collision and activates the powerUps.
        private void CheckPowerUpCollision(Ball ball)
        {
            for (int i = PowerUps.Count - 1; i >= 0; --i)
            {
                var powerUp = PowerUps.ElementAt(i);
                if (powerUp.CheckCollision(ball))
                {
                    if (powerUp.ActivatePowerUp(ball))
                    {
                        if (powerUp.GetType() == typeof(PlusPowerUp))
                        {
                            Player.IncreaseBallsToShoot();
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

        // Checks for dead balls and removes them.
        // Gets the new position of the shooter from the first dead ball.
        private void CheckDeadBalls()
        {
            for (int i = Balls.Count - 1; i >= 0; --i)
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
                        else if (xNewPos > left + width - PlayerClampDistance - Player.ShooterImage.Width)
                        {
                            xNewPos = left + width - PlayerClampDistance - Player.ShooterImage.Width;
                        }
                        newPlayerPosition = new Point(xNewPos, Player.Position.Y);
                        movedShooter = !movedShooter;
                    }
                    Balls.RemoveAt(i);
                }
            }
        }

        // If a ball has too low vertical velocity, it generates a powerUp that moves
        // the ball in a random direction to unstuck the ball.
        public void CheckForStuckBalls()
        {
            currentTicks++;

            if (currentTicks < ticksAllowedBeforeGameIsStuck)
                return;

            currentTicks -= 5;

            foreach (Ball ball in Balls)
            {
                if (Math.Abs(ball.VelocityY) < minAllowedVerticalVelocity)
                {
                    Point nextBallPosition = new Point(
                        ball.Center.X + (int)ball.VelocityX - Ball.RADIUS,
                        ball.Center.Y + (int)ball.VelocityY - Ball.RADIUS);

                    PowerUps.Add(powerUpFactory.GeneratePowerUp(nextBallPosition, "randomdirection"));
                    break;
                }
            }
        }

        // Handles level change.
        private void IncreaseLevel()
        {
            if (!canStartLevel)
                return;

            gameStuckTimer.Stop();
            movedShooter = false;
            canStartLevel = false;
            ++Level;

            if (Level % 10 == 0)
                ++ticksAllowedBeforeGameIsStuck;

            MoveRectangles();
            MovePowerUps();
            GenerateObjects();

            if (newPlayerPosition != null)
            {
                Player.Relocate(newPlayerPosition);
            }

            if (GameOverCheck())
            {
                isGameOver = true;
            }

            // if current level is higher than highscore -> change highscore
            HighScore = Level > HighScore ? Level : HighScore; 
        }

        // Moves the rectangles.
        private void MoveRectangles()
        {
            foreach (Rectangle rect in Rectangles)
            {
                rect.LeftTopPoint = new Point(rect.LeftTopPoint.X, rect.LeftTopPoint.Y + objectHeight + 2);
            }
        }

        // Moves the powerUps and removes powerUps that are used or in the bottom row.
        private void MovePowerUps()
        {
            for(int i=PowerUps.Count - 1; i >= 0; --i)
            {
                var powerUp = PowerUps.ElementAt(i);
                if(powerUp.IsUsed)
                {
                    PowerUps.RemoveAt(i);
                }
                else
                {
                    powerUp.LeftTopPoint = new Point(powerUp.LeftTopPoint.X, powerUp.LeftTopPoint.Y + objectHeight + 2);
                    powerUp.CalculateCenter();

                    if (powerUp.LeftTopPoint.Y > yGameOverCheck)
                    {
                        PowerUps.RemoveAt(i);
                    }
                }
            }
        }

        // Randomly generates rectangles and powerUps on every level.
        // First it chooses a position for the plus powerUp, then 
        // for the remaining positions randomly chooses if it will be a rectangle or powerUp or empty space.
        // High probability for a rectangle, low probability for a powerUp or empty space.
        public void GenerateObjects()
        {
            int height = top + objectHeight + 3;
            int step = 80;
            int extraStep = step / 5;
            int startingPoint = left + 6;
            List<int> positions = new List<int>();

            for (int i = 0; i < objectsToGenerate; i++)
                positions.Add(i);

            // plus powerUp - generate position
            int rnd = random.Next(objectsToGenerate);
            positions.Remove(rnd);
            Point plusPosition = new Point(startingPoint + rnd * step + extraStep, height);
            PowerUps.Add(powerUpFactory.GeneratePowerUp(plusPosition, "plus")); //plus powerUp

            foreach (int pos in positions)
            {
                rnd = random.Next(11);

                if (rnd < 7)
                {
                    Point point = new Point(startingPoint + pos * step, height);
                    Rectangles.Add(new Rectangle(point, rectWidth, objectHeight, Color.Blue, Level));
                }
                else if (rnd < 9)
                {
                    Point point = new Point(startingPoint + pos * step + extraStep, height);
                    PowerUps.Add(powerUpFactory.GeneratePowerUp(point, random.Next(10)));
                }
                //else -> empty position
            }
        }

        // Checks if any rectangle is in the bottom row.
        public bool GameOverCheck()
        {
            foreach (Rectangle rect in Rectangles)
            {
                if (rect.LeftTopPoint.Y > yGameOverCheck)
                {
                    return true;
                }
            }
            return false;
        }

        // Draws the game.
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


        // Helper method, called when horizontal/vertical hit powerups are used.
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
        
        // Draws the indicator line when the player can shoot.
        public void DrawIndicatorLine(Graphics g, Point mouseLocation)
        {
            float alphaRadians = CalculateAngle(mouseLocation);

            if (alphaRadians == -1)
                return;

            int mouseX = mouseLocation.X;
            int mouseY = mouseLocation.Y;

            if (mouseLocation.Y < top)
                mouseY = top;

            if (mouseLocation.X > left + width)
                mouseX = left + width;
            else if (mouseLocation.X < left)
                mouseX = left;

            Point newMouseLocation = new Point(mouseX, mouseY);

            Player.DrawLine(g, newMouseLocation);
        }

    }
}
