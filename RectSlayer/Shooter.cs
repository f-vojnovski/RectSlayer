﻿using RectSlayer.Properties;
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

        public Image ShooterImage { get; set; }

        public int BallsToShoot { get; set; }

        public int BallsShot { get; set; }

        public bool CanShoot { get; set; }

        public float xBallVelocity { get; set; }
        public float yBallVelocity { get; set; }

        public Shooter(Point position)
        {
            this.Position = position;
            this.BallsToShoot = 1;
            this.BallsShot = 0;
            this.CanShoot = true;
            xBallVelocity = 0;
            yBallVelocity = 0;
            ShooterImage = Resources.shooter;
        }

        // When a ball hits the + power up.
        public void IncreaseBallsToShoot()
        {
            ++BallsToShoot;
        }

        public Ball ShootBall(Color color)
        {
            ++BallsShot;
            Point position = new Point(Position.X, Position.Y);
            return new Ball(position, xBallVelocity, yBallVelocity, color);
        }

        // Move to the position of the first dead ball.
        public void Relocate(Point position)
        {
            this.Position = position;
        }

        public void Draw(Graphics g)
        {
            /*
            Brush brush = new SolidBrush(Color.Purple);
            g.FillRectangle(brush, Position.X,Position.Y, 30, 30);
            brush.Dispose();
            */

            g.DrawImage(ShooterImage,Position);
            
        }

        // Draw the indicator line.
        public void DrawLine(Graphics g, Point mouseLocation)
        {
            if (!CanShoot)
                return;

            if (mouseLocation == Point.Empty)
                return;

            Pen pen = new Pen(Color.White, 3);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            g.DrawLine(pen, Position, mouseLocation);
            pen.Dispose();
        }
    }
}
