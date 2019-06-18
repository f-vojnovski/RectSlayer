using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectSlayer
{
    public class Shooter
    {
        public Point Position { get; set; }

        public int BallsCount { get; set; }

        public int BallsShot { get; set; } // shooting ball after ball

        public bool CanShoot { get; set; }

        public List<Ball> Balls { get; set; } // might get moved to GameManager or not

        public Shooter(Point position)
        {
            this.Position = position;
            this.BallsCount = 1;
            this.BallsShot = 0;
            this.CanShoot = true;
            this.Balls = new List<Ball>();
        }

        // a ball hit the + power up
        public void IncreaseCount()
        {
            ++BallsCount;
        }

        //will be called in timer_tick, because we don't want to shoot all the balls at once
        //CanShoot goes false when the last ball is shot, it will go back to true when all the balls are dead
        // TODO: Reimplement later when GameManager is finished
        public void ShootBall(float velocityX, float velocityY, Color color)
        {
            if(CanShoot)
            {
                if (BallsShot < BallsCount)
                {
                    ++BallsShot;
                    Balls.Add(new Ball(this.Position, velocityX, velocityY, color));
                }
                else
                {
                    BallsShot = 0;
                    CanShoot = false;
                }
            } 
        }

        // move to the position of the first dead ball
        // managed by GameManager?
        public void Relocate(Point position)
        {
            this.Position = position;
        }

        // TODO: Add other necessary methods

        public void Draw(Graphics g)
        {
            Brush brush = new SolidBrush(Color.Purple);
            g.FillRectangle(brush, Position.X,Position.Y, 30, 30);
        }
        public void DrawLine(Graphics g, Point mouseLocation)
        {
            if (!CanShoot)
            {
                return;
            }
            if (mouseLocation == Point.Empty)
            {
                return;
            }
            Pen pen = new Pen(Color.White, 2);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            g.DrawLine(pen, Position, mouseLocation);
            pen.Dispose();
        }
    }
}
