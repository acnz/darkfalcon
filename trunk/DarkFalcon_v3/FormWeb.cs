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
    public partial class FormWeb : Form
    {
        public FormWeb()
        {
            InitializeComponent();

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
        }

        private void FormWeb_Load(object sender, EventArgs e)
        {

        }
        public void PlayVideo(string t)
        {
            string vid = "";

            switch (t)
            {
                case "Motherboard":
                  vid = "http://www.youtube.com/v/PmCzgFItOWg?version=3&autoplay=1&start=0";
                    break;
                case "Processador":
                    vid = "http://www.youtube.com/v/MkhwDIpEHIQ?version=3&autoplay=1&start=15&stop=48";
                    break;
                case "Memoria":
                    vid = "http://www.youtube.com/v/-4OlRsjHFMQ?version=3&autoplay=1&start=15";
                    break;
                case "PlaVideo":
                    vid = "http://www.youtube.com/v/aGJmHyPVXPc?version=3&autoplay=1&start=62";
                    break;
            }

            FileStream youtube = File.Create(Properties.Settings.Default.CRoot + "youtube.html");
            using (StreamWriter writer = new StreamWriter(youtube))
            {
                writer.Write("<html>");
                writer.Write("<head>");
                writer.Write("</head>");
                writer.Write("<body>");
                writer.Write("<embed src=\""+vid+"\" type=\"application/x-shockwave-flash\" wmode=\"transparent\" width=\"425\" height=\"350\"></embed>");
                writer.Write("</body>");
                writer.Write("</html>");
            }
            webBrowser1.Navigate(Properties.Settings.Default.CRoot + "youtube.html");
        }
    }
}
