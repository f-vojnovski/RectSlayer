﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RectSlayer
{
    public partial class Form1 : Form
    {
        private GameManager Manager;

        private int leftX;
        private int topY;
        private int width;
        private int height;
        Point mouseLocation;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            SetupVariables();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
        }
        private void SetupVariables()
        {
            leftX = 10;
            topY = 100;
            width = this.Width - 40;
            height = this.Height - (int)(2.1 * topY);
            Manager = new GameManager(leftX, topY, width, height, shootTimer, gameStuckTimer);
            gameLoopTimer.Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            Pen pen = new Pen(Color.Yellow, 3);
            e.Graphics.DrawRectangle(pen, leftX, topY, width, height);
            pen.Dispose();
            Manager.Draw(e.Graphics);
            Manager.DrawIndicatorLine(e.Graphics, mouseLocation);
        }

        private void GameLoopTimer_Tick(object sender, EventArgs e)
        {
            Manager.HandleLogic();
            lblLevel.Text = "Level: " + Manager.Level.ToString();
            lblBalls.Text = "Shots: " + Manager.Player.BallsToShoot.ToString();
            Invalidate(true);

            if (Manager.isGameOver)
            {
                gameLoopTimer.Stop();
                DialogResult dialogResult = MessageBox.Show("Play again?", "Game Over", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    gameLoopTimer.Start();
                    Manager.StartGame();
                }
                else if (dialogResult == DialogResult.No)
                {
                    this.Hide();
                    var oForm = new MainMenu();
                    oForm.Closed += (s, args) => this.Close();
                    oForm.Show();
                }
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            mouseLocation = e.Location;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Manager.StartShooting(e.Location);
        }

        private void ShootTimer_Tick(object sender, EventArgs e)
        {
            Manager.ShootBall();
        }

        private void GameStuckTimer_Tick(object sender, EventArgs e)
        {
            Manager.CheckForStuckBalls();
        }
    }
}
