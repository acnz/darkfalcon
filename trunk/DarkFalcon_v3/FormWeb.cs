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
            webBrowser1.Navigate(Properties.Settings.Default.CRoot+"youtube.html");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
        }

        private void FormWeb_Load(object sender, EventArgs e)
        {

        }
    }
}
