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
            Thread newThread = new Thread(new ThreadStart(AbrirImagem));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
            
        }
        private void AbrirImagem()
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Title = "Escolha uma imagem";
            o.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            o.ShowDialog();
            
            if (tbImg.InvokeRequired)
tbImg.BeginInvoke((MethodInvoker)delegate {
    if(o.CheckFileExists){
        tbImg.Text = o.FileName;
    }
                        else
                    {
                        MessageBox.Show("Arquivo nao existe");
                    }
 ;});
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

            if (tbImg.Text != "")
            {
                FileInfo imgF = PadronizarIMG(tbImg.Text);


                DirectoryInfo dir = textures.CreateSubdirectory(cbTipo.Text);
                FileInfo file = imgF.CopyTo(dir.FullName + "\\" + tbCod.Text + ".jpg", true);

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
            else
            {
                MessageBox.Show("Sem Imagem"); 
            }
            if (tb3D.Text != "")
            {
                DirectoryInfo model3d = Padronizar3D(tb3D.Text);
                DirectoryInfo dir2 = model.CreateSubdirectory(cbTipo.Text);
                model3d.MoveTo(dir2.FullName + "\\" + tbCod.Text);
                model3d.GetFiles()[0].MoveTo(model3d.FullName + "\\" + tbCod.Text + ".fbx");
            }
            else
            {
                MessageBox.Show("Sem Modelo"); 
            }

        }

        private DirectoryInfo Padronizar3D(string p)
        {
            FileInfo mod = new FileInfo(p);

            string modN = mod.Name.Replace(mod.Extension,""); 

            DirectoryInfo dir = mod.Directory;

            // Open a file for reading
            StreamReader streamReader;
            streamReader = File.OpenText(mod.FullName);
            // Now, read the entire file into a strin
            string contents = streamReader.ReadToEnd();
            streamReader.Close();
            string[] main = contents.Split(new string[]{"RelativeFilename:"}, StringSplitOptions.None);
            string fbx = "";
            fbx += main[0] + " RelativeFilename:";
            for (int i = 1; i < main.Count(); i++)
            {
                if (i != main.Count() - 1)
                {
                    string[] x1 = main[i].Split(new string[] { "\\" + modN }, StringSplitOptions.None);
                    fbx += " \"" + modN + x1[1] + " RelativeFilename:";
                }
                else
                {
                    string[] x1 = main[i].Split(new string[] { "\\" + modN }, StringSplitOptions.None);
                    fbx += " \"" + modN + x1[1];
                }
            }
            Random x = new Random();
            DirectoryInfo r = Directory.CreateDirectory(appPath + "\\" + modN + x.Next(0,1031203810));
            StreamWriter streamWriter = File.CreateText(r.FullName + "\\" + modN+".fbx");

            streamWriter.Write(fbx);
            streamWriter.Close();

            copyDirectory(dir.FullName + "\\" + modN, r.FullName + "\\" + modN);
            return r;
        }
        public static void copyDirectory(string Src, string Dst)
        {
            String[] Files;

            if (Dst[Dst.Length - 1] != Path.DirectorySeparatorChar)
                Dst += Path.DirectorySeparatorChar;
            if (!Directory.Exists(Dst)) Directory.CreateDirectory(Dst);
            Files = Directory.GetFileSystemEntries(Src);
            foreach (string Element in Files)
            {
                // Sub directories

                if (Directory.Exists(Element))
                    copyDirectory(Element, Dst + Path.GetFileName(Element));
                // Files in directory

                else
                    File.Copy(Element, Dst + Path.GetFileName(Element), true);
            }
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
            ApagaCont();
            fillF();
            ExportarCont();
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
            ApagaCont();
            fillF();

        }

        private void ApagaCont()
        {
            DirectoryInfo d = new DirectoryInfo(appPath);
            DirectoryInfo f = d.CreateSubdirectory("TempCont");
            DirectoryInfo model = f.CreateSubdirectory("Models");
            DirectoryInfo textures = f.CreateSubdirectory("Textures");


            DirectoryInfo dir = textures.CreateSubdirectory(cbTipo.Text);
            try
            {
                FileInfo file = new FileInfo(dir.FullName + "\\" + tbCod.Text + ".jpg");
                try
                {
                    pb1.Image.Dispose();
                    pb1.Image = null;
                    imgF.Delete();
                }
                catch (Exception casd) { }
                file.Delete();
            }
            catch
            {
                MessageBox.Show("Imagem nao encontrada");
            }

            DirectoryInfo dir2 = model.CreateSubdirectory(cbTipo.Text);
            try
            {
                DirectoryInfo model3d = new DirectoryInfo(dir2.FullName + "\\" + tbCod.Text);
                model3d.Delete(true);
            }
            catch
            {
                MessageBox.Show("Modelo nao encontrado");
            }

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

        private void but3d_Click(object sender, EventArgs e)
        {
            Thread newThread = new Thread(new ThreadStart(Abrir3D));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
        }
        private void Abrir3D()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Escolha uma Um modelo 3D";
            op.Filter = "FBX files (*.fbx)|*.fbx";
            op.ShowDialog();

            if (tb3D.InvokeRequired)
                tb3D.BeginInvoke((MethodInvoker)delegate
                {
                    if (op.CheckFileExists)
                    {
                        tb3D.Text = op.FileName;
                    }
                    else
                    {
                        MessageBox.Show("Arquivo nao existe");
                    }
                });
        }
    }
}
