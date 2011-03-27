using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.OleDb;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading;

namespace DarkFalcon_v3
{
    public partial class FrmAdmDB : Form
    {
        public bool tSelected = false;
        public List<string[]> labs = new List<string[]>();
        private DataSet ds = new DataSet();
        private int iindex = 0;
        private OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Base.mdb");
        private OleDbDataAdapter da = new OleDbDataAdapter();
        string appPath = Path.GetDirectoryName(Application.ExecutablePath);
        public FrmAdmDB()
        {
            InitializeComponent();
        }

        private void FrmAdmDB_Load(object sender, EventArgs e)
        {
            DialogResult r =MessageBox.Show("Atualize o banco de dados Primerio","Atualizar",MessageBoxButtons.OKCancel,MessageBoxIcon.Asterisk);
            if (r == DialogResult.OK)
            {
                ProcessStartInfo sInfo = new ProcessStartInfo("http://www.4shared.com/account/home.jsp?startPage=1&sId=E5EeNfVMCsOJxGtG");

                Process.Start(sInfo);
            }
           cbTipo.SelectedIndex = 0;
           fillF();

            //mother
            //string[] a0 = new string[3];
            //a0[0] = "Fabricante";
            //a0[1] = "Socket";
            //a0[2] = "";
            //labs.Add(a0);

        
            
        }

        private void fillF()
        {
            fillDS();
            if (ds.Tables[0].Rows.Count > 0)
            {
                tbCod.Text = ds.Tables[0].Rows[iindex][0].ToString();
                tbNome.Text = ds.Tables[0].Rows[iindex][1].ToString();
                tbPreco.Text = ds.Tables[0].Rows[iindex][6].ToString();
                tbi1.Text = ds.Tables[0].Rows[iindex][2].ToString();
                tbi2.Text = ds.Tables[0].Rows[iindex][3].ToString();
                tbtag.Text = ds.Tables[0].Rows[iindex][4].ToString();
                tbsite.Text = ds.Tables[0].Rows[iindex][5].ToString();
            }
        }

        private void fillDS()
        {
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Base.mdb";
            da.SelectCommand = new OleDbCommand("Select * from tab" + cbTipo.Text);
            da.SelectCommand.Connection = con;
            ds.Reset();
            da.Fill(ds, "tab" + cbTipo.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (iindex < ds.Tables[0].Rows.Count - 1)
            {
                iindex = iindex + 1;
                fillF();
            }
            else
            {
                MessageBox.Show("voce ja esta no ultimo registro");
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Thread newThread = new Thread(new ThreadStart(AbrirArquivo));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
            
        }
        private void AbrirArquivo()
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Title = "Escolha uma imagem";
            o.ShowDialog();
            if (tbImg.InvokeRequired)
tbImg.BeginInvoke((MethodInvoker)delegate {
    tbImg.Text = o.FileName;});
        }

        private void cbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            iindex = 0;
            try
            {
                fillF();
            }
            catch (Exception exc)
            {
            }
            tSelected = true;
        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            String valor = "insert into tab" + cbTipo.Text + "(nome,f1,f2,tags,info,preco)values('" + tbNome.Text + "','" + tbi1.Text + "','" + tbi2.Text + "','" + tbtag.Text + "','" + tbsite.Text + "','" + tbPreco.Text + "')";
            OleDbCommand grava = new OleDbCommand(valor, con);
            con.Open();
            grava.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("gravado");
            fillDS();
            iindex = ds.Tables[0].Rows.Count - 1;
            fillF();

            ExportarCont();
            
        }

        private void ExportarCont()
        {
            DirectoryInfo d = new DirectoryInfo(appPath);
            DirectoryInfo f = d.CreateSubdirectory("TempCont");
            DirectoryInfo model = f.CreateSubdirectory("Models");
            DirectoryInfo textures = f.CreateSubdirectory("Textures");

           FileInfo imgF =  PadronizarIMG(tbImg.Text);


            DirectoryInfo dir = textures.CreateSubdirectory(cbTipo.Text);
            FileInfo file = imgF.CopyTo(dir.FullName+"\\"+tbCod.Text+".jpg",true);

            try
            {
                pb1.Image.Dispose();
                pb1.Image = null;
                imgF.Delete();
            }
            catch (Exception casd) { }
            if (file != null)
                pb1.Image = Image.FromFile(file.FullName);


        }

        private FileInfo PadronizarIMG(string strFile)
        {
        FileInfo fiPicture = new FileInfo(strFile);
        if (fiPicture.Exists)
        {
            Image newImage = Image.FromFile(strFile);

            Bitmap pad = new Bitmap(300, 300);
            int newW, newH, nx, ny = 0;
           
            if (newImage.Width > newImage.Height)
            {

                newW = 300;
                newH = newImage.Height * 300 / newImage.Width;

                nx = 0;
                ny = (300 - newH) / 2;

            }
            else
            {
                newW = newImage.Width * 300 / newImage.Height;
                newH = 300;

                nx = (300 - newW) / 2;
                ny = 0;
            }
            Graphics g = Graphics.FromImage(pad);

            g.FillRectangle(new SolidBrush(Color.White), 0, 0, 300, 300);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(newImage, new Rectangle(nx, ny, newW, newH));
            g.Dispose();



            Random x = new Random();
            String newpath = appPath + "\\" + x.Next(0, 1000000) + fiPicture.Name;
            FileStream s = new FileStream(newpath,FileMode.Create);
            pad.Save(s, ImageFormat.Jpeg);
            s.Close();
            FileInfo retPic = new FileInfo(newpath);

            return retPic;
        }else
            return null;

        }

        private void butthu_Click(object sender, EventArgs e)
        {
            iindex = 0;
            fillF(); 
        }

        private void buttr_Click(object sender, EventArgs e)
        {
            if (iindex > 0)
            {
                iindex = iindex - 1;
                fillF();
            }
            else
            {
                MessageBox.Show("voce ja esta no primeiro registro");
            }
        }

        private void butfo_Click(object sender, EventArgs e)
        {
                    iindex = ds.Tables[0].Rows.Count - 1;
                    fillF();
        }

        private void butEdit_Click(object sender, EventArgs e)
        {
            String valor = "update tab" + cbTipo.Text + " set nome = '" + tbNome.Text + "',f1 = '" + tbi1.Text + "',f2 = '" + tbi2.Text + "',tags='" + tbtag.Text + "',preco='" + tbPreco.Text + "',info='" + tbsite.Text + "'  where cod=" + tbCod.Text;
            OleDbCommand grava = new OleDbCommand(valor, con);
            con.Open();
            grava.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("alterado");

            fillF();
        }

        private void butRem_Click(object sender, EventArgs e)
        {
            String valor = "delete from tab" + cbTipo.Text + " where cod=" + tbCod.Text;
            OleDbCommand grava = new OleDbCommand(valor, con);
            con.Open();
            grava.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("apagado");
            fillDS();
            iindex = ds.Tables[0].Rows.Count - 1;
            fillF();
        }
        private FileInfo imgF =  new FileInfo("omygodhowwiilputthisfilename.jpg");
        private void tbImg_TextChanged(object sender, EventArgs e)
        {
            try
            {
                pb1.Image.Dispose();
                pb1.Image = null;
                imgF.Delete();
            }
            catch (Exception casd) { }
            imgF = PadronizarIMG(tbImg.Text);
            if (imgF != null)
                pb1.Image = Image.FromFile(imgF.FullName);
        }
    }
}
