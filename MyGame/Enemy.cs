using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyGame
{
    public class Enemy : Player
    {
        List<Point> path;
        int crtPosition = 0;
        int direction = 1;
        Point startPoint;
        Point endPoint;

        public Enemy() : base()
        {
            speed = 3;
            DeterminePath();
            image = new PictureBox
            {
                Parent = Engine.panel,
                Location = new Point(endPoint.Y * 40, endPoint.X * 40),
                Size = new Size(30, 30),
                Image = Image.FromFile(@"..\..\resources\enemy.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            image.BringToFront();
        }

        public void DeterminePath()
        {
            int startIndex;
            int endIndex;
            do
            {
                startIndex = Engine.rnd.Next(0, Engine.way.Count());
                endIndex = Engine.rnd.Next(0, Engine.way.Count());
                startPoint = new Point(Engine.way[startIndex].Location.Y / 40, Engine.way[startIndex].Location.X / 40);
                endPoint = new Point(Engine.way[endIndex].Location.Y / 40, Engine.way[endIndex].Location.X / 40);
            
            } while (!Engine.FindPathLee(startPoint, endPoint) || startIndex == endIndex);
            path = Engine.GetPathLee(startPoint, endPoint);
        }

        public void Move(int move)
        {
            if(path.Count > 2)
            {
                if (crtPosition == path.Count() - 1)
                    direction = -1;
                if (crtPosition == 1)
                    direction = 1;

                int newPosition = crtPosition + direction;

                if (move == 0)
                {
                    crtPosition = newPosition;
                }
                int newLocationY = (move * (path[crtPosition].Y * 40) + (16-move) * (path[newPosition].Y * 40)) / 16;
                int newLocationX = (move * (path[crtPosition].X * 40) + (16-move) * (path[newPosition].X * 40)) / 16;

                image.Location = new Point(newLocationY, newLocationX);
            }
        }
    }
}
