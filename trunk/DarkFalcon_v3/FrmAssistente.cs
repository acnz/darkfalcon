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

            if (comboBox1.SelectedIndex == 0)
            {
                FileStream youtube = File.Create(Properties.Settings.Default.CRoot+"youtube.html");
                using (StreamWriter writer = new StreamWriter(youtube))
                {
                    writer.Write("<html>");
                    writer.Write("<head>");
                    writer.Write("</head>");
                    writer.Write("<body>");
                    writer.Write("<embed src=\"http://www.youtube.com/v/aGJmHyPVXPc?version=3&autoplay=1&start=62\" type=\"application/x-shockwave-flash\" wmode=\"transparent\" width=\"425\" height=\"350\"></embed>");
                    writer.Write("</body>");
                    writer.Write("</html>");
                }
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                FileStream youtube = File.Create(Properties.Settings.Default.CRoot + "youtube.html");
                using (StreamWriter writer = new StreamWriter(youtube))
                {
                    writer.Write("<html>");
                    writer.Write("<head>");
                    writer.Write("</head>");
                    writer.Write("<body>");
                    writer.Write("<embed src=\"http://www.youtube.com/v/MdZNU8N-teU?version=3&autoplay=1&start=62\" type=\"application/x-shockwave-flash\" wmode=\"transparent\" width=\"425\" height=\"350\"></embed>");
                    writer.Write("</body>");
                    writer.Write("</html>");
                }
            }
        





            FormWeb b = new FormWeb();
            b.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
            
        }
        
    }
    
