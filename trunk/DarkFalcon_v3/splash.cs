using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace DarkFalcon_v3
{
    public partial class splash : Form
    {
        public splash()
        {
            InitializeComponent();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
     
        }

        private void splash_Load(object sender, EventArgs e)
        {
        }

        private void splash_Load_1(object sender, EventArgs e)
        {
            pictureBox1.Load("splash.png");
            timer1.Enabled = true;
        }

        private void splash_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.frmMain.TopMost = true;
            Program.frmMain.Visible = true;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close() ;
        }
    }
}
