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
    public partial class FrmMain : Form
    {
        public bool CanClose = false;
        FrmInterface1 frmI1;
        FrmInterface2 frmI2;
        //FrmAssistente frmAss;
        Size s;
        DialogResult res;

        
        public FrmMain()
        {
            InitializeComponent();

        }

        private void FrmMain_Load(object sender, EventArgs e)
        {


            s = this.Size; 
            frmI1 = new FrmInterface1();
            frmI1.MdiParent = this;
            frmI1.StartPosition = FormStartPosition.Manual;
            frmI1.Size = new Size(frmI1.Size.Width, s.Height - 66);
            frmI1.Left = s.Width - (frmI1.Size.Width+20);
            frmI1.Show();

            frmI2 = new FrmInterface2();
            frmI2.MdiParent = this;
            frmI2.StartPosition = FormStartPosition.Manual;
            frmI2.Size = new Size(frmI1.Left, frmI2.Size.Height);
            frmI2.Show();

            frmI1.getInter2(this.frmI2);
            frmI2.getInter1(this.frmI1);

        }


        private void novoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((FrmTabs)this.MdiChildren[2]).NovaAba();
        }

        public bool Salvar(string nome, dfCom[] pc)
        {
            Thread newThread = new Thread(new ThreadStart(SalvarArquivo));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
            //res = MessageBox.Show("Salvar?", "Demonstração ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            Console.Out.WriteLine(res);

                if (res == DialogResult.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            CanClose = true;

                if (((FrmTabs)this.MdiChildren[2]).TabPcs.TabCount == 0)
            {
                Application.Exit();
            }
            else
            {

                if (!((FrmTabs)this.MdiChildren[2]).closeAll())
                {
                    e.Cancel = true;
                }
                else
                {
                    
                    Application.Exit();
                }
            }
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void FrmMain_VisibleChanged(object sender, EventArgs e)
        {
            this.TopMost = false;
        }

        private void FrmMain_Move(object sender, EventArgs e)
        {
            try
            {
                ((FrmTabs)this.MdiChildren[2]).FixPosition();
            }
            catch (Exception exc)
            {
                Console.Out.WriteLine(exc);
            }
        }

        private void FrmMain_ResizeBegin(object sender, EventArgs e)
        {
           // s = this.Size; 
        }

        private void FrmMain_ResizeEnd(object sender, EventArgs e)
        {
            s = this.Size;
            frmI1.Size = new Size(frmI1.Size.Width, s.Height - 66);
            frmI1.Left = s.Width - (frmI1.Size.Width + 20);
            if (frmI1.Left < 745)
                frmI1.Left = 745;
        }

        private void testeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void FrmMain_Activated(object sender, EventArgs e)
        {
        
        }

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                s = this.Size;
                FrmTabs frmTabs = (FrmTabs)this.MdiChildren[2];

                frmI1.Size = new Size(frmI1.Size.Width, s.Height - 66);
                frmI1.Left = s.Width - (frmI1.Size.Width + 20);
                if (frmI1.Left < 745)
                    frmI1.Left = 745;
                if (frmI1.Size.Height < 614)
                    frmI1.Size = new Size(frmI1.Size.Width, 614);

                frmTabs.Size = new Size(frmI1.Left, frmI1.Size.Height - 188);

                frmI2.Size = new Size(frmTabs.Size.Width, frmI2.Size.Height);

            }
            catch (Exception) { }
        }

        private void FrmMain_MouseCaptureChanged(object sender, EventArgs e)
        {
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {


            Thread newThread = new Thread(new ThreadStart(AbrirArquivo));
        newThread.SetApartmentState(ApartmentState.STA);
        newThread.Start();     
        
    }

        static void AbrirArquivo()
    {
        OpenFileDialog dlg = new OpenFileDialog();
        dlg.ShowDialog();
        MessageBox.Show(dlg.FileName);
    }

         

        private void salvarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Salvar("teste", new dfCom [4]);
        }

        void SalvarArquivo()
        {
            SaveFileDialog dlg = new SaveFileDialog();
           res=dlg.ShowDialog();
         
            
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {

        }

    }
}
