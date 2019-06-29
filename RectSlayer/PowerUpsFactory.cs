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
        // Generate powerUp by name
        public PowerUp GeneratePowerUp(Point position, string type)
        {
            if (type.Equals("horizontal", StringComparison.OrdinalIgnoreCase))
            {
                return new HorizontalHitPowerUp(position, Resources.horizontal);
            }
            else if (type.Equals("vertical", StringComparison.OrdinalIgnoreCase))
            {
                return new VerticalHitPowerUp(position, Resources.vertical);
            }
            else if (type.Equals("randomdirection", StringComparison.OrdinalIgnoreCase))
            {
                return new RandomDirectionPowerUp(position, Resources.random);
            }
            else if (type.Equals("plus", StringComparison.OrdinalIgnoreCase))
            {
                return new PlusPowerUp(position, Resources.plus);
            }

            return null;
        }


        // Method used to generate a random powerUp
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
