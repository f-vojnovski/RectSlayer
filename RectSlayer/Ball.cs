using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectSlayer
{
    public class Ball
    {
        public Point Center { get; set; }

        public static readonly int RADIUS = 10;

        public static readonly float VELOCITY = 4.4f;

        public float VelocityX { get; set; }

        public float VelocityY { get; set; }

        public Color CurrentColor { get; set; }

        public bool IsDead { get; private set; }

        private float xPos, yPos;

        public Ball(Point center, float velocityX, float velocityY, Color color)
        {
            this.Center = center;
            this.VelocityX = velocityX;
            this.VelocityY = velocityY;
            this.CurrentColor = color;
            this.IsDead = false;
            xPos = Center.X;
            yPos = Center.Y;
        }

        public void Draw(Graphics g)
        {
            Brush brush = new SolidBrush(CurrentColor);
            g.FillEllipse(brush, Center.X - RADIUS, Center.Y - RADIUS, RADIUS * 2, RADIUS * 2);
            brush.Dispose();
        }

        public void Move(int left, int top, int width, int height)
        {
            float x = xPos + VelocityX;
            float y = yPos + VelocityY;

            if(x - RADIUS <= left || x + RADIUS >= left + width)
            {
                VelocityX *= -1;
            }

            if(y - RADIUS <= top)
            {
                VelocityY *= -1;
            }
            
            if(y + RADIUS >= top + height)
            {
                IsDead = true;
            }

            xPos = xPos + VelocityX;
            yPos = yPos + VelocityY;
            int newX = (int)Math.Round((xPos));
            int newY = (int)Math.Round((yPos));

            Center = new Point(newX, newY);
        }

        public bool CheckCollision(Rectangle rectangle)
        {
            float cx = xPos;
            float cy = yPos;

            float rx = rectangle.LeftTopPoint.X;
            float ry = rectangle.LeftTopPoint.Y;

            float rw = rectangle.Width;
            float rh = rectangle.Height;

            float testX = cx;
            float testY = cy;

            bool changeHorizontalVelocity = false;
            bool changeVerticalVelocity = false;

            float xMoveFactor = 0f, yMoveFactor = 0f;


            if (cx < rx)
            {
                testX = rx;
                xMoveFactor = -6;
                changeHorizontalVelocity = true;
            }
            else if (cx > rx + rw)
            {
                xMoveFactor = 6;
                testX = rx + rw;     
                changeHorizontalVelocity = true;
            }

            if (cy < ry)
            {
                yMoveFactor = -6;
                testY = ry;
                changeVerticalVelocity = true;

            }
            else if (cy > ry + rh)
            {
                xMoveFactor = 6;
                testY = ry + rh;
                changeVerticalVelocity = true;

            }
            float distX = cx - testX;
            float distY = cy - testY;
            float distance = (float)Math.Sqrt((distX * distX) + (distY * distY));

            if (distance <= RADIUS + 1)
            {

                if (changeVerticalVelocity && !changeHorizontalVelocity)
                {
                    VelocityY *= -1;
                }
                else if (changeHorizontalVelocity && !changeVerticalVelocity)
                {
                    VelocityX *= -1;
                }
                else if (changeHorizontalVelocity && changeVerticalVelocity)
                {
                    float x  = xPos;
                    float y = yPos;
                    float c = -2 * (VelocityX * x + VelocityY * y) / (x * x + y * y);
                    VelocityX = VelocityX + c * xPos;
                    VelocityY = VelocityY + c * yPos;
                }

                xPos += xMoveFactor;
                yPos += yMoveFactor;

                rectangle.HitsRemaining--;

                return true;
            }
            return false;

        }
    }
}
