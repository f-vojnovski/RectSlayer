using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectSlayer
{
    abstract class PowerUp
    {
        public Point Center { get; set; }

        public static readonly int RADIUS = 10;

        public Image PowerUpImage { get; set; }

        public bool IsUsed { get; set; }

        public PowerUp(Point center, Image image)
        {
            this.Center = center;
            this.PowerUpImage = image;
            IsUsed = false;
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(PowerUpImage, Center);
        }

        abstract public void UsePowerUp(Ball ball);
    }
}
