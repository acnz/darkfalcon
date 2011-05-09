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
    public partial class FrmInterface2 : Form
    {
        FrmInterface1 frm1;

        public FrmInterface2()
        {
            InitializeComponent();
        }
        public void getInter1(FrmInterface1 frm)
        {
            frm1 = frm;
        }
        

        private void listBoxC_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            //Select the Icon that you want to display
            Image i = imageListC.Images[e.Index];
            int t = imageListC.ImageSize.Height;
            // Get the Bounding rectangle

            // Get the Bounding rectangle
            Rectangle rc = new Rectangle(e.Bounds.X + 5, e.Bounds.Y + 5, e.Bounds.Width - 10, e.Bounds.Height - 25);

           

            // Setup the stringformatting object
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            // Get the item text
            listBoxC = (ListBox)sender;
            string str = (string)listBoxC.Items[e.Index];

            // Draw the rectangle
            e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black), 2), rc);
            e.Graphics.FillRectangle(new SolidBrush(Color.White), rc);
            //Console.Out.WriteLine(e.State.ToString());
            // Check if the item is selected
            if (e.State == (DrawItemState.NoAccelerator | DrawItemState.NoFocusRect))
            {
                
                // Paint the item that if not selected
                e.Graphics.FillRectangle(new SolidBrush(Color.White), rc);
                e.Graphics.DrawImage(i, e.Bounds.X + (e.Bounds.Width / 2 - t / 2), e.Bounds.Y + (e.Bounds.Height / 2 - t / 2), t, t);
                e.Graphics.DrawString(str, new Font("Ariel", 12), new SolidBrush(Color.Black), rc, sf);
            }
            else
            {
                // Paint the item accordingly if it is selected
                e.DrawFocusRectangle();
                e.Graphics.FillRectangle(new SolidBrush(Color.Gray), rc);
                e.Graphics.DrawImage(i, e.Bounds.X +( e.Bounds.Width/2 - t/2),e.Bounds.Y+(e.Bounds.Height/2 - t/2),t,t);
                e.Graphics.DrawString(str, new Font("Ariel", 12), new SolidBrush(Color.Black), rc, sf);



            }
        }

        

        private void listBoxC_SelectedIndexChanged(object sender, EventArgs e)
        {
      // Unbox the sender
            PcView pc = ((Frm3D)MdiParent.MdiChildren[2]).pc;
            DataSet data = frm1.getFullDataSetAtual();
            listBoxC = (ListBox)sender;
            int i = listBoxC.SelectedIndex;
            DialogResult result= MessageBox.Show("Deseja adicionar a peça "+listBoxC.Items[i]+" ao seu Coputador?","Adicionar Peça",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                string Tipo = data.Tables[0].TableName.Substring(data.Tables[0].TableName.LastIndexOf("tab") + 3);
                string r="";
                if(Tipo != "Outros"){
                    r=frm1.myPc.add(new dfCom(data.Tables[0].Rows[i][0].ToString(), data.Tables[0].Rows[i][1].ToString(), Tipo, float.Parse(data.Tables[0].Rows[i][6].ToString()), data.Tables[0].Rows[i][4].ToString()));
                }else{
                    r = frm1.myPc.add(new dfCom(data.Tables[0].Rows[i][0].ToString(), data.Tables[0].Rows[i][1].ToString(), data.Tables[0].Rows[i][2].ToString(), float.Parse(data.Tables[0].Rows[i][6].ToString()), data.Tables[0].Rows[i][4].ToString()));
                 }
                Console.Out.WriteLine(r);
                frm1.listar();
            }
        }

        public void listar(DataSet data, string tipo)
        {
            listBoxC.Items.Clear();
            imageListC.Images.Clear();

            for (int i = 0; i < data.Tables[0].Rows.Count; i++)
            {
                listBoxC.Items.Add(data.Tables[0].Rows[i][1].ToString());
                imageListC.Images.Add(PegarImagens(tipo+"\\"+data.Tables[0].Rows[i][0].ToString()));
            }

        }

        public Bitmap PegarImagens(string path)
        {
            PcView pc = ((Frm3D)MdiParent.MdiChildren[2]).pc;
            return pc.getBmp("Textures\\"+path);
            
        }

        private void FrmInterface2_Load(object sender, EventArgs e)
        {
            listBoxC.ItemHeight = 181;
            listBoxC.ColumnWidth = 181;
        }
    }
}
