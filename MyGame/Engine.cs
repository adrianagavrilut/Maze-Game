using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MyGame
{
    public static class Engine
    {
        public static Panel panel;
        public static Timer timer;
        public static Graphics graphics;
        public static Bitmap bitmap;
        public static Random rnd;
        public static Hero hero;
        public static List<Enemy> enemies = new List<Enemy>();
        public static List<PictureBox> walls = new List<PictureBox>();
        public static List<PictureBox> way = new List<PictureBox>();
        public static List<PictureBox> mazeEnd = new List<PictureBox>();
        public static List<Tuple<PictureBox, int>> chances = new List<Tuple<PictureBox, int>>();
        public static int[,] matrix;
        public static int n;// = 18;
        public static int m;// = 33;
        public static Point startPoint; 
        public static Point endPoint; 
        public static double time = 2500;
        public static int directionLeft = 0;
        public static int directionRight = 1;
        public static int directionUp = 2;
        public static int directionDown = 3;

        public static void Initialize(Panel panel, Timer timer)
        {
            rnd = new Random();
            Engine.panel = panel;
            Engine.timer = timer;
            hero = new Hero(40, 0);
            bitmap = new Bitmap(panel.Width, panel.Height);
            graphics = Graphics.FromImage(bitmap);
            n = bitmap.Height / 40;
            m = bitmap.Width / 40;
            startPoint = new Point(0, 1);
            endPoint = new Point(n - 1, m - 2);
        }

        public static void GenerateMatrix()
        {
            GenerateWalls();
            DrawMaze();
        }

        private static void GenerateWalls()
        {
            matrix = new int[n, m];
            for (int i = 0; i < m - 1; i++)
                matrix[0, i] = 1;
            for (int i = 0; i < n - 1; i++)
                matrix[i, m - 1] = 1;
            for (int i = m - 1; i > 0; i--)
                matrix[n - 1, i] = 1;
            for (int i = n - 1; i > 0; i--)
                matrix[i, 0] = 1;
            matrix[startPoint.X, startPoint.Y] = -1;
            matrix[endPoint.X, endPoint.Y] = -2;

            int currentPlacedWall = 0;
            int nrWalls = (n * m) / 3;
            while (currentPlacedWall < nrWalls)
            {
                int x, y;
                do
                {
                    x = rnd.Next(1, n - 1);
                    y = rnd.Next(1, m - 1);

                } while (matrix[x, y] != 0);
                matrix[x, y] = 1;
                if (FindPathLee(startPoint, endPoint))
                {
                    currentPlacedWall++;
                }
                else
                {
                    matrix[x, y] = 0;
                }
            }
        }

        public static void DrawMaze()
        {
            //matrix = ReadMatrixFromFile(@"..\..\TextFile1.txt");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        PictureBox pctBoxMazeWall = new PictureBox
                        {
                            Parent = panel,
                            Image = Image.FromFile(@"..\..\resources\wall_small.JPG"),
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Size = new Size(40, 40),
                            Location = new Point(j * 40, i * 40)
                        };
                        walls.Add(pctBoxMazeWall);
                        panel.Controls.Add(pctBoxMazeWall);
                    }
                    else if (matrix[i, j] == 0)
                    {
                        PictureBox pctBoxMazeWay = new PictureBox
                        {
                            Parent = panel,
                            BackColor = Color.Transparent,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Size = new Size(40, 40),
                            Location = new Point(j * 40, i * 40)
                        };
                        way.Add(pctBoxMazeWay);
                        panel.Controls.Add(pctBoxMazeWay);
                    }
                    else if (matrix[i, j] == -1)
                    {
                        PictureBox pctBoxMazeStart = new PictureBox
                        {
                            Parent = panel,
                            Image = Image.FromFile(@"..\..\resources\start.png"),
                            Size = new Size(40, 40),
                            Location = new System.Drawing.Point(j * 40, i * 40)
                        };
                        panel.Controls.Add(pctBoxMazeStart);
                    }
                    else if (matrix[i, j] == -2)
                    {
                        PictureBox pctBoxMazeEnd = new PictureBox
                        {
                            Parent = panel,
                            Image = Image.FromFile(@"..\..\resources\end.png"),
                            Size = new Size(40, 40),
                            Location = new System.Drawing.Point(j * 40, i * 40)
                        };
                        mazeEnd.Add(pctBoxMazeEnd);
                        panel.Controls.Add(pctBoxMazeEnd);
                    }
                }
            }
        }

        public static bool FindPathLee(Point start, Point end)
        {            
            int[,] matrixCopy = GetMatrixCopy(matrix);

            Queue A = new Queue();
            A.Push(new TriData(start.X, start.Y, 2));
            matrixCopy[start.X, start.Y] = 2;

            while (!A.IsEmpty())
            {
                TriData t = A.Pop();
                if (t.l - 1 >= 0 && matrixCopy[t.l - 1, t.c] == 0)//N
                {
                    A.Push(new TriData(t.l - 1, t.c, t.v + 1));
                    matrixCopy[t.l - 1, t.c] = t.v + 1;
                }
                else if (t.l - 1 == end.X && t.c == end.Y)
                    return true;
                if (t.l + 1 < n && matrixCopy[t.l + 1, t.c] == 0)//S
                {
                    A.Push(new TriData(t.l + 1, t.c, t.v + 1));
                    matrixCopy[t.l + 1, t.c] = t.v + 1;
                }
                else if (t.l + 1 == end.X && t.c == end.Y)
                    return true;
                if (t.c - 1 >= 0 && matrixCopy[t.l, t.c - 1] == 0)//V
                {
                    A.Push(new TriData(t.l, t.c - 1, t.v + 1));
                    matrixCopy[t.l, t.c - 1] = t.v + 1;
                }
                else if (t.l == end.X && t.c - 1 == end.Y)
                    return true;
                if (t.c + 1 < m && matrixCopy[t.l, t.c + 1] == 0)//E
                {
                    A.Push(new TriData(t.l, t.c + 1, t.v + 1));
                    matrixCopy[t.l, t.c + 1] = t.v + 1;
                }
                else if (t.l == end.X && t.c + 1 == end.Y)
                    return true;
            }
            return false;
        }

        public static int[,] getMatrixForPathLee(Point start, Point end)
        {
            int[,] matrixCopy = GetMatrixCopy(matrix);

            Queue A = new Queue();
            A.Push(new TriData(start.X, start.Y, 2));
            matrixCopy[start.X, start.Y] = 2;

            while (!A.IsEmpty())
            {
                TriData t = A.Pop();
                if (t.l - 1 >= 0 && matrixCopy[t.l - 1, t.c] == 0)//N
                {
                    A.Push(new TriData(t.l - 1, t.c, t.v + 1));
                    matrixCopy[t.l - 1, t.c] = t.v + 1;
                }
                if (t.l + 1 < n && matrixCopy[t.l + 1, t.c] == 0)//S
                {
                    A.Push(new TriData(t.l + 1, t.c, t.v + 1));
                    matrixCopy[t.l + 1, t.c] = t.v + 1;
                }
                if (t.c - 1 >= 0 && matrixCopy[t.l, t.c - 1] == 0)//V
                {
                    A.Push(new TriData(t.l, t.c - 1, t.v + 1));
                    matrixCopy[t.l, t.c - 1] = t.v + 1;
                }
                if (t.c + 1 < m && matrixCopy[t.l, t.c + 1] == 0)//E
                {
                    A.Push(new TriData(t.l, t.c + 1, t.v + 1));
                    matrixCopy[t.l, t.c + 1] = t.v + 1;
                }
            }
            return matrixCopy;
        }

        public static List<Point> GetPathLee(Point start, Point end)
        {
            List<Point> pathLee = new List<Point>();
            int[,] matrixCpy = getMatrixForPathLee(start, end);
            TriData crnt = new TriData(end.X, end.Y, matrixCpy[end.X, end.Y]);
            pathLee.Add(end);
            while (crnt.v > 2)
            {
                if (crnt.l - 1 >= 0 && matrixCpy[crnt.l - 1, crnt.c] == crnt.v - 1)
                {
                    pathLee.Add(new Point(crnt.l - 1, crnt.c));
                    crnt.l--;
                }
                else if (crnt.l + 1 < Engine.n && matrixCpy[crnt.l + 1, crnt.c] == crnt.v - 1)
                {
                    pathLee.Add(new Point(crnt.l + 1, crnt.c));
                    crnt.l++;
                }
                else if (crnt.c - 1 >= 0 && matrixCpy[crnt.l, crnt.c - 1] == crnt.v - 1)
                {
                    pathLee.Add(new Point(crnt.l, crnt.c - 1));
                    crnt.c--;
                }
                else if (crnt.c + 1 < Engine.m && matrixCpy[crnt.l, crnt.c + 1] == crnt.v - 1)
                {
                    pathLee.Add(new Point(crnt.l, crnt.c + 1));
                    crnt.c++;
                }
                crnt.v--;
            }
            return pathLee;
        }

        public static int[,] GetMatrixCopy(int[,] mtrx)
        {
            int[,] cpy = new int[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    cpy[i, j] = mtrx[i, j];
                }
            }
            return cpy;
        }

        private static int[,] ReadMatrixFromFile(string fileName)
        {
            TextReader load = new StreamReader(fileName);
            List<string> lines = new List<string>();
            string line;
            while ((line = load.ReadLine()) != null)
                lines.Add(line);
            load.Close();
            int n = lines.Count;
            int m = lines[0].Split(' ').Length;
            int[,] a = new int[n, m];
            for (int i = 0; i < n; i++)
            {
                string[] buffer = lines[i].Split(' ');
                for (int j = 0; j < m; j++)
                    a[i, j] = int.Parse(buffer[j]);
            }
            return a;
        }

        public static void GenerateEnemies()
        {
            enemies.Add(new Enemy());
            enemies.Add(new Enemy());
            enemies.Add(new Enemy());
            enemies.Add(new Enemy());
            enemies.Add(new Enemy());
        }

        public static void MoveEnemies(int move)
        {
            for (int i = 0; i < enemies.Count; i++)
                enemies[i].Move(move);
        }

        public static bool IsCollisionWithWall(Rectangle rectangle)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                if (rectangle.IntersectsWith((walls[i] as PictureBox).Bounds))
                {
                    return true;
                }
            }
            return false;
        }

        public static void DecreaseTime()
        {
            time--;
        }

        public static void RepositionHero()
        {
            hero.image.Location = new Point(40, 0);
        }

        public static bool CheckIfYouLose()
        {
            if (time <= 0)
            {
                timer.Enabled = false;
                MessageBox.Show("Time is up!", "You Lost!");
                return true;
            }
            return false;
        }

        internal static bool CheckIfYouWin()
        {
            if (mazeEnd.Any())
            {
                if (hero.image.Bounds.IntersectsWith((mazeEnd[0] as PictureBox).Bounds))
                {
                    timer.Enabled = false;
                    MessageBox.Show("Congratulations! You escaped in time!", "You Won!");
                    return true;
                }
            }
            return false;
        }

        public static void PlaceChances()
        {
            if (time % 100 == 0)
            {
                int position = rnd.Next(way.Count);
                int rndChance = rnd.Next(4);
                if (rndChance == 0)
                {
                    way[position].Image = Image.FromFile(@"..\..\resources\time_add.png");
                    chances.Add(new Tuple<PictureBox, int>(way[position], 0));
                }
                else if (rndChance == 1)
                {
                    way[position].Image = Image.FromFile(@"..\..\resources\time_minus.png");
                    chances.Add(new Tuple<PictureBox, int>(way[position], 1));
                    
                }
                else if (rndChance == 2)
                {
                    way[position].Image = Image.FromFile(@"..\..\resources\speed_add.png");
                    chances.Add(new Tuple<PictureBox, int>(way[position], 2));
                }
                else if (rndChance == 3)
                {
                    way[position].Image = Image.FromFile(@"..\..\resources\speed_minus.png");
                    chances.Add(new Tuple<PictureBox, int>(way[position], 3));
                    /*PictureBox pctBoxReduceSpeed = new PictureBox
                    {
                        Parent = panel,
                        Image = Image.FromFile(@"..\..\resources\speed_minus.png"),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Size = new Size(40, 40),
                    };
                    way[position] = pctBoxReduceSpeed;
                    chances.Add(pctBoxReduceSpeed);
                    panel.Controls.Add(pctBoxReduceSpeed);*/
                }
            }
        }

        public static void CheckCollisionWithChance()
        {
            for (int i = 0; i < chances.Count; i++)
            {
                if (chances[i].Item2 == 0)
                    {
                    if (hero.image.Bounds.IntersectsWith(chances[i].Item1.Bounds))//add time
                    {
                        if (time < 2000 && time > 1000)
                        {
                            time += 500;
                        }
                        else if (time < 1000)
                        {
                            time += 1000;
                        }
                        chances[i].Item1.Image = null;
                    }
                }
                else if (chances[i].Item2 == 1)
                {
                    if (hero.image.Bounds.IntersectsWith(chances[i].Item1.Bounds))//reduce ttime
                    {
                        if (time > 1000)
                        {
                            time -= 700;
                        }
                        CheckIfYouLose();
                        chances[i].Item1.Image = null;
                    }
                }
                else if (chances[i].Item2 == 2)
                {
                    if (hero.image.Bounds.IntersectsWith(chances[i].Item1.Bounds))//increase speed
                    {
                        hero.speed = 6;
                        chances[i].Item1.Image = null;
                    }
                }
                else if (chances[i].Item2 == 3)
                {
                    if (hero.image.Bounds.IntersectsWith(chances[i].Item1.Bounds))//reduce speed
                    {
                        hero.speed = 3;
                        chances[i].Item1.Image = null;
                    }
                }
            }
        }

        public static void CheckCollisionWithEnemy()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (hero.image.Bounds.IntersectsWith(enemies[i].image.Bounds))
                {
                    timer.Enabled = false;
                    MessageBox.Show("You were killed! Don't meet with the enemies!", "You Lost!");
                }
            }
        }
    }
}
