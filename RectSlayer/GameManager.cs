using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectSlayer
{
    public class GameManager
    {
        public Shooter Player{ get; set; }
        public List<Ball> Balls { get; set; }
        public List<Rectangle> Rectangles { get; set; }

        public int left { get; set; }
        public int top { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int Level { get; set; }
        private int rectWidth = 40;
        private int rectHeight = 40;

        private static readonly int objectsToGenerate = 6;

        public GameManager(int left, int top, int width, int height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            rectWidth = width / 6 - 5;
            StartGame();
        }

        public void StartGame()
        {
            Level = 1;
            Player = new Shooter(new Point(left + width / 2, top + height - 20));
            Balls = new List<Ball>();
            Rectangles = new List<Rectangle>();
            Balls.Add(new Ball(new Point(250, 250), -5, -5, Color.Red));
            GenerateRectangles();


        }

        public void GameOver()
        {

        }

        public void HandleLogic()
        {
            foreach (Ball ball in Balls)
            {

                foreach (Rectangle rect in Rectangles)
                {
                    if (ball.CheckCollision(rect)) {
                        Console.WriteLine("PRAVOAGOLNIK USPESNO DEMOLIRAN");
                        break;
                    }
                }

                ball.Move(left, top, width, height);

            }
            
            for (int i = Balls.Count - 1; i >= 0; i--)
            {
                if (Balls.ElementAt(i).IsDead)
                {
                    Balls.RemoveAt(i);
                }
            }
        }

        public void Draw(Graphics g)
        {
            foreach (Ball ball in Balls)
            {
                ball.Draw(g);
            }
            foreach(Rectangle rect in Rectangles)
            {
                rect.Draw(g);
            }
            Player.Draw(g);
        }

        public void DrawIndicatorLine(Graphics g, Point mouseLocation)
        {
            Player.DrawLine(g, mouseLocation);
        }

        public void GenerateRectangles()
        {
            int step = 80;
            int currentStartingPoint = left + 6;

            for (int i = 0; i<objectsToGenerate; ++i)
            {
                Rectangle newRect = new Rectangle(new Point(currentStartingPoint, top + rectHeight + 3), rectWidth, rectHeight,
                    Color.Blue, 1);
                Rectangles.Add(newRect);
                currentStartingPoint += step;
            }
        }
    }
}
