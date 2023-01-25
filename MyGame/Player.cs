using System;
using System.Drawing;
using System.Windows.Forms;

namespace MyGame
{
    public abstract class Player
    {
        public PictureBox image;
        public int speed = 5;
        public bool isMovingLeft;
        public bool isMovingRight;
        public bool isMovingUp;
        public bool isMovingDown;

        public Player()
        {
        }

        public void Move(int direction)
        {
            if (direction == Engine.directionLeft && !Engine.IsCollisionWithWall(new Rectangle(image.Location.X - speed, image.Location.Y, image.Size.Width, image.Size.Height)))
                image.Left -= speed;
            else if (direction == Engine.directionRight && !Engine.IsCollisionWithWall(new Rectangle(image.Location.X + speed, image.Location.Y, image.Size.Width, image.Size.Height)))
                image.Left += speed;
            else if (direction == Engine.directionUp && !Engine.IsCollisionWithWall(new Rectangle(image.Location.X, image.Location.Y - speed, image.Size.Width, image.Size.Height)))
                image.Top -= speed;
            else if (direction == Engine.directionDown && !Engine.IsCollisionWithWall(new Rectangle(image.Location.X, image.Location.Y + speed, image.Size.Width, image.Size.Height)))
                image.Top += speed;
        }

        public Tuple<int, int> GetMatrixPosition()
        {
            return new Tuple<int, int>(image.Location.Y / 40, image.Location.X / 40);
        }
    }
}
