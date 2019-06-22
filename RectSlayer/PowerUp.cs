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
        public Point LeftTopPoint { get; set; }

        public List<Ball> BallsInside { get; set; }

        protected float centerX;
        protected float centerY;


        public Image PowerUpImage { get; set; }

        public bool IsUsed { get; set; }

        public PowerUp(Point center, Image image)
        {
            this.LeftTopPoint = center;
            this.PowerUpImage = image;
            CalculateCenter();
            IsUsed = false;
            BallsInside = new List<Ball>();
        }

        public void CalculateCenter()
        {
            this.centerX = LeftTopPoint.X + PowerUpImage.Width / 2f;
            this.centerY = LeftTopPoint.Y + PowerUpImage.Height / 2f;
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(PowerUpImage, LeftTopPoint);

           /* 
            Brush br = new SolidBrush(Color.White);
            g.FillEllipse(br, centerX,centerY, 10, 10);
            br.Dispose();
            */

        }

        public bool CheckCollision(Ball ball)
        {
            double ballX = ball.Center.X - Ball.RADIUS;
            double ballY = ball.Center.Y - Ball.RADIUS;
            double d = (centerX - ballX) * (centerX - ballX) + (centerY - ballY) * (centerY - ballY);
            double radius = PowerUpImage.Width/2 + Ball.RADIUS;

            return d <= radius * radius;
        }

        public bool ActivatePowerUp(Ball ball)
        {
            if(BallsInside.Contains(ball))
            {
                return false;
            }

            IsUsed = true;

            BallsInside.Add(ball);

            FilterBalls(); //remove balls that are no longer touching the powerup

            return true;

        }

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
