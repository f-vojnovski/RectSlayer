using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectSlayer
{
    public class PowerUp
    {
        public Image PowerUpImage { get; set; }
        public Point LeftTopPoint { get; set; }

        public List<Ball> BallsInside { get; set; }

        protected float centerX;
        protected float centerY;


        public bool IsUsed { get; set; }

        public bool IsActive { get; set; }

        public PowerUp(Point center, Image image)
        {
            this.LeftTopPoint = center;
            this.PowerUpImage = image;
            CalculateCenter();
            IsUsed = false;
            IsActive = false;
            BallsInside = new List<Ball>();
        }

        // Needed for collision check.
        public void CalculateCenter()
        {
            this.centerX = LeftTopPoint.X + PowerUpImage.Width / 2f;
            this.centerY = LeftTopPoint.Y + PowerUpImage.Height / 2f;
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(PowerUpImage, LeftTopPoint);
        }

        // Check if a ball touches with the powerUp.
        public bool CheckCollision(Ball ball)
        {
            double ballX = ball.Center.X - Ball.RADIUS;
            double ballY = ball.Center.Y - Ball.RADIUS;
            double d = (centerX - ballX) * (centerX - ballX) + (centerY - ballY) * (centerY - ballY);
            double radius = PowerUpImage.Width/2.0 + Ball.RADIUS;

            return d <= radius * radius;
        }


        // Called only if a ball is touching with the powerUp.
        // If the ball is in the list then the ball has already activated the powerUp.
        // If the ball is not in the list, the powerUp will be activated and the ball added in the list.
        public bool ActivatePowerUp(Ball ball)
        {
            if(BallsInside.Contains(ball))
                return false;


            IsUsed = true;
            BallsInside.Add(ball);

            FilterBalls(); 

            return true;
        }

        // Remove balls that are no longer touching the powerup.
        public void FilterBalls()
        {
            for(int i=BallsInside.Count-1; i>=0; --i)
            {
                if(!CheckCollision(BallsInside.ElementAt(i)))
                {
                    BallsInside.RemoveAt(i);
                }
            }
        }

    }
}
