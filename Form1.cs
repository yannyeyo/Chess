using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Chess
{ // попытаться сделать пешку-королеву
    public partial class Form1 : Form
    {
        public Image chessSprites;
        public int[,] map = new int[8, 8]
        {
            {15,14,13,12,11,13,14,15 },
            {16,16,16,16,16,16,16,16 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {26,26,26,26,26,26,26,26 },
            {25,24,23,22,21,23,24,25 },
        };
        public string[,] pol = new string[8, 8]
        {
            {"a8", "b8", "c8", "d8", "e8", "f8", "g8", "h8"},
            {"a7", "b7", "c7", "d7", "e7", "f7", "g7", "h7"},
            {"a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6"},
            {"a5", "b5", "c5", "d5", "e5", "f5", "g5", "h5"},
            {"a4", "b4", "c4", "d4", "e4", "f4", "g4", "h4"},
            {"a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3"},
            {"a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2"},
            {"a1", "b1", "c1", "d1", "e1", "f1", "g1", "h1"},
        };

        //int i = 0; int j = 0;
        //int k = 5; int l = 5;
        string str = "";
        string str1 = "";

        public Button[,] butts = new Button[8, 8];

        public int currPlayer;

        public Button prevButton;

        public bool isMoving = false;

        public Form1()
        {
            InitializeComponent();

            chessSprites = new Bitmap("C:\\Users\\User\\.vscode\\chess.png");

            Init();
        }

        public void Init()
        {
            map = new int[8, 8]
            {
            {15,14,13,12,11,13,14,15 },
            {16,16,16,16,16,16,16,16 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {26,26,26,26,26,26,26,26 },
            {25,24,23,22,21,23,24,25 },
            };

            currPlayer = 1;
            CreateMap();
        }

        public void CreateMap()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j] = new Button();

                    Button butt = new Button();
                    butt.Size = new Size(50, 50);
                    butt.Location = new Point(j * 50, i * 50);
                    butt.ForeColor = Color.Red;

                    switch (map[i, j] / 10)
                    {
                        case 1:
                            Image part = new Bitmap(50, 50);
                            Graphics g = Graphics.FromImage(part);
                            g.DrawImage(chessSprites, new Rectangle(0, 0, 50, 50), 0 + 150 * (map[i, j] % 10 - 1), 0, 150, 150, GraphicsUnit.Pixel);
                            butt.BackgroundImage = part;
                            break;
                        case 2:
                            Image part1 = new Bitmap(50, 50);
                            Graphics g1 = Graphics.FromImage(part1);
                            g1.DrawImage(chessSprites, new Rectangle(0, 0, 50, 50), 0 + 150 * (map[i, j] % 10 - 1), 150, 150, 150, GraphicsUnit.Pixel);
                            butt.BackgroundImage = part1;
                            break;
                    }
                    butt.BackColor = Color.LemonChiffon;
                    butt.Click += new EventHandler(OnFigurePress);
                    this.Controls.Add(butt);

                    butts[i, j] = butt;
                }
            }
        }
        //int whitecoutn = 16;
        //int blackcoutn = 16;
        public void OnFigurePress(object sender, EventArgs e)
        {
            //if (whitecoutn == 1 && blackcoutn > 1)
            //{
            //    for (int k = 0; k < 1; k++)
            //    {
            //        DialogResult dialogResult = MessageBox.Show("Победа белых, поздравляем!", "Результат", MessageBoxButtons.OK);
            //        break;
            //    }
            //}
            //else if (blackcoutn == 1 && whitecoutn > 1)
            //{
            //    for (int k = 0; k < 1; k++)
            //    {
            //        DialogResult dialogResult = MessageBox.Show("Победа черных, поздравляем!", "Результат", MessageBoxButtons.OK);
            //        break;
            //    }
            //}
            //else
            //{
                if (prevButton != null)
                    prevButton.BackColor = Color.LemonChiffon;
                Button pressedButton = sender as Button;
                //pressedButton.Enabled = false;
                if (map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50] != 0 && map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50] / 10 == currPlayer)
                {
                    CloseSteps();
                    pressedButton.BackColor = Color.Red;
                    DeactivateAllButtons();
                    pressedButton.Enabled = true;
                    ShowSteps(pressedButton.Location.Y / 50, pressedButton.Location.X / 50, map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50]);
                    //if (currPlayer == 1)
                    //{
                    //    blackcoutn--;
                    //}
                    //if (currPlayer == 2)
                    //{
                    //    whitecoutn--;
                    //}
                    if (isMoving)
                    {
                        CloseSteps();
                        pressedButton.BackColor = Color.LemonChiffon;
                        ActivateAllButtons();
                        isMoving = false;
                    }
                    else
                        isMoving = true;
                }
                else
                {
                    if (isMoving)
                    {
                            int temp1 = map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50];
                            map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50] = map[prevButton.Location.Y / 50, prevButton.Location.X / 50];
                            if (currPlayer == 1)
                            {
                                str += "белые: ";
                                str += pol[prevButton.Location.Y / 50, prevButton.Location.X / 50];
                                str += "-";
                                str += pol[pressedButton.Location.Y / 50, prevButton.Location.X / 50];
                                str += "; ";
                                
                                str1 += "белые: ";
                                str1 += pol[prevButton.Location.Y / 50, prevButton.Location.X / 50];
                                str1 += "-";
                                str1 += pol[pressedButton.Location.Y / 50, prevButton.Location.X / 50];
                                str1 += "; ";
                    }
                            else
                            {
                                str += "черные: ";
                                str += pol[prevButton.Location.Y / 50, prevButton.Location.X / 50];
                                str += "-";
                                str += pol[pressedButton.Location.Y / 50, prevButton.Location.X / 50];
                                str += "; ";

                                str1 += "черные: ";
                                str1 += pol[prevButton.Location.Y / 50, prevButton.Location.X / 50];
                                str1 += "-";
                                str1 += pol[pressedButton.Location.Y / 50, prevButton.Location.X / 50];
                                str1 += "; ";
                            }
                            map[prevButton.Location.Y / 50, prevButton.Location.X / 50] = 0; //исправил тут
                            pressedButton.BackgroundImage = prevButton.BackgroundImage;
                            prevButton.BackgroundImage = null;
                            isMoving = false;
                            CloseSteps();
                            ActivateAllButtons();
                            SwitchPlayer();
                    }
                }
                prevButton = pressedButton;
            
        }

        public void ShowSteps(int IcurrFigure, int JcurrFigure, int currFigure)
        {
            int dir = currPlayer == 1 ? 1 : -1;
            switch (currFigure % 10)
            {
                case 6:
                    if (InsideBorder(IcurrFigure + 1 * dir, JcurrFigure))
                    {
                        if (map[IcurrFigure + 1 * dir, JcurrFigure] == 0)
                        {
                            butts[IcurrFigure + 1 * dir, JcurrFigure].BackColor = Color.Yellow;
                            butts[IcurrFigure + 1 * dir, JcurrFigure].Enabled = true;
                            if ((InsideBorder(IcurrFigure + 2 * dir, JcurrFigure) && IcurrFigure == 6) || (InsideBorder(IcurrFigure + 2 * dir, JcurrFigure) && IcurrFigure == 1))
                            {

                                if (map[IcurrFigure + 2 * dir, JcurrFigure] == 0)
                                {
                                    butts[IcurrFigure + 2 * dir, JcurrFigure].BackColor = Color.Yellow;
                                    butts[IcurrFigure + 2 * dir, JcurrFigure].Enabled = true;
                                }
                            }
                        }

                    }
                    if (InsideBorder(IcurrFigure + 1 * dir, JcurrFigure + 1))
                    {
                        if (map[IcurrFigure + 1 * dir, JcurrFigure + 1] != 0 && map[IcurrFigure + 1 * dir, JcurrFigure + 1] / 10 != currPlayer)
                        {
                            butts[IcurrFigure + 1 * dir, JcurrFigure + 1].BackColor = Color.Yellow;
                            butts[IcurrFigure + 1 * dir, JcurrFigure + 1].Enabled = true;
                        }
                    }
                    if (InsideBorder(IcurrFigure + 1 * dir, JcurrFigure - 1))
                    {
                        if (map[IcurrFigure + 1 * dir, JcurrFigure - 1] != 0 && map[IcurrFigure + 1 * dir, JcurrFigure - 1] / 10 != currPlayer)
                        {
                            butts[IcurrFigure + 1 * dir, JcurrFigure - 1].BackColor = Color.Yellow;
                            butts[IcurrFigure + 1 * dir, JcurrFigure - 1].Enabled = true;
                        }
                    }
                    break;
                case 5:
                    ShowVerticalHorizontal(IcurrFigure, JcurrFigure);
                    break;
                case 3:
                    ShowDiagonal(IcurrFigure, JcurrFigure);
                    break;
                case 2:
                    ShowVerticalHorizontal(IcurrFigure, JcurrFigure);
                    ShowDiagonal(IcurrFigure, JcurrFigure);
                    break;
                case 1:
                    ShowVerticalHorizontal(IcurrFigure, JcurrFigure, true);
                    ShowDiagonal(IcurrFigure, JcurrFigure, true);
                    break;
                case 4:
                    ShowHorseSteps(IcurrFigure, JcurrFigure);
                    break;
            }
        }

        public void ShowHorseSteps(int IcurrFigure, int JcurrFigure)
        {
            if (InsideBorder(IcurrFigure - 2, JcurrFigure + 1))
            {
                DeterminePath(IcurrFigure - 2, JcurrFigure + 1);
            }
            if (InsideBorder(IcurrFigure - 2, JcurrFigure - 1))
            {
                DeterminePath(IcurrFigure - 2, JcurrFigure - 1);
            }
            if (InsideBorder(IcurrFigure + 2, JcurrFigure + 1))
            {
                DeterminePath(IcurrFigure + 2, JcurrFigure + 1);
            }
            if (InsideBorder(IcurrFigure + 2, JcurrFigure - 1))
            {
                DeterminePath(IcurrFigure + 2, JcurrFigure - 1);
            }
            if (InsideBorder(IcurrFigure - 1, JcurrFigure + 2))
            {
                DeterminePath(IcurrFigure - 1, JcurrFigure + 2);
            }
            if (InsideBorder(IcurrFigure + 1, JcurrFigure + 2))
            {
                DeterminePath(IcurrFigure + 1, JcurrFigure + 2);
            }
            if (InsideBorder(IcurrFigure - 1, JcurrFigure - 2))
            {
                DeterminePath(IcurrFigure - 1, JcurrFigure - 2);
            }
            if (InsideBorder(IcurrFigure + 1, JcurrFigure - 2))
            {
                DeterminePath(IcurrFigure + 1, JcurrFigure - 2);
            }
        }

        public void DeactivateAllButtons()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].Enabled = false;
                }
            }
        }

        public void ActivateAllButtons()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].Enabled = true;
                }
            }
        }

        public void ShowDiagonal(int IcurrFigure, int JcurrFigure, bool isOneStep = false)
        {
            int j = JcurrFigure + 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
        }

        public void ShowVerticalHorizontal(int IcurrFigure, int JcurrFigure, bool isOneStep = false)
        {
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (InsideBorder(i, JcurrFigure))
                {
                    if (!DeterminePath(i, JcurrFigure))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (InsideBorder(i, JcurrFigure))
                {
                    if (!DeterminePath(i, JcurrFigure))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int j = JcurrFigure + 1; j < 8; j++)
            {
                if (InsideBorder(IcurrFigure, j))
                {
                    if (!DeterminePath(IcurrFigure, j))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int j = JcurrFigure - 1; j >= 0; j--)
            {
                if (InsideBorder(IcurrFigure, j))
                {
                    if (!DeterminePath(IcurrFigure, j))
                        break;
                }
                if (isOneStep)
                    break;
            }
        }

        public bool DeterminePath(int IcurrFigure, int j)
        {
            if (map[IcurrFigure, j] == 0)
            {
                butts[IcurrFigure, j].BackColor = Color.Yellow;
                butts[IcurrFigure, j].Enabled = true;
            }
            else
            {
                if (map[IcurrFigure, j] / 10 != currPlayer)
                {
                    butts[IcurrFigure, j].BackColor = Color.Yellow;
                    butts[IcurrFigure, j].Enabled = true;

                }
                return false;
            }
            return true;
        }

        public bool InsideBorder(int ti, int tj)
        {
            if (ti >= 8 || tj >= 8 || ti < 0 || tj < 0)
                return false;
            return true;
        }

        //public void DeleteFigure(int ti, int tj)
        //{

        //}
        public void CloseSteps()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].BackColor = Color.LemonChiffon;
                }
            }
        }

        public void SwitchPlayer()
        {
            if (currPlayer == 1)
                currPlayer = 2;
            else currPlayer = 1;
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    this.Controls.Clear();
        //    Init();

        //}
            int h = 5;
            int m = 0;
            int s = 0;

            int h1 = 5;
            int m1 = 0;
            int s1 = 0;
            private void timer1_Tick(object sender, EventArgs e)
            {
                if (m == 0 && s == 0)
                {
                    h -= 1;
                    m = 59;
                    s = 60;
                }
                if (!(m == 0 && s == 0))
                {
                    s--;
                    if (s == 0)
                    {
                        m--;
                        s = 59;
                    }
                }
                if (m < 10)
                {
                    if (s < 10)
                    {
                        label1.Text = "0" + h.ToString() + ":0" + m.ToString() + ":0" + s.ToString();
                    }
                    else
                    {
                        label1.Text = "0" + h.ToString() + ":0" + m.ToString() + ":" + s.ToString();
                    }
                }
                else
                {
                    if (s < 10)
                    {
                        label1.Text = "0" + h.ToString() + ":" + m.ToString() + ":0" + s.ToString();
                    }
                    else
                    {
                        label1.Text = "0" + h.ToString() + ":" + m.ToString() + ":" + s.ToString();
                    }
                }

                if (h == 0 && m == 0 && s == 0)
                {
                    for (int k = 0; k < 1; k++)
                    {
                        DialogResult dialogResult = MessageBox.Show("Время вышло, вы проиграли", "Результат", MessageBoxButtons.OK);
                        break;
                    }
                }
        }

            private void label2_Click(object sender, EventArgs e)
            {

            }

            private void timer2_Tick(object sender, EventArgs e)
            {
                if (m1 == 0 && s1 == 0)
                {
                    h1 -= 1;
                    m1 = 59;
                    s1 = 60;
                }
                if (!(m1 == 0 && s1 == 0))
                {
                    s1--;
                    if (s1 == 0)
                    {
                        m1--;
                        s1 = 59;
                    }
                }
                if (m1 < 10)
                {
                    if (s1 < 10)
                    {
                        label2.Text = "0" + h1.ToString() + ":0" + m1.ToString() + ":0" + s1.ToString();
                    }
                    else
                    {
                        label2.Text = "0" + h1.ToString() + ":0" + m1.ToString() + ":" + s1.ToString();
                    }
                }
                else
                {
                    if (s1 < 10)
                    {
                        label2.Text = "0" + h1.ToString() + ":" + m1.ToString() + ":0" + s1.ToString();
                    }
                    else
                    {
                        label2.Text = "0" + h1.ToString() + ":" + m1.ToString() + ":" + s1.ToString();
                    }
                }
                if (h1 == 0 && m1 == 0 && s1 == 0)
                {
                    for (int k = 0; k < 1; k++)
                    {
                        DialogResult dialogResult = MessageBox.Show("Время вышло, вы проиграли", "Результат", MessageBoxButtons.OK);
                        break;
                    }
            }
        }

            private void label1_Click(object sender, EventArgs e)
            {

            }

            private void button3_Click(object sender, EventArgs e)
            {
                timer1.Enabled = false;
                timer2.Enabled = true;

            }

            private void button2_Click(object sender, EventArgs e)
            {
                timer1.Enabled = true;
                timer2.Enabled = false;
            }

            private void button4_Click(object sender, EventArgs e)
            {
                timer1.Enabled = false;
                timer2.Enabled = false;
            }

            private void button1_Click(object sender, EventArgs e)
            {
                string str_ref = "Ходы данного матча: ";
                MessageBox.Show(str_ref + str);
            }

            private void button5_Click(object sender, EventArgs e)
            {
                MessageBox.Show(str1);
            }

            private void button6_Click(object sender, EventArgs e)
            {
                Form2 form2 = new Form2();
                form2.Show();
            }

            private void button7_Click(object sender, EventArgs e)
            {
                this.Controls.Clear();
                Init();
                Button but = new Button();
            //button2.Controls.Add(this);
                but = button6;
                but.Location = button6.Location;
                but.Width = button6.Width;
                but.Height = button6.Height;
                but.Text = button6.Text;
                this.Controls.Add(but);

                but.Enabled = true;
                str1 = "";
                str = "";
                Button but2 = new Button();
                but2 = button2;
                but2.Location = button2.Location;
                but2.Width = button2.Width;
                but2.Height = button2.Height;
                but2.Text = button2.Text;
                but2.Enabled = true;
                this.Controls.Add(but2);

                Button but3 = new Button();
                but3 = button3;
                but3.Location = button3.Location;
                but3.Width = button3.Width;
                but3.Height = button3.Height;
                but3.Text = button3.Text;
                but3.Enabled = true;
                this.Controls.Add(but3);
 

                Button but4 = new Button();
                but4 = button4;
                but4.Location = button4.Location;
                but4.Width = button4.Width;
                but4.Height = button4.Height;
                but4.Text = button4.Text;
                this.Controls.Add(but4);
                but4.Enabled = true;

                Button but5 = new Button();
                but5 = button5;
                but5.Location = button5.Location;
                but5.Width = button5.Width;
                but5.Height = button5.Height;
                but5.Text = button5.Text;
                this.Controls.Add(but5);
                but5.Enabled = true;

                Button but1 = new Button();
                but1 = button1;
                but1.Location = button1.Location;
                but1.Width = button1.Width;
                but1.Height = button1.Height;
                but1.Text = button1.Text;
                this.Controls.Add(but1);
                but1.Enabled = true;

                Button but7 = new Button();
                but7 = button7;
                but7.Location = button7.Location;
                but7.Width = button7.Width;
                but7.Height = button7.Height;
                but7.Text = button7.Text;
                this.Controls.Add(but7);
                but7.Enabled = true;


                System.Windows.Forms.Label lab = new System.Windows.Forms.Label();
                lab = label1;
                lab.Location = label1.Location;
                lab.Width = label1.Width;
                lab.Height = label1.Height;
                lab.Text = "05:00:00";
                lab.Enabled = label1.Enabled;
                this.Controls.Add(lab);

                System.Windows.Forms.Label lab2 = new System.Windows.Forms.Label();
                lab2 = label2;
                lab2.Location = label2.Location;
                lab2.Width = label2.Width;
                lab2.Height = label2.Height;
                lab2.Text = "05:00:00";
                lab2.Enabled = label2.Enabled;
                this.Controls.Add(lab2);

                System.Windows.Forms.Label lab3 = new System.Windows.Forms.Label();
                lab3 = label3;
                lab3.Location = label3.Location;
                lab3.Width = label3.Width;
                lab3.Height = label3.Height;
                lab3.Text = label3.Text;
                lab3.Enabled = label3.Enabled;
                this.Controls.Add(lab3);

                System.Windows.Forms.Label lab4 = new System.Windows.Forms.Label();
                lab4 = LA;
                lab4.Location = LA.Location;
                lab4.Width = LA.Width;
                lab4.Height = LA.Height;
                lab4.Text = LA.Text;
                lab4.Enabled = LA.Enabled;
                this.Controls.Add(lab4);

                System.Windows.Forms.Label lab5 = new System.Windows.Forms.Label();
                lab5 = label5;
                lab5.Location = label5.Location;
                lab5.Width = label5.Width;
                lab5.Height = label5.Height;
                lab5.Text = label5.Text;
                lab5.Enabled = label5.Enabled;
                this.Controls.Add(lab5);

                System.Windows.Forms.Label lab44 = new System.Windows.Forms.Label();
                lab44 = label4;
                lab44.Location = label4.Location;
                lab44.Width = label4.Width;
                lab44.Height = label4.Height;
                lab44.Text = label4.Text;
                lab44.Enabled = label4.Enabled;
                this.Controls.Add(lab44);

                System.Windows.Forms.Label lab6 = new System.Windows.Forms.Label();
                lab6 = label6;
                lab6.Location = label6.Location;
                lab6.Width = label6.Width;
                lab6.Height = label6.Height;
                lab6.Text = label6.Text;
                lab6.Enabled = label6.Enabled;
                this.Controls.Add(lab6);

                System.Windows.Forms.Label lab7 = new System.Windows.Forms.Label();
                lab7 = label7;
                lab7.Location = label7.Location;
                lab7.Width = label7.Width;
                lab7.Height = label7.Height;
                lab7.Text = label7.Text;
                lab7.Enabled = label7.Enabled;
                this.Controls.Add(lab7);


                System.Windows.Forms.Label lab8 = new System.Windows.Forms.Label();
                lab8 = label8;
                lab8.Location = label8.Location;
                lab8.Width = label8.Width;
                lab8.Height = label8.Height;
                lab8.Text = label8.Text;
                lab8.Enabled = label8.Enabled;
                this.Controls.Add(lab8);

                System.Windows.Forms.Label lab9 = new System.Windows.Forms.Label();
                lab9 = label9;
                lab9.Location = label9.Location;
                lab9.Width = label9.Width;
                lab9.Height = label9.Height;
                lab9.Text = label9.Text;
                lab9.Enabled = label9.Enabled;
                this.Controls.Add(lab9);

                System.Windows.Forms.Label lab10 = new System.Windows.Forms.Label();
                lab10 = label10;
                lab10.Location = label10.Location;
                lab10.Width = label10.Width;
                lab10.Height = label10.Height;
                lab10.Text = label10.Text;
                lab10.Enabled = label10.Enabled;
                this.Controls.Add(lab10);

                System.Windows.Forms.Label lab11 = new System.Windows.Forms.Label();
                lab11 = label11;
                lab11.Location = label11.Location;
                lab11.Width = label11.Width;
                lab11.Height = label11.Height;
                lab11.Text = label11.Text;
                lab11.Enabled = label11.Enabled;
                this.Controls.Add(lab11);

                System.Windows.Forms.Label lab12 = new System.Windows.Forms.Label();
                lab12 = label12;
                lab12.Location = label12.Location;
                lab12.Width = label12.Width;
                lab12.Height = label12.Height;
                lab12.Text = label12.Text;
                lab12.Enabled = label12.Enabled;
                this.Controls.Add(lab12);

                System.Windows.Forms.Label lab13 = new System.Windows.Forms.Label();
                lab13 = label13;
                lab13.Location = label13.Location;
                lab13.Width = label13.Width;
                lab13.Height = label13.Height;
                lab13.Text = label13.Text;
                lab13.Enabled = label13.Enabled;
                this.Controls.Add(lab13);

                System.Windows.Forms.Label lab14 = new System.Windows.Forms.Label();
                lab14 = label14;
                lab14.Location = label14.Location;
                lab14.Width = label14.Width;
                lab14.Height = label14.Height;
                lab14.Text = label14.Text;
                lab14.Enabled = label14.Enabled;
                this.Controls.Add(lab14);

                System.Windows.Forms.Label lab15 = new System.Windows.Forms.Label();
                lab15 = label15;
                lab15.Location = label15.Location;
                lab15.Width = label15.Width;
                lab15.Height = label15.Height;
                lab15.Text = label15.Text;
                lab15.Enabled = label15.Enabled;
                this.Controls.Add(lab15);

                System.Windows.Forms.Label lab16 = new System.Windows.Forms.Label();
                lab16 = label16;
                lab16.Location = label16.Location;
                lab16.Width = label16.Width;
                lab16.Height = label16.Height;
                lab16.Text = label16.Text;
                lab16.Enabled = label16.Enabled;
                this.Controls.Add(lab16);

                System.Windows.Forms.Label lab17 = new System.Windows.Forms.Label();
                lab17 = label17;
                lab17.Location = label17.Location;
                lab17.Width = label17.Width;
                lab17.Height = label17.Height;
                lab17.Text = label17.Text;
                lab17.Enabled = label17.Enabled;
                this.Controls.Add(lab17);

                Timer time1 = new Timer();
                time1 = timer1;
                time1.Stop();

                Timer time2 = new Timer();
                time2 = timer2;
                time2.Stop();
        }
    }
}

