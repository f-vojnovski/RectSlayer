using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectSlayer
{
    class PlusPowerUp : PowerUp
    {
        public PlusPowerUp(Point center, Image image) : base(center,image)
        {

        }
        public override void UsePowerUp(Ball ball)
        {
            throw new NotImplementedException();
        }
    }
}
