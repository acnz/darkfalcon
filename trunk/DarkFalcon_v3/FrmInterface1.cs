using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using DarkFalcon.df;

namespace DarkFalcon
{
    public partial class FrmInterface1 : Form
    {
        FrmInterface2 frm2;

        public dfPC myPc = new dfPC(true);
        public FrmInterface1()
        {
            InitializeComponent();
        }
        public void getInter2(FrmInterface2 frm)
        {
            frm2 = frm;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dataSet1.Reset();
                oleDbDataAdapter1.SelectCommand.CommandText = "select f1 from tab" + comboBox1.Text.Replace("Placa de ","Pla").Replace("Placa de ","Pla") + " group by f1";
                oleDbDataAdapter1.Fill(dataSet1, "tab" + comboBox1.Text.Replace("Placa de ","Pla"));
                dataSet2.Reset();
                oleDbDataAdapter2.SelectCommand.CommandText = "select cod,nome from tab" + comboBox1.Text.Replace("Placa de ","Pla");
                oleDbDataAdapter2.Fill(dataSet2, "tab" + comboBox1.Text.Replace("Placa de ","Pla"));

            }
            catch (OleDbException)
            {
                MessageBox.Show("Erro no banco de dados", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            comboBox2.Items.Clear();
            comboBox2.Text = null;
            comboBox3.Items.Clear();
            comboBox3.Text = null;
            comboBox4.Items.Clear();
            comboBox4.Text = null;
            for (int i =0;i < dataSet1.Tables[0].Rows.Count;i++)
            {
                
                comboBox2.Items.Add(dataSet1.Tables[0].Rows[i][0].ToString());
            }
            frm2.listar(dataSet2,comboBox1.Text.Replace("Placa de ","Pla"));
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            try
            {
                dataSet1.Reset();
                oleDbDataAdapter1.SelectCommand.CommandText = "select f2 from tab" + comboBox1.Text.Replace("Placa de ","Pla") + " where f1 = '" + comboBox2.Text + "' group by f2";
                oleDbDataAdapter1.Fill(dataSet1, "tab" + comboBox1.Text.Replace("Placa de ","Pla"));
                dataSet2.Reset();
                oleDbDataAdapter2.SelectCommand.CommandText = "select cod,nome from tab" + comboBox1.Text.Replace("Placa de ","Pla") + " where f1 = '" + comboBox2.Text + "'";
                oleDbDataAdapter2.Fill(dataSet2, "tab" + comboBox1.Text.Replace("Placa de ","Pla"));

            }
            catch (OleDbException)
            {
                MessageBox.Show("Erro no banco de dados", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            comboBox3.Items.Clear();
            comboBox3.Text = null;
            comboBox4.Items.Clear();
            comboBox4.Text = null;
            for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
            {

                comboBox3.Items.Add(dataSet1.Tables[0].Rows[i][0].ToString());
            }
            frm2.listar(dataSet2, comboBox1.Text.Replace("Placa de ","Pla"));
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dataSet1.Reset();
                oleDbDataAdapter1.SelectCommand.CommandText = "select nome from tab" + comboBox1.Text.Replace("Placa de ","Pla") + "  where f1 = '" + comboBox2.Text + "' and f2 = '" + comboBox3.Text + "'";
                oleDbDataAdapter1.Fill(dataSet1, "tab" + comboBox1.Text.Replace("Placa de ","Pla"));
                dataSet2.Reset();
                oleDbDataAdapter2.SelectCommand.CommandText = "select cod,nome from tab" + comboBox1.Text.Replace("Placa de ","Pla") + "  where f1 = '" + comboBox2.Text + "' and f2 = '" + comboBox3.Text + "'";
                oleDbDataAdapter2.Fill(dataSet2, "tab" + comboBox1.Text.Replace("Placa de ","Pla"));

            }
            catch (OleDbException)
            {
                MessageBox.Show("Erro no banco de dados", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            comboBox4.Items.Clear();
            comboBox4.Text = null;
            for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
            {

                comboBox4.Items.Add(dataSet1.Tables[0].Rows[i][0].ToString());
            }
            frm2.listar(dataSet2, comboBox1.Text.Replace("Placa de ","Pla"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
                foreach (Control c in this.panel1.Controls)
                {
                    if (c.Name != "comboBox1")
                    {
                        if (c.GetType().ToString() == "System.Windows.Forms.ComboBox")
                        {
                            ((ComboBox)c).Items.Clear();
                            ((ComboBox)c).Text = null;
                        }
                    }
                }
        }
        public DataSet getDataSetAtual()
        {
            try
            {
                dataSet1.Reset();
                oleDbDataAdapter1.SelectCommand.CommandText = "select nome from tab" + comboBox1.Text.Replace("Placa de ","Pla") + "  where f1 = '" + comboBox2.Text + "' and f2 = '" + comboBox3.Text + "'";
                oleDbDataAdapter1.Fill(dataSet1, "tab" + comboBox1.Text.Replace("Placa de ","Pla"));
                dataSet2.Reset();
                if(comboBox3.SelectedItem != null)
                    oleDbDataAdapter2.SelectCommand.CommandText = "select cod,nome from tab" + comboBox1.Text.Replace("Placa de ","Pla") + "  where f1 = '" + comboBox2.Text + "' and f2 = '" + comboBox3.Text + "'";
               else if (comboBox3.SelectedItem != null)
                    oleDbDataAdapter2.SelectCommand.CommandText = "select cod,nome from tab" + comboBox1.Text.Replace("Placa de ","Pla") + "  where f1 = '" + comboBox2.Text + "'";
                else
                    oleDbDataAdapter2.SelectCommand.CommandText = "select cod,nome from tab" + comboBox1.Text.Replace("Placa de ","Pla");
                oleDbDataAdapter2.Fill(dataSet2, "tab" + comboBox1.Text.Replace("Placa de ","Pla"));

            }
            catch (OleDbException)
            {
                MessageBox.Show("Erro no banco de dados", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return dataSet2;
        }
        public DataSet getFullDataSetAtual()
        {
            try
            {
                dataSet1.Reset();
                oleDbDataAdapter1.SelectCommand.CommandText = "select * from tab" + comboBox1.Text.Replace("Placa de ","Pla") + "  where f1 = '" + comboBox2.Text + "' and f2 = '" + comboBox3.Text + "'";
                oleDbDataAdapter1.Fill(dataSet1, "tab" + comboBox1.Text.Replace("Placa de ","Pla"));
                dataSet2.Reset();
                if(comboBox3.SelectedItem != null)
                    oleDbDataAdapter2.SelectCommand.CommandText = "select * from tab" + comboBox1.Text.Replace("Placa de ","Pla") + "  where f1 = '" + comboBox2.Text + "' and f2 = '" + comboBox3.Text + "'";
               else if (comboBox3.SelectedItem != null)
                    oleDbDataAdapter2.SelectCommand.CommandText = "select * from tab" + comboBox1.Text.Replace("Placa de ","Pla") + "  where f1 = '" + comboBox2.Text + "'";
                else
                    oleDbDataAdapter2.SelectCommand.CommandText = "select * from tab" + comboBox1.Text.Replace("Placa de ","Pla");
                oleDbDataAdapter2.Fill(dataSet2, "tab" + comboBox1.Text.Replace("Placa de ","Pla"));

            }
            catch (OleDbException)
            {
                MessageBox.Show("Erro no banco de dados", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return dataSet2;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void oleDbConnection1_InfoMessage(object sender, OleDbInfoMessageEventArgs e)
        {

        }

        private void FrmInterface1_Load(object sender, EventArgs e)
        {
            oleDbConnection1.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Properties.Settings.Default.CRoot+"Base.mdb";
        }


        internal void listar()
        {
            listBox1.Items.Clear();
            foreach(dfCom d in myPc.GetAllCom()){
                listBox1.Items.Add(d.Nome);
            }
        }
    }
}
