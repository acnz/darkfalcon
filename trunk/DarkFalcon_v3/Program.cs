using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace DarkFalcon_v3
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static FrmMain frmMain = new FrmMain();
        static void Main(string[] args)
        {
            splash sp = new splash();
            sp.Show();
            
            frmMain.IsMdiContainer = true;
            frmMain.Show();
            frmMain.Visible = false;

            FrmTabs frmTabs = new FrmTabs();
            frmTabs.MdiParent = frmMain;

            frmTabs.Show();
            frmTabs.Top += 188;
            
            frmTabs.Size = new Size(745, frmTabs.MdiParent.MdiChildren[0].Size.Height - 188);

            PcView game = new PcView(frmTabs.getDrawSurface(), frmTabs);
            frmTabs.getPcView(game);
            game.Run();

            

        }
    }
}

