using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace DarkFalcon
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static FrmMain frmMain = new FrmMain();
        [STAThread]
        static void Main(string[] args)
        {
            splash sp = new splash();
            sp.StartPosition = FormStartPosition.Manual;
            sp.Show();
            sp.Location = new Point(1000, 800);
            
            frmMain.IsMdiContainer = true;
            frmMain.Show();
            frmMain.WindowState = FormWindowState.Maximized;
            frmMain.Visible = false;

            Frm3D frmTabs = new Frm3D();
            frmTabs.MdiParent = frmMain;
            //eh o assistente
            frmTabs.Show();
            //frmTabs.Top += 188;
            
            frmTabs.Size = new Size(1016, 674);

            PcView game = new PcView(frmTabs.getDrawSurface(), frmTabs);
            frmTabs.getPcView(game);
            game.Run();

            

        }
    }
}

