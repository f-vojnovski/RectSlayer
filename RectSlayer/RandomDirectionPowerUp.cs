using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectSlayer
{
    class RandomDirectionPowerUp : PowerUp
    {
        private static Random random = new Random(DateTime.Now.Millisecond);

        public RandomDirectionPowerUp(Point center, Image image) : base(center, image)
        {

        }

        public override void UsePowerUp(Ball ball)
        {
            throw new NotImplementedException();
        }
    }
}
