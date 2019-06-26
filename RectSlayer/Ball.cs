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

        public static readonly float collisionMoveFactor = 1.9f;

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

        // Moves the ball and checks border collision
        public void Move(int left, int top, int width, int height)
        {
            float x = xPos + VelocityX;
            float y = yPos + VelocityY;

            if (x - RADIUS <= left || x + RADIUS >= left + width)
            {
                VelocityX *= -1;
            }

            if (y - RADIUS <= top)
            {
                VelocityY *= -1;
            }

            if (y + RADIUS >= top + height)
            {
                IsDead = true;
            }

            xPos = xPos + VelocityX;
            yPos = yPos + VelocityY;
            int newX = (int)Math.Round(xPos);
            int newY = (int)Math.Round(yPos);

            Center = new Point(newX, newY);
        }

        // Complex method that checks for collision and changes the direction of the ball.
        // A lot of maths involved.
        public bool CheckCollision(Rectangle rectangle)
        {
            float ballX = xPos;
            float ballY = yPos;

            float rectX = rectangle.LeftTopPoint.X;
            float rectY = rectangle.LeftTopPoint.Y;

            float rectWidth = rectangle.Width;
            float rectHeight = rectangle.Height;

            float testX = ballX;
            float testY = ballY;

            bool changeHorizontalVelocity = false;
            bool changeVerticalVelocity = false;

            float xMoveFactor = 0f, yMoveFactor = 0f;

            bool leftCornerTest = false;
            bool topCornerTest = false;

            if (ballX < rectX)
            {
                testX = rectX;
                xMoveFactor = -VelocityX * collisionMoveFactor;
                changeHorizontalVelocity = true;
                leftCornerTest = true;
            }
            else if (ballX > rectX + rectWidth)
            {
                xMoveFactor = -VelocityX * collisionMoveFactor;
                testX = rectX + rectWidth;
                changeHorizontalVelocity = true;
            }

            if (ballY < rectY)
            {
                yMoveFactor = -VelocityY * collisionMoveFactor;
                testY = rectY;
                changeVerticalVelocity = true;
                topCornerTest = true;

            }
            else if (ballY > rectY + rectHeight)
            {
                yMoveFactor = -VelocityY * collisionMoveFactor;
                testY = rectY + rectHeight;
                changeVerticalVelocity = true;

            }

            float distX = ballX - testX;
            float distY = ballY - testY;
            float distance = (float)Math.Sqrt((distX * distX) + (distY * distY));

            if (distance <= RADIUS )
            {

                if (changeVerticalVelocity && !changeHorizontalVelocity)
                {
                    VelocityY *= -1;
                    xPos += xMoveFactor;
                }
                else if (changeHorizontalVelocity && !changeVerticalVelocity)
                {
                    VelocityX *= -1;
                    yPos += yMoveFactor;

                }
                else if (changeHorizontalVelocity && changeVerticalVelocity)
                {
                    xPos += xMoveFactor * Math.Abs(VelocityX) / VELOCITY;
                    yPos += yMoveFactor * Math.Abs(VelocityY) / VELOCITY;
                    float x = xPos;
                    float y = yPos;
                    float c = -2 * (VelocityX * x + VelocityY * y) / (x * x + y * y);

                    if (leftCornerTest)
                    {
                        VelocityX = -Math.Abs(VelocityX + c * xPos);
                    } else
                    {
                        VelocityX = Math.Abs(VelocityX + c * xPos);
                    }
                    if (topCornerTest)
                    {
                        VelocityY = -Math.Abs(VelocityY + c * yPos);
                    }
                    else
                    {
                        VelocityY = Math.Abs(VelocityY + c * yPos);
                    }
                }

                rectangle.HitsRemaining--;

                return true;
            }

            return false;
        }
    }
}
