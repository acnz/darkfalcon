using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DarkFalcon
{
    public partial class FrmAssistente : Form
    {
        public FrmAssistente()
        {
            InitializeComponent();
        }

        private void FrmAssistente_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void FrmAssistente_Load(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                button1.Enabled = false;
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
            
        }
        
    }
    
