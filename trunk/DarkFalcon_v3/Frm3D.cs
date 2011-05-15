using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DarkFalcon.df;
using System.Data.OleDb;

namespace DarkFalcon
{
    public partial class Frm3D : Form
    {
        public PcView pc;
        String TabName = "";
        TabPage tab = new TabPage();
        int prevIndex = 0;
        public List<dfCom[]> ListaTabs = new List<dfCom[]>();
        public List<bool> ListaSalvo = new List<bool>();
        DataSet ds1;
        OleDbDataAdapter da;
        
        public Frm3D()
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
            da = new OleDbDataAdapter();
            ds1 = new DataSet();
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

        private void FrmTabs_Activated(object sender, EventArgs e)
        {
            pc.focus = true;
        }

        private void FrmTabs_Deactivate(object sender, EventArgs e)
        {
            pc.focus = false;
        }

        private void FrmTabs_Enter(object sender, EventArgs e)
        {
            pc.focus = true;
        }

        private void FrmTabs_Leave(object sender, EventArgs e)
        {
            pc.focus = false;
        }

        public List<dfCom> freeSearch(string args)
        {
            List<dfCom> list = new List<dfCom>();
            try
            {

                OleDbConnection cn = new OleDbConnection();
                DataTable schemaTable;

                cn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Properties.Settings.Default.CRoot + "Base.mdb"; 
                cn.Open();

                //Retrieve schema information about tables.
                //Because tables include tables, views, and other objects,
                //restrict to just TABLE in the Object array of restrictions.
                schemaTable = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                              new Object[] { null, null, null, "TABLE" });
                string[] tables = new string[schemaTable.Rows.Count];
                //List the table name from each row in the schema table.
                for (int i = 0; i < schemaTable.Rows.Count; i++)
                {
                    tables[i] = schemaTable.Rows[i].ItemArray[2].ToString();
                }

                //Explicitly close - don't wait on garbage collection.
                cn.Close();
                da.SelectCommand = new OleDbCommand();
                da.SelectCommand.Connection = cn;
                
                for (int i = 0; i < tables.Count(); i++)
                {
                    if (tables[i].Contains("tab"))
                    {
                        try
                        {
                            string Tipo = tables[i].Substring(tables[i].LastIndexOf("tab") + 3);
                            ds1.Reset();
                            if (Tipo.ToLower().Contains(args))
                                da.SelectCommand.CommandText = "select * from " + tables[i];
                            else
                            da.SelectCommand.CommandText = "select * from " + tables[i] + "  where nome LIKE '%" + args + "%'";
                            da.Fill(ds1, tables[i]);
                            
                            
                            DataTable t = ds1.Tables[0];
                            for (int j = 0; j < t.Rows.Count; j++)
                                if (Tipo != "Outros")
                                {
                                    list.Add(new dfCom(t.Rows[j][0].ToString(), t.Rows[j][1].ToString(), Tipo, float.Parse(t.Rows[j][6].ToString()), t.Rows[j][4].ToString()));
                                }
                                else
                                {
                                    list.Add(new dfCom(t.Rows[j][0].ToString(), t.Rows[j][1].ToString(), t.Rows[j][2].ToString(), float.Parse(t.Rows[j][6].ToString()), t.Rows[j][4].ToString(),true));
                                }
                        }
                        catch (OleDbException)
                        {
                            MessageBox.Show("Erro no banco de dados", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                    }
                }
                return list;


            }
            catch (OleDbException)
            {
                MessageBox.Show("Erro no banco de dados", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            } return new List<dfCom> { };
        }
        public List<dfCom> tagSearch(string args)
        {
            List<dfCom> list = new List<dfCom>();
            try
            {

                OleDbConnection cn = new OleDbConnection();
                DataTable schemaTable;

                cn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Properties.Settings.Default.CRoot + "Base.mdb";
                cn.Open();

                //Retrieve schema information about tables.
                //Because tables include tables, views, and other objects,
                //restrict to just TABLE in the Object array of restrictions.
                schemaTable = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                              new Object[] { null, null, null, "TABLE" });
                string[] tables = new string[schemaTable.Rows.Count];
                //List the table name from each row in the schema table.
                for (int i = 0; i < schemaTable.Rows.Count; i++)
                {
                    tables[i] = schemaTable.Rows[i].ItemArray[2].ToString();
                }

                //Explicitly close - don't wait on garbage collection.
                cn.Close();
                da.SelectCommand = new OleDbCommand();
                da.SelectCommand.Connection = cn;

                for (int i = 0; i < tables.Count(); i++)
                {
                    if (tables[i].Contains("tab"))
                    {
                        try
                        {
                            string Tipo = tables[i].Substring(tables[i].LastIndexOf("tab") + 3);
                            ds1.Reset();
                                da.SelectCommand.CommandText = "select * from " + tables[i] + "  where tags LIKE '%" + args + "%'";
                            da.Fill(ds1, tables[i]);


                            DataTable t = ds1.Tables[0];
                            for (int j = 0; j < t.Rows.Count; j++)
                                if (Tipo != "Outros")
                                {
                                    list.Add(new dfCom(t.Rows[j][0].ToString(), t.Rows[j][1].ToString(), Tipo, float.Parse(t.Rows[j][6].ToString()), t.Rows[j][4].ToString()));
                                }
                                else
                                {
                                    list.Add(new dfCom(t.Rows[j][0].ToString(), t.Rows[j][1].ToString(), t.Rows[j][2].ToString(), float.Parse(t.Rows[j][6].ToString()), t.Rows[j][4].ToString(), true));
                                }
                        }
                        catch (OleDbException)
                        {
                            MessageBox.Show("Erro no banco de dados", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                    }
                }
                return list;


            }
            catch (OleDbException)
            {
                MessageBox.Show("Erro no banco de dados", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            } return new List<dfCom> { };
        }
        public List<dfCom> cbSearch(string args)
        {
            List<dfCom> list = new List<dfCom>();
            try
            {

                OleDbConnection cn = new OleDbConnection();

                cn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Properties.Settings.Default.CRoot + "Base.mdb";
                cn.Open();

                da.SelectCommand = new OleDbCommand();
                da.SelectCommand.Connection = cn;



                string str1 = args.Replace("Placa de ", "Pla");

                ds1.Reset();
                da.SelectCommand.CommandText = "select * from tab" + str1;
                da.Fill(ds1, "tab" + str1);

                string Tipo = str1;
                DataTable t = ds1.Tables[0];
                for (int j = 0; j < t.Rows.Count; j++)
                    if (Tipo != "Outros")
                    {
                        list.Add(new dfCom(t.Rows[j][0].ToString(), t.Rows[j][1].ToString(), Tipo, float.Parse(t.Rows[j][6].ToString()), t.Rows[j][4].ToString()));
                    }
                    else
                    {
                        list.Add(new dfCom(t.Rows[j][0].ToString(), t.Rows[j][1].ToString(), t.Rows[j][2].ToString(), float.Parse(t.Rows[j][6].ToString()), t.Rows[j][4].ToString(), true));
                    }




                return list;


            }
            catch (OleDbException)
            {
                MessageBox.Show("Erro no banco de dados", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            } return new List<dfCom> { };
        }
        public List<dfCom> cbSearch(string args1, string args2)
        {
            List<dfCom> list = new List<dfCom>();
            try
            {

                OleDbConnection cn = new OleDbConnection();

                cn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Properties.Settings.Default.CRoot + "Base.mdb";
                cn.Open();

                da.SelectCommand = new OleDbCommand();
                da.SelectCommand.Connection = cn;



                string str1 = args1.Replace("Placa de ", "Pla");
                string str2 = args2;

                ds1.Reset();
                da.SelectCommand.CommandText = "select * from tab" + str1 + " where f1 = '" + str2 + "'";
                da.Fill(ds1, "tab" + str1);

                string Tipo = str1;
                DataTable t = ds1.Tables[0];
                for (int j = 0; j < t.Rows.Count; j++)
                    if (Tipo != "Outros")
                    {
                        list.Add(new dfCom(t.Rows[j][0].ToString(), t.Rows[j][1].ToString(), Tipo, float.Parse(t.Rows[j][6].ToString()), t.Rows[j][4].ToString()));
                    }
                    else
                    {
                        list.Add(new dfCom(t.Rows[j][0].ToString(), t.Rows[j][1].ToString(), t.Rows[j][2].ToString(), float.Parse(t.Rows[j][6].ToString()), t.Rows[j][4].ToString(), true));
                    }




                return list;


            }
            catch (OleDbException)
            {
                MessageBox.Show("Erro no banco de dados", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            } return new List<dfCom> { };
        }
        public List<dfCom> cbSearch(string args1, string args2,string args3)
        {
            List<dfCom> list = new List<dfCom>();
            try
            {

                OleDbConnection cn = new OleDbConnection();

                cn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Properties.Settings.Default.CRoot + "Base.mdb";
                cn.Open();

                da.SelectCommand = new OleDbCommand();
                da.SelectCommand.Connection = cn;



                string str1 = args1.Replace("Placa de ", "Pla");
                string str2 = args2;
                string str3 = args3;

                ds1.Reset();
                da.SelectCommand.CommandText = "select * from tab" + str1 + " where f1 = '" + str2 + "' and f2 = '"+str3+"'";
                da.Fill(ds1, "tab" + str1);

                string Tipo = str1;
                DataTable t = ds1.Tables[0];
                for (int j = 0; j < t.Rows.Count; j++)
                    if (Tipo != "Outros")
                    {
                        list.Add(new dfCom(t.Rows[j][0].ToString(), t.Rows[j][1].ToString(), Tipo, float.Parse(t.Rows[j][6].ToString()), t.Rows[j][4].ToString()));
                    }
                    else
                    {
                        list.Add(new dfCom(t.Rows[j][0].ToString(), t.Rows[j][1].ToString(), t.Rows[j][2].ToString(), float.Parse(t.Rows[j][6].ToString()), t.Rows[j][4].ToString(), true));
                    }




                return list;


            }
            catch (OleDbException)
            {
                MessageBox.Show("Erro no banco de dados", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            } return new List<dfCom> { };
        }

        public List<string> cb1Search2(string args)
        {
            List<string> list = new List<string>();
            try
            {
                string str = args.Replace("Placa de ", "Pla");
                OleDbConnection cn = new OleDbConnection();

                cn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Properties.Settings.Default.CRoot + "Base.mdb";
                cn.Open();
                da.SelectCommand = new OleDbCommand();
                da.SelectCommand.Connection = cn;

                ds1.Reset();
                da.SelectCommand.CommandText = "select f1 from tab" + str + " group by f1";
                da.Fill(ds1, "tab" + str );

                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {

                    list.Add(ds1.Tables[0].Rows[i][0].ToString());
                }

                return list;


            }
            catch (OleDbException)
            {
                MessageBox.Show("Erro no banco de dados", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            } return new List<string> { };
        }
        public List<string> cb2Search2(string args1,string args2)
        {
            List<string> list = new List<string>();
            try
            {
                string str1 = args1.Replace("Placa de ", "Pla");
                string str2 = args2;
                OleDbConnection cn = new OleDbConnection();

                cn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Properties.Settings.Default.CRoot + "Base.mdb";
                cn.Open();
                da.SelectCommand = new OleDbCommand();
                da.SelectCommand.Connection = cn;

                ds1.Reset();
                da.SelectCommand.CommandText = "select f2 from tab" + str1 + " where f1 = '" + str2 + "' group by f2";
                da.Fill(ds1, "tab" + str1);

                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {

                    list.Add(ds1.Tables[0].Rows[i][0].ToString());
                }

                return list;


            }
            catch (OleDbException)
            {
                MessageBox.Show("Erro no banco de dados", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            } return new List<string> { };
        }
    }
}
