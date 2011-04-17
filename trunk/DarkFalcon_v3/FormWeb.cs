using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DarkFalcon
{
    public partial class FormWeb : Form
    {
        public FormWeb()
        {
            InitializeComponent();
            webBrowser1.Navigate("c:\\youtube.html");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
        }
    }
}
