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

        public static readonly float VELOCITY = 15;
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public Color CurrentColor { get; set; }

        public Ball(Point center, float velocityX, float velocityY, Color color)
        {
            this.Center = center;
            this.VelocityX = velocityX;
            this.VelocityY = velocityY;
            this.CurrentColor = color;
        }

        public void Draw(Graphics g)
        {
            Brush brush = new SolidBrush(CurrentColor);
            g.FillEllipse(brush, Center.X - RADIUS, Center.Y - RADIUS, RADIUS * 2, RADIUS * 2);
            brush.Dispose();
        }

        public void Move(int left, int top, int width, int height)
        {
            float x = Center.X + VelocityX;
            float y = Center.Y + VelocityY;

            if(x - RADIUS <= left || x + RADIUS >= left + width)
            {
                VelocityX *= -1;
            }

            if (y - RADIUS <= top || y + RADIUS >= top + height)
            {
                VelocityY *= -1;
            }

            int newX = (int)(Center.X + VelocityX);
            int newY = (int)(Center.Y + VelocityY);

            Center = new Point(newX, newY);
        }

        public bool CheckCollision(Rectangle rectangle)
        {
            float cx = Center.X;
            float cy = Center.Y;

            float rx = rectangle.LeftTopPoint.X;
            float ry = rectangle.LeftTopPoint.Y;

            float rw = rectangle.Width;
            float rh = rectangle.Height;

            float testX = cx;
            float testY = cy;

            // TODO: Implement a way to know which side is hit

            if (cx < rx)
            {
                // TODO: The ball hit the rect from left
                // TODO: Multiply X vel by -1
                testX = rx;         // left edge
            }
            else if (cx > rx + rw)
            {
                // TODO: The ball hit the rect from right
                // TODO: Multiply X vel by -1
                testX = rx + rw;     // right edge
            }

            if (cy < ry)
            {
                // TODO: The ball hit the rect from above
                // TODO: Multiply Y vel by -1   
                testY = ry;         // top edge
            }
            else if (cy > ry + rh)
            {
                // TODO: The ball hit the rect from below
                // TODO: Multiply Y vel by -1   
                testY = ry + rh;     // bottom edge
            }
            float distX = cx - testX;
            float distY = cy - testY;
            float distance = (float)Math.Sqrt((distX * distX) + (distY * distY));

            if (distance <= RADIUS)
            {
                return true;
            }
            return false;

        }
    }
}
