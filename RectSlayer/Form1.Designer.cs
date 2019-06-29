namespace RectSlayer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gameLoopTimer = new System.Windows.Forms.Timer(this.components);
            this.shootTimer = new System.Windows.Forms.Timer(this.components);
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblBalls = new System.Windows.Forms.Label();
            this.gameStuckTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // gameLoopTimer
            // 
            this.gameLoopTimer.Interval = 10;
            this.gameLoopTimer.Tick += new System.EventHandler(this.GameLoopTimer_Tick);
            // 
            // shootTimer
            // 
            this.shootTimer.Interval = 130;
            this.shootTimer.Tick += new System.EventHandler(this.ShootTimer_Tick);
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(17, 16);
            this.lblLevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(46, 17);
            this.lblLevel.TabIndex = 0;
            this.lblLevel.Text = "label1";
            this.lblLevel.Visible = false;
            // 
            // lblBalls
            // 
            this.lblBalls.AutoSize = true;
            this.lblBalls.Location = new System.Drawing.Point(16, 662);
            this.lblBalls.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBalls.Name = "lblBalls";
            this.lblBalls.Size = new System.Drawing.Size(38, 17);
            this.lblBalls.TabIndex = 1;
            this.lblBalls.Text = "Balls";
            this.lblBalls.Visible = false;
            // 
            // gameStuckTimer
            // 
            this.gameStuckTimer.Interval = 1500;
            this.gameStuckTimer.Tick += new System.EventHandler(this.GameStuckTimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 689);
            this.Controls.Add(this.lblBalls);
            this.Controls.Add(this.lblLevel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "RectSlayer";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer gameLoopTimer;
        private System.Windows.Forms.Timer shootTimer;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label lblBalls;
        private System.Windows.Forms.Timer gameStuckTimer;
    }
}

