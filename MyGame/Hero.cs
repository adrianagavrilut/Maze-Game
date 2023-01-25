using System.Drawing;
using System.Windows.Forms;

namespace MyGame
{
    public class Hero : Player
    {
        public Hero(int x, int y) : base()
        {
            image = new PictureBox
            {
                Parent = Engine.panel,
                Location = new Point(x, y),
                Size = new Size(30, 30),
                BackColor = Color.Transparent,
                Image = Image.FromFile(@"..\..\resources\player.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
        }

        public void Move()
        {
            if (isMovingLeft)
                base.Move(Engine.directionLeft);
            else if (isMovingRight)
                base.Move(Engine.directionRight);
            else if (isMovingUp)
                base.Move(Engine.directionUp);
            else if (isMovingDown)
                base.Move(Engine.directionDown);
        }
    }
}
