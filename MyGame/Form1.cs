using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace MyGame
{
    public partial class Form1 : Form
    {
        public Image background = Image.FromFile(@"..\..\resources\backgroundG.png");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panelGame.Width = this.Width;
            panelGame.Height = this.Height - 80;
            panelGame.Visible = false;
            panelGame.Enabled = false;
            panelGame.Location = new Point(0, 80);

            panelTopMenu.Width = this.Width;
            panelTopMenu.Height = 80;
            panelTopMenu.Visible = false;
            panelTopMenu.Enabled = false;
            panelTopMenu.Location = new Point(0, 0);

            pctBoxMenuScreen.Width = this.Width;
            pctBoxMenuScreen.Height = this.Height;

            buttonStart.Parent = buttonExit.Parent = pctBoxMenuScreen;
            buttonStart.Left = buttonExit.Left = this.Width -  2 *  buttonStart.Width + 150;
            buttonStart.Top = this.Height - buttonStart.Height * 4;
            buttonExit.Top = this.Height - buttonStart.Height * 2;

            labelTask.Parent = pctBoxMenuScreen;
            labelTask.Left = 500;

            pictureBoxWin.Parent = panelGame;
            pictureBoxWin.Width = this.Width;
            pictureBoxWin.Height = this.Height;
            pictureBoxWin.Visible = false;
            pictureBoxWin.Enabled = false;

            DoubleBuffered = true;
            Engine.Initialize(panelGame, timer1);
            Engine.GenerateMatrix();
            Engine.GenerateEnemies();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Engine.DecreaseTime();
            labelTime.Text = (Math.Round(Engine.time / 100, 1)).ToString();
            Engine.hero.Move();
            labelPosition.Text = Engine.hero.GetMatrixPosition().ToString();
            Engine.MoveEnemies((int)Engine.time % 16);
            if (Engine.CheckIfYouWin())
            {
                pictureBoxWin.Visible = true;
                pictureBoxWin.Enabled = true;
                buttonRestart.Enabled = false;
                buttonRestart.Visible = false;
            }
            Engine.PlaceChances();
            Engine.CheckCollisionWithChance();
            Engine.CheckCollisionWithEnemy();
            if (Engine.CheckIfYouLose())
            {
                buttonRestart.Enabled = false;
                buttonRestart.Visible = false;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                timer1.Enabled = false;
                var option = MessageBox.Show("Are you sure you want to exit this game? Your progress will be lost", "Exit Game", MessageBoxButtons.OKCancel);
                if (option == DialogResult.OK)
                {
                    Close();
                }
                timer1.Enabled = true;
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
                Engine.hero.isMovingLeft = true;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
                Engine.hero.isMovingRight = true;
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
                Engine.hero.isMovingUp = true;
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
                Engine.hero.isMovingDown = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
                Engine.hero.isMovingLeft = false;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
                Engine.hero.isMovingRight = false;
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
                Engine.hero.isMovingUp = false;
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
                Engine.hero.isMovingDown = false;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = background;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            timer1.Enabled = true;
            pctBoxMenuScreen.Enabled = false;
            pctBoxMenuScreen.Visible = false;
            panelGame.Enabled = true;
            panelGame.Visible = true;
            panelTopMenu.Enabled = true;
            panelTopMenu.Visible = true;
            labelTask.Visible = false;
        }

        private void buttonExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonRestart_Click(object sender, EventArgs e)
        {
            Engine.RepositionHero();
            Engine.time = 2500;
            timer1.Enabled = true;
            panelGame.Focus();
            Engine.hero.isMovingDown = false;
            Engine.hero.isMovingLeft = false;
            Engine.hero.isMovingRight = false;
            Engine.hero.isMovingUp = false;
        }

        private void buttonNewGame_Click(object sender, EventArgs e)
        {
            buttonRestart.Enabled = true;
            buttonRestart.Visible = true;
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Process.Start(exePath);
            Environment.Exit(Environment.ExitCode);
        }
    }
}
