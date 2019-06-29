using System;
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
    public partial class MainMenu : Form
    {
        public int HighScore { get; set; }
        public MainMenu()
        {
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            HighScore = 1;
            InitializeComponent();
        }

        private void BtnNewGame_Click(object sender, EventArgs e)
        {
            this.Hide();
            var gameForm = new Form1();
            gameForm.Closed += (s, args) => this.Close();
            gameForm.Show();
            gameForm.Manager.HighScore = HighScore;
        }

        private void BtnQuitGame_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
