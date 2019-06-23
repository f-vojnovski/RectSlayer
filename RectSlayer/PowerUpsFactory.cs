using RectSlayer.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectSlayer
{
    public class PowerUpsFactory
    {
        public PowerUp GeneratePowerUp(Point position, int type)
        {
            if (type < 4)
            {
                return new HorizontalHitPowerUp(position, Resources.horizontal);
            }
            else if (type < 8)
            {
                return new VerticalHitPowerUp(position, Resources.vertical);
            }
            else if (type < 10)
            {
                return new RandomDirectionPowerUp(position, Resources.random);
            }
            else
                return new PlusPowerUp(position, Resources.plus);
        }
    }
}
