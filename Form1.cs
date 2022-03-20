using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution;
        private GameEngine gameEngine;


        public Form1()
        {
            InitializeComponent();
        }



        private void bStart_Click(object sender, EventArgs e)
        {
                StartGame();
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void bStartTime_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
                return;

            nudResolution.Enabled = false;
            nudDensity.Enabled = false;
            resolution = (int)nudResolution.Value;

            timer1.Start();
            timer2.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawNextgeneration();
        }


        // Turn on numeric up/down buttons and stop timer1
        private void StopGame()
        {
            if (!timer1.Enabled)
                return;
            
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;

            timer1.Stop();
            Text = "Game of life";
        }


        /* Create new rectangle in current mous position if left mouse button on 
         * or delete rectangle in current mous position if right mouse button 
         * (right - delete, left - create)
        */
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled)
                return;

            //Left mouse button on
            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;

                gameEngine.AddCell(x, y);
            }

            //Right mouse button on
            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;

                gameEngine.RemoveCell(x, y);
            }
        }



        // Skript to start game
        private void StartGame()
        {
            if (timer1.Enabled)
                return;

            // Disable numeric up/down buttons 
            nudResolution.Enabled = false;
            nudDensity.Enabled = false;
            resolution = (int)nudResolution.Value;

            // Initialize constructor component
            gameEngine = new GameEngine
            (
                rows: pictureBox1.Height / resolution,
                cols: pictureBox1.Width / resolution,
                density: (int)nudDensity.Minimum + (int)nudDensity.Maximum - (int)nudDensity.Value
            );

            Text = $"Generation {gameEngine.CurrentGeneration}";

            // Initialize size imege, grafics class and start to do generation
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }



        // Draw next generation with rules
        private void DrawNextgeneration()
        {
            graphics.Clear(Color.Black);

            var field = gameEngine.GetCurrentGeneration();

            // Draw all rectangle in the field
            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y])
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution, resolution);
                }
            }

            // Do rules of the game
            pictureBox1.Refresh();
            Text = $"Generation {gameEngine.CurrentGeneration}";
            gameEngine.Nextgeneration();
        }




        //Delete all rectangle in the field
        private void bClear_Click(object sender, EventArgs e)
        {
            gameEngine.Clear();

            DrawNextgeneration(); 
        }

        //private void bEditemode_Click(object sender, EventArgs e)
        //{
        //    if (timer2.Enabled)
        //        return;

        //    nudResolution.Enabled = false;
        //    nudDensity.Enabled = false;
        //    resolution = (int)nudResolution.Value;

        //    timer1.Stop();
        //    timer2.Start();
        //}

        private void timer2_Tick(object sender, EventArgs e)
        {
            Editemode();
        }


        private void Editemode()
        {
            graphics.Clear(Color.Black);

            var field = gameEngine.GetCurrentGeneration();

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y])
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution, resolution);
                }
            }

            pictureBox1.Refresh();
        }
    }
}
