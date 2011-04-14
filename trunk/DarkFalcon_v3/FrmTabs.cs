using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DarkFalcon.df;

namespace DarkFalcon
{
    public partial class FrmTabs : Form
    {
        public PcView pc;
        String TabName = "";
        TabPage tab = new TabPage();
        int prevIndex = 0;
        public List<dfCom[]> ListaTabs = new List<dfCom[]>();
        public List<bool> ListaSalvo = new List<bool>();
        
        public FrmTabs()
        {
            InitializeComponent();

        }


        public void getPcView(PcView pV)
        {
            pc = pV;
        }

        public IntPtr getDrawSurface()
        {
            return PicPcView.Handle;
        }

        private void FrmTabs_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void FrmTabs_Load(object sender, EventArgs e)
        {
            dfCom[] novo = new dfCom[4];
            ListaTabs.Add(novo);
            ListaSalvo.Add(false);
        }

        public void NovaAba()
        {
            TabPage tab = new TabPage();

            //tab.Controls.Add(this.PicPcView);
            if (TabName == "")
                if (TabPcs.TabPages.Count + 1 < 10)
                    TabName = "Pc0" + (TabPcs.TabPages.Count + 1);
                else
                    TabName = "Pc" + (TabPcs.TabPages.Count + 1);

            tab.Text = TabName;
            TabPcs.TabPages.Add(tab);
            TabPcs.SelectedIndex = TabPcs.TabPages.Count - 1;

            TabName = "";

            dfCom[] novo = new dfCom[4];
            ListaTabs.Add(novo);
            ListaSalvo.Add(false);
            pc.noTabs = false;
        }

        private void TabPcs_TabIndexChanged(object sender, EventArgs e)
        {
            if (TabPcs.TabCount == 0)
            {
                pc.noTabs = true;
            }
            else
            {
                pc.noTabs = false;
            }

            //TabPcs.TabPages[TabPcs.SelectedIndex].Controls.Add(this.PicPcView);
            prevIndex = TabPcs.SelectedIndex;
            //Console.WriteLine(prevIndex);
        }

        private void PicPcView_Click(object sender, EventArgs e)
        {

        }

        private void FrmTabs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void FrmTabs_Move(object sender, EventArgs e)
        {
            pc.FixPosition();
        }

        public bool closeAll()
        {
            bool done = true;
            for (int i = TabPcs.TabCount - 1; i >= 0; --i)
            {
                string nome = TabPcs.TabPages[i].Text;

                {
                    if (ListaSalvo[i] == true)
                    {
                        TabPcs.TabPages.RemoveAt(i);
                        ListaTabs.RemoveAt(i);
                        ListaSalvo.RemoveAt(i);
                    }
                    else
                    {

                        DialogResult res;
                        res = MessageBox.Show("Deseja Salvar " + nome + " ?", "Fechar computador", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (res == DialogResult.No)
                        {
                            TabPcs.TabPages.RemoveAt(i);
                            ListaTabs.RemoveAt(i);
                            ListaSalvo.RemoveAt(i);
                        }
                        else if (res == DialogResult.Yes)
                        {
                            if (((FrmMain)this.MdiParent).Salvar(nome, ListaTabs[i]) == true)
                            {
                                TabPcs.TabPages.RemoveAt(i);
                                ListaTabs.RemoveAt(i);
                                ListaSalvo.RemoveAt(i);
                            }
                            else
                            {
                                done = false;
                                break;
                            }
                        }
                        else if (res == DialogResult.Cancel)
                        {
                            done = false;
                            break;
                        }
                    }
                }
            }
            ((FrmMain)this.MdiParent).CanClose = false;
            return done;
              

        }

        private void FrmTabs_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.MdiFormClosing)
            {
                if (this.TabPcs.TabCount != 0)
                {
                    int i = TabPcs.SelectedIndex;
                    string nome = TabPcs.TabPages[i].Text;
                    if (TabPcs.TabCount > 0)
                    {
                        if (ListaSalvo[i] == true)
                        {
                            TabPcs.TabPages.RemoveAt(i);
                            ListaTabs.RemoveAt(i);
                            ListaSalvo.RemoveAt(i);
                        }
                        else
                        {

                            DialogResult res;
                            res = MessageBox.Show("Deseja Salvar " + nome + " ?", "Fechar computador", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                            if (res == DialogResult.No)
                            {
                                TabPcs.TabPages.RemoveAt(i);
                                ListaTabs.RemoveAt(i);
                                ListaSalvo.RemoveAt(i);
                            }
                            else if (res == DialogResult.Yes)
                            {
                                if (((FrmMain)this.MdiParent).Salvar(nome, ListaTabs[i]) == true)
                                {
                                    TabPcs.TabPages.RemoveAt(i);
                                    ListaTabs.RemoveAt(i);
                                    ListaSalvo.RemoveAt(i);
                                }
                            }
                        }
                    }
                    else
                        pc.noTabs = true;
                }

                e.Cancel = true;

            }
        }


        public void FrmTabs_Move_1(object sender, EventArgs e)
        {
            FixPosition();
        }

        public void FixPosition()
        {
            try
            {
                pc.FixPosition();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e);
            }
        }

        private void FrmTabs_SizeChanged(object sender, EventArgs e)
        {
            try{
            pc.resize(this);
            FixPosition();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
            }
        }

    }
}
