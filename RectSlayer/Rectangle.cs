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
        private readonly int maxColorHits = 20;

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
            Brush brush = new SolidBrush(CalculateColor());
            Brush lvlBrush = new SolidBrush(Color.White);
            g.FillRectangle(brush, LeftTopPoint.X, LeftTopPoint.Y, Width, Height);
            g.DrawString(HitsRemaining.ToString(), new Font("Arial", 10), lvlBrush, LeftTopPoint.X + 10, LeftTopPoint.Y + 5);
            brush.Dispose();
            lvlBrush.Dispose();
        }

        // Calculates the color based on hits remaining.
        private Color CalculateColor()
        {
            // FROM: (193, 91, 91)
            // TO:   (133, 193, 91)
            int hits = HitsRemaining;
            if (hits > maxColorHits) hits = maxColorHits;
            float ratio = (float)((float)hits / (float)maxColorHits);
            float red = 255 - (ratio * 193);
            float green = 0;
            int blue = 93;
            return Color.FromArgb((int)red, (int)green, blue);
        }
    }
}
