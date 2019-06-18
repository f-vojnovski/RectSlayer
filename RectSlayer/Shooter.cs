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

        public int BallsToShoot { get; set; }

        public int BallsShot { get; set; }

        public bool CanShoot { get; set; }

        public float xBallVelocity { get; set; }
        public float yBallVelocity { get; set; }
        public Shooter(Point position)
        {
            this.Position = position;
            this.BallsToShoot = 2;
            this.BallsShot = 0;
            this.CanShoot = true;
            xBallVelocity = 0;
            yBallVelocity = 0;
        }

        // a ball hit the + power up
        public void IncreaseCBalls()
        {
            ++BallsToShoot;
        }

        //will be called in timer_tick, because we don't want to shoot all the balls at once
        //CanShoot goes false when the last ball is shot, it will go back to true when all the balls are dead
        // TODO: Reimplement later when GameManager is finished
        public Ball ShootBall(Color color)
        {
            ++BallsShot;
            Point testP = new Point(Position.X, Position.Y);
            return new Ball(testP, xBallVelocity, yBallVelocity, color);

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
