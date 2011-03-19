using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace DarkFalcon_v3
{
    public partial class FrmInterface1 : Form
    {
        FrmInterface2 frm2;
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
                oleDbDataAdapter1.SelectCommand.CommandText = "select fab from tab"+comboBox1.Text+" group by fab";
                oleDbDataAdapter1.Fill(dataSet1, "tab" + comboBox1.Text);
                dataSet2.Reset();
                oleDbDataAdapter2.SelectCommand.CommandText = "select cod,nome from tab" + comboBox1.Text;
                oleDbDataAdapter2.Fill(dataSet2, "tab" + comboBox1.Text);

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
            frm2.listar(dataSet2,comboBox1.Text);
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            try
            {
                dataSet1.Reset();
                oleDbDataAdapter1.SelectCommand.CommandText = "select socket from tab" + comboBox1.Text + " where fab = '" + comboBox2.Text + "' group by socket";
                oleDbDataAdapter1.Fill(dataSet1, "tab" + comboBox1.Text);
                dataSet2.Reset();
                oleDbDataAdapter2.SelectCommand.CommandText = "select cod,nome from tab" + comboBox1.Text+ " where fab = '" + comboBox2.Text + "'";
                oleDbDataAdapter2.Fill(dataSet2, "tab" + comboBox1.Text);

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
            frm2.listar(dataSet2, comboBox1.Text);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dataSet1.Reset();
                oleDbDataAdapter1.SelectCommand.CommandText = "select nome from tab" + comboBox1.Text + "  where fab = '" + comboBox2.Text + "' and socket = '" + comboBox3.Text + "'";
                oleDbDataAdapter1.Fill(dataSet1, "tab" + comboBox1.Text);
                dataSet2.Reset();
                oleDbDataAdapter2.SelectCommand.CommandText = "select cod,nome from tab" + comboBox1.Text + "  where fab = '" + comboBox2.Text + "' and socket = '" + comboBox3.Text + "'";
                oleDbDataAdapter2.Fill(dataSet2, "tab" + comboBox1.Text);

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
            frm2.listar(dataSet2, comboBox1.Text);
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
                oleDbDataAdapter1.SelectCommand.CommandText = "select nome from tab" + comboBox1.Text + "  where fab = '" + comboBox2.Text + "' and socket = '" + comboBox3.Text + "'";
                oleDbDataAdapter1.Fill(dataSet1, "tab" + comboBox1.Text);
                dataSet2.Reset();
                if(comboBox3.SelectedItem != null)
                oleDbDataAdapter2.SelectCommand.CommandText = "select cod,nome from tab" + comboBox1.Text + "  where fab = '" + comboBox2.Text + "' and socket = '" + comboBox3.Text + "'";
               else if (comboBox3.SelectedItem != null)
                    oleDbDataAdapter2.SelectCommand.CommandText = "select cod,nome from tab" + comboBox1.Text + "  where fab = '" + comboBox2.Text +"'";
                else
                    oleDbDataAdapter2.SelectCommand.CommandText = "select cod,nome from tab" + comboBox1.Text;
                oleDbDataAdapter2.Fill(dataSet2, "tab" + comboBox1.Text);

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

    }
}
