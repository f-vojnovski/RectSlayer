using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectSlayer
{
    public class RandomDirectionPowerUp : PowerUp
    {
        private static Random random = new Random(DateTime.Now.Millisecond);

        public RandomDirectionPowerUp(Point center, Image image) : base(center, image)
        {

        }

        // Give the ball a random direction.
        public void RandomDirection(Ball ball)
        {
            double angle = random.NextDouble() * 2 * Math.PI;
            ball.VelocityX = (float)(Math.Cos(angle) * Ball.VELOCITY);
            ball.VelocityY = (float)(Math.Sin(angle) * Ball.VELOCITY);
        }
    }
}
