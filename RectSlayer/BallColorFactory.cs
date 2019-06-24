using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectSlayer
{
    class BallColorFactory
    {
        public Color GenerateColor(int type)
        {
            if (type <= 10)
            {
                return Color.FromArgb(163, 255, 160);
            }
            else if (type <= 20)
            {
                return Color.FromArgb(239, 160, 255);

            }
            else if (type <= 30)
            {
                return Color.FromArgb(48, 255, 100);

            }
            else if (type <= 40)
            {
                return Color.FromArgb(255, 250, 0);

            }
            else if (type <= 50)
            {
                return Color.FromArgb(145, 229, 255);

            }
            else
            {
                return Color.FromArgb(255, 255, 255);
            }
        }
    }
}
