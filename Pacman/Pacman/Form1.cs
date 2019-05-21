using Pacman.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pacman
{
    public partial class Form1 : Form
    {
        Timer timer1;
        Pacman pacman;
        static readonly int TIMER_INTERVAL = 250;
        static readonly int WORLD_WIDTH = 15;
        static readonly int WORLD_HEIGHT = 10;
        Image Hrana;
        Image Zid;
        bool[][] foodWorld;
        bool[][] canMove;
        int eaten = 0;
        public Form1()
        {
            InitializeComponent();
            Hrana = Resources.fiji;
            Zid = Resources.novwall;
            DoubleBuffered = true;
            NewGame();
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            for (int i = 0; i < foodWorld.Length; i++)
            {
                for (int j = 0; j < foodWorld[i].Length; j++)
                {
                    if (foodWorld[i][j])
                    {
                        g.DrawImageUnscaled(Hrana, j * Pacman.Radius * 2 + (Pacman.Radius * 2 - Hrana.Height) / 2, i * Pacman.Radius * 2 + (Pacman.Radius * 2 - Hrana.Width) / 2);
                    }
                    if(!canMove[i][j])
                    {
                        g.DrawImageUnscaled(Zid, j * Pacman.Radius * 2 + (Pacman.Radius * 2 - Hrana.Height) / 2, i * Pacman.Radius * 2 + (Pacman.Radius * 2 - Hrana.Width) / 2);
                    }
                }
            }
            pacman.Draw(g);
        }

        //Start the game
        private void NewGame()
        {
            pacman = new Pacman();
            this.Width = Pacman.Radius * 2 * (WORLD_WIDTH + 1);
            this.Height = Pacman.Radius * 2 * (WORLD_HEIGHT + 1)+50;

            progressBar1.Maximum = 126;
            progressBar1.Value = 0;
            //add fields in food
            foodWorld = new bool[WORLD_HEIGHT][];
            canMove = new bool[WORLD_HEIGHT][];
            for (int i = 0; i < WORLD_HEIGHT; i++)
            {
                canMove[i] = new bool[WORLD_WIDTH];
                for(int j = 0; j < WORLD_WIDTH; j++)
                {
                    canMove[i][j] = true;
                }
            }
            canMove[1][1] = false; canMove[2][1] = false; canMove[3][1] = false; //barikada 1
            canMove[7][1] = false; canMove[8][1] = false; canMove[9][1] = false; //barikada 2
            canMove[4][3] = false; canMove[5][3] = false; canMove[6][3] = false; //barikada 3
            canMove[3][6] = false; canMove[4][6] = false; canMove[5][6] = false; //barikada 4
            canMove[0][8] = false; canMove[1][8] = false; canMove[2][8] = false; //barikada 5
            canMove[7][10] = false; canMove[8][10] = false; canMove[9][10] = false; //barikada 6
            canMove[2][11] = false; canMove[3][11] = false; canMove[4][11] = false; //barikada 7
            canMove[5][13] = false; canMove[6][13] = false; canMove[7][13] = false; //barikada 8
            for (int i = 0; i < WORLD_HEIGHT; i++)
            {
                foodWorld[i] = new bool[WORLD_WIDTH];
                for (int j = 0; j < WORLD_WIDTH; j++)
                {
                    if (canMove[i][j])
                    {

                        foodWorld[i][j] = true;
                    }
                }
            }
            timer1 = new Timer();
            timer1.Interval = TIMER_INTERVAL;
            timer1.Tick += new EventHandler(Timer_Tick);
            timer1.Start();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            
            int calculatedXPos = Math.Abs(pacman.Xpos / 40);
            int calculatedYpos = Math.Abs(pacman.Ypos / 38);

            
            if (calculatedXPos == WORLD_WIDTH)
            {
                calculatedXPos--;
            }
            if (calculatedYpos == WORLD_HEIGHT)
            {
                calculatedYpos--;
            }

            //proveri hrana
            if (foodWorld[calculatedYpos][calculatedXPos])
            {
                foodWorld[calculatedYpos][calculatedXPos] = false;
                eaten++;
                label1.Text = eaten.ToString() + " %";
                progressBar1.Value = eaten;
            }
            if (canMove[calculatedYpos][calculatedXPos])
            {
                pacman.Move();
            }
            else
            {
                if (pacman.Direction == Pacman.DIRECTION.left)
                    pacman.Direction = Pacman.DIRECTION.right;
                else if (pacman.Direction == Pacman.DIRECTION.right)
                    pacman.Direction = Pacman.DIRECTION.left;
                else if (pacman.Direction == Pacman.DIRECTION.down)
                    pacman.Direction = Pacman.DIRECTION.up;
                else if (pacman.Direction == Pacman.DIRECTION.up)
                    pacman.Direction = Pacman.DIRECTION.down;
                pacman.Move();
            }
                Invalidate();
            
        }

        //Catch key presses and change the direction acordingly
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (pacman.Direction != Pacman.DIRECTION.up)
                    {
                        pacman.ChangeDirection(Pacman.DIRECTION.up);
                    }
                    break;
                case Keys.Down:
                    if (pacman.Direction != Pacman.DIRECTION.down)
                    {
                        pacman.ChangeDirection(Pacman.DIRECTION.down);
                    }
                    break;
                case Keys.Left:
                    if (pacman.Direction != Pacman.DIRECTION.left)
                    {
                        pacman.ChangeDirection(Pacman.DIRECTION.left);
                    }
                    break;
                case Keys.Right:
                    if (pacman.Direction != Pacman.DIRECTION.right)
                    {
                        pacman.ChangeDirection(Pacman.DIRECTION.right);
                    }
                    break;
                default:
                    break;
            }

            Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }



}
