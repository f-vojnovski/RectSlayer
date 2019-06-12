using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectSlayer
{
    public class Rectangle
    {
        public Point LeftTopPoint { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Color CurrentColor { get; set; }
        public int HitsRemaining { get; set; }

        public Rectangle(Point leftTop, int width, int height, Color color, int hits)
        {
            this.LeftTopPoint = leftTop;
            this.Width = width;
            this.Height = height;
            this.CurrentColor = color;
            this.HitsRemaining = hits;
        }

        public void Draw(Graphics g)
        {
            Brush brush = new SolidBrush(CurrentColor);
            g.FillRectangle(brush, LeftTopPoint.X, LeftTopPoint.Y, Width, Height);
            brush.Dispose();
        }
    }
}
