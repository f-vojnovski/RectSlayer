﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectSlayer
{
    public class VerticalHitPowerUp : PowerUp
    {
        public VerticalHitPowerUp(Point center, Image image) : base(center, image)
        {

        }

        // Hit every rectangle that is in the same column.
        public void Hit(List<Rectangle> rectangles)
        {
            for(int i=rectangles.Count-1; i>=0; i--)
            {
                var rect = rectangles.ElementAt(i);
                if(Math.Abs(rect.LeftTopPoint.X - this.LeftTopPoint.X) < rect.Width/2)
                {
                    rect.HitsRemaining--;

                    if(rect.HitsRemaining <= 0)
                    {
                        rectangles.RemoveAt(i);
                    }
                }
            }
        }
    }
}
