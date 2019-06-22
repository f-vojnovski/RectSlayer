using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectSlayer
{
    public class HorizontalHitPowerUp : PowerUp
    {
        public HorizontalHitPowerUp(Point center, Image image) : base(center, image)
        {

        }

        public void Hit(List<Rectangle> rectangles)
        {
            for (int i = rectangles.Count-1; i >= 0; i--)
            {
                var rect = rectangles.ElementAt(i);
                if (Math.Abs(rect.LeftTopPoint.Y - this.LeftTopPoint.Y) < 10)
                {
                    rect.HitsRemaining--;

                    if (rect.HitsRemaining <= 0)
                    {
                        rectangles.RemoveAt(i);
                    }
                }
            }
        }
    }
}
