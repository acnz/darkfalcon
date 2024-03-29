﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using DarkFalcon.df;

namespace DarkFalcon
{
    public partial class FrmMain : Form
    {
        public bool CanClose = false;
        public bool wasMoving = false;
        
        Size s;
        DialogResult res;

        
        public FrmMain()
        {
            InitializeComponent();

        }

        private void FrmMain_Load(object sender, EventArgs e)
        {


            s = this.Size;

        }


        private void novoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //((FrmTabs)this.MdiChildren[2]).NovaAba();
        }

        public bool Salvar(string nome, dfCom[] pc)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            res = dlg.ShowDialog();
            //res = MessageBox.Show("Salvar?", "Demonstração ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            MessageBox.Show("A função Salvar ainda esta indisponivel!");

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


                if (!((Frm3D)this.MdiChildren[0]).closeAll())
                {
                    e.Cancel = true;
                }
                else
                {
                    
                    Application.Exit();
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
                ((Frm3D)this.MdiChildren[0]).FixPosition();
                ((Frm3D)this.MdiChildren[0]).pc.nmv = false;
                wasMoving = true;
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
            //frmI1.Size = new Size(frmI1.Size.Width, s.Height - 66);
            //frmI1.Left = s.Width - (frmI1.Size.Width + 20);
            //if (frmI1.Left < 745)
            //    frmI1.Left = 745;
        }

        private void testeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAdmDB frmA = new FrmAdmDB();
            frmA.Show();
        }

        private void FrmMain_Activated(object sender, EventArgs e)
        {
            try
            {
                ((Frm3D)this.MdiChildren[0]).pc.focus = true;
            }
            catch (Exception) { }
        }

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            int desk = Screen.PrimaryScreen.Bounds.Width;

            if (desk <= 1024)
            {
                if (this.WindowState != FormWindowState.Minimized)
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
            }
            //try
            //{
            //    FrmTabs frmTabs = (FrmTabs)this.MdiChildren[2];

            //    frmI1.Size = new Size(frmI1.Size.Width, s.Height - 66);
            //    frmI1.Left = s.Width - (frmI1.Size.Width + 20);
            //    if (frmI1.Left < 745)
            //        frmI1.Left = 745;
            //    if (frmI1.Size.Height < 614)
            //        frmI1.Size = new Size(frmI1.Size.Width, 614);

            //    frmTabs.Size = new Size(frmI1.Left, frmI1.Size.Height - 188);

            //    frmI2.Size = new Size(frmTabs.Size.Width, frmI2.Size.Height);

            //}
            //catch (Exception) { }
        }

        private void FrmMain_MouseCaptureChanged(object sender, EventArgs e)
        {
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
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

         
            
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {

        }

        private void FrmMain_Enter(object sender, EventArgs e)
        {
           // ((Frm3D)this.MdiChildren[2]).pc.focus = true;
        }

        private void FrmMain_MouseUp(object sender, MouseEventArgs e)
        {
            
          //  
        }

        private void FrmMain_RegionChanged(object sender, EventArgs e)
        {
            //((Frm3D)this.MdiChildren[0]).pc.active = true;

        }

        private void FrmMain_Deactivate(object sender, EventArgs e)
        {
            try
            {
                Console.Out.WriteLine("deactive");
                ((Frm3D)this.MdiChildren[0]).pc.focus = false;
            }
            catch (Exception) { }
        }

    }
}
