namespace DarkFalcon
{
    partial class FrmAdmDB
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.butthu = new System.Windows.Forms.Button();
            this.buttr = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.butfo = new System.Windows.Forms.Button();
            this.cbTipo = new System.Windows.Forms.ComboBox();
            this.tbi1 = new System.Windows.Forms.TextBox();
            this.tbi2 = new System.Windows.Forms.TextBox();
            this.tbsite = new System.Windows.Forms.TextBox();
            this.tbtag = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labI1 = new System.Windows.Forms.Label();
            this.labI2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labT = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.butImg = new System.Windows.Forms.Button();
            this.tbImg = new System.Windows.Forms.TextBox();
            this.butAdd = new System.Windows.Forms.Button();
            this.tb3D = new System.Windows.Forms.TextBox();
            this.but3d = new System.Windows.Forms.Button();
            this.butEdit = new System.Windows.Forms.Button();
            this.butLimpa = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbNome = new System.Windows.Forms.TextBox();
            this.tbPreco = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbCod = new System.Windows.Forms.TextBox();
            this.butRem = new System.Windows.Forms.Button();
            this.pb1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb1)).BeginInit();
            this.SuspendLayout();
            // 
            // butthu
            // 
            this.butthu.Location = new System.Drawing.Point(12, 12);
            this.butthu.Name = "butthu";
            this.butthu.Size = new System.Drawing.Size(75, 23);
            this.butthu.TabIndex = 0;
            this.butthu.Text = "<<";
            this.butthu.UseVisualStyleBackColor = true;
            this.butthu.Click += new System.EventHandler(this.butthu_Click);
            // 
            // buttr
            // 
            this.buttr.Location = new System.Drawing.Point(93, 12);
            this.buttr.Name = "buttr";
            this.buttr.Size = new System.Drawing.Size(75, 23);
            this.buttr.TabIndex = 0;
            this.buttr.Text = "<";
            this.buttr.UseVisualStyleBackColor = true;
            this.buttr.Click += new System.EventHandler(this.buttr_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(284, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 0;
            this.button3.Text = ">";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button1_Click);
            // 
            // butfo
            // 
            this.butfo.Location = new System.Drawing.Point(365, 12);
            this.butfo.Name = "butfo";
            this.butfo.Size = new System.Drawing.Size(75, 23);
            this.butfo.TabIndex = 0;
            this.butfo.Text = ">>";
            this.butfo.UseVisualStyleBackColor = true;
            this.butfo.Click += new System.EventHandler(this.butfo_Click);
            // 
            // cbTipo
            // 
            this.cbTipo.FormattingEnabled = true;
            this.cbTipo.Items.AddRange(new object[] {
            "Motherboard",
            "Processador",
            "Memoria",
            "Fonte",
            "HD",
            "Monitor",
            "Video",
            "Outros"});
            this.cbTipo.Location = new System.Drawing.Point(30, 62);
            this.cbTipo.Name = "cbTipo";
            this.cbTipo.Size = new System.Drawing.Size(389, 21);
            this.cbTipo.TabIndex = 1;
            this.cbTipo.SelectedIndexChanged += new System.EventHandler(this.cbTipo_SelectedIndexChanged);
            // 
            // tbi1
            // 
            this.tbi1.Location = new System.Drawing.Point(29, 141);
            this.tbi1.Name = "tbi1";
            this.tbi1.Size = new System.Drawing.Size(177, 20);
            this.tbi1.TabIndex = 2;
            // 
            // tbi2
            // 
            this.tbi2.Location = new System.Drawing.Point(241, 141);
            this.tbi2.Name = "tbi2";
            this.tbi2.Size = new System.Drawing.Size(177, 20);
            this.tbi2.TabIndex = 2;
            this.tbi2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // tbsite
            // 
            this.tbsite.Location = new System.Drawing.Point(30, 242);
            this.tbsite.Name = "tbsite";
            this.tbsite.Size = new System.Drawing.Size(389, 20);
            this.tbsite.TabIndex = 2;
            // 
            // tbtag
            // 
            this.tbtag.Location = new System.Drawing.Point(30, 187);
            this.tbtag.Name = "tbtag";
            this.tbtag.Size = new System.Drawing.Size(389, 20);
            this.tbtag.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Tipo";
            // 
            // labI1
            // 
            this.labI1.AutoSize = true;
            this.labI1.Location = new System.Drawing.Point(27, 125);
            this.labI1.Name = "labI1";
            this.labI1.Size = new System.Drawing.Size(31, 13);
            this.labI1.TabIndex = 5;
            this.labI1.Text = "Info1";
            // 
            // labI2
            // 
            this.labI2.AutoSize = true;
            this.labI2.Location = new System.Drawing.Point(238, 125);
            this.labI2.Name = "labI2";
            this.labI2.Size = new System.Drawing.Size(31, 13);
            this.labI2.TabIndex = 5;
            this.labI2.Text = "Info1";
            this.labI2.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 171);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Tags";
            // 
            // labT
            // 
            this.labT.AutoSize = true;
            this.labT.Location = new System.Drawing.Point(27, 210);
            this.labT.Name = "labT";
            this.labT.Size = new System.Drawing.Size(31, 13);
            this.labT.TabIndex = 5;
            this.labT.Text = "Tags";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 226);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Site";
            // 
            // butImg
            // 
            this.butImg.Location = new System.Drawing.Point(30, 272);
            this.butImg.Name = "butImg";
            this.butImg.Size = new System.Drawing.Size(52, 28);
            this.butImg.TabIndex = 6;
            this.butImg.Text = "Imagem";
            this.butImg.UseVisualStyleBackColor = true;
            this.butImg.Click += new System.EventHandler(this.button5_Click);
            // 
            // tbImg
            // 
            this.tbImg.Location = new System.Drawing.Point(88, 277);
            this.tbImg.Name = "tbImg";
            this.tbImg.Size = new System.Drawing.Size(331, 20);
            this.tbImg.TabIndex = 7;
            this.tbImg.TextChanged += new System.EventHandler(this.tbImg_TextChanged);
            // 
            // butAdd
            // 
            this.butAdd.Location = new System.Drawing.Point(70, 343);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(80, 35);
            this.butAdd.TabIndex = 8;
            this.butAdd.Text = "Adicionar";
            this.butAdd.UseVisualStyleBackColor = true;
            this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
            // 
            // tb3D
            // 
            this.tb3D.Location = new System.Drawing.Point(88, 311);
            this.tb3D.Name = "tb3D";
            this.tb3D.Size = new System.Drawing.Size(331, 20);
            this.tb3D.TabIndex = 10;
            // 
            // but3d
            // 
            this.but3d.Location = new System.Drawing.Point(30, 306);
            this.but3d.Name = "but3d";
            this.but3d.Size = new System.Drawing.Size(52, 28);
            this.but3d.TabIndex = 9;
            this.but3d.Text = "3D";
            this.but3d.UseVisualStyleBackColor = true;
            this.but3d.Click += new System.EventHandler(this.but3d_Click);
            // 
            // butEdit
            // 
            this.butEdit.Location = new System.Drawing.Point(156, 343);
            this.butEdit.Name = "butEdit";
            this.butEdit.Size = new System.Drawing.Size(80, 35);
            this.butEdit.TabIndex = 8;
            this.butEdit.Text = "Editar";
            this.butEdit.UseVisualStyleBackColor = true;
            this.butEdit.Click += new System.EventHandler(this.butEdit_Click);
            // 
            // butLimpa
            // 
            this.butLimpa.Location = new System.Drawing.Point(354, 343);
            this.butLimpa.Name = "butLimpa";
            this.butLimpa.Size = new System.Drawing.Size(80, 35);
            this.butLimpa.TabIndex = 8;
            this.butLimpa.Text = "Limpar";
            this.butLimpa.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Nome";
            // 
            // tbNome
            // 
            this.tbNome.Location = new System.Drawing.Point(29, 102);
            this.tbNome.Name = "tbNome";
            this.tbNome.Size = new System.Drawing.Size(177, 20);
            this.tbNome.TabIndex = 11;
            // 
            // tbPreco
            // 
            this.tbPreco.Location = new System.Drawing.Point(241, 102);
            this.tbPreco.Name = "tbPreco";
            this.tbPreco.Size = new System.Drawing.Size(177, 20);
            this.tbPreco.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(239, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Preço";
            // 
            // tbCod
            // 
            this.tbCod.Location = new System.Drawing.Point(178, 15);
            this.tbCod.Name = "tbCod";
            this.tbCod.Size = new System.Drawing.Size(100, 20);
            this.tbCod.TabIndex = 13;
            // 
            // butRem
            // 
            this.butRem.Location = new System.Drawing.Point(242, 343);
            this.butRem.Name = "butRem";
            this.butRem.Size = new System.Drawing.Size(80, 35);
            this.butRem.TabIndex = 8;
            this.butRem.Text = "Remover";
            this.butRem.UseVisualStyleBackColor = true;
            this.butRem.Click += new System.EventHandler(this.butRem_Click);
            // 
            // pb1
            // 
            this.pb1.Location = new System.Drawing.Point(455, 46);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(300, 300);
            this.pb1.TabIndex = 14;
            this.pb1.TabStop = false;
            // 
            // FrmAdmDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 390);
            this.Controls.Add(this.pb1);
            this.Controls.Add(this.tbCod);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbPreco);
            this.Controls.Add(this.tbNome);
            this.Controls.Add(this.tb3D);
            this.Controls.Add(this.but3d);
            this.Controls.Add(this.butLimpa);
            this.Controls.Add(this.butRem);
            this.Controls.Add(this.butEdit);
            this.Controls.Add(this.butAdd);
            this.Controls.Add(this.tbImg);
            this.Controls.Add(this.butImg);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.labT);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labI2);
            this.Controls.Add(this.labI1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbtag);
            this.Controls.Add(this.tbsite);
            this.Controls.Add(this.tbi2);
            this.Controls.Add(this.tbi1);
            this.Controls.Add(this.cbTipo);
            this.Controls.Add(this.butfo);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.buttr);
            this.Controls.Add(this.butthu);
            this.Name = "FrmAdmDB";
            this.Text = "BancoAdm";
            this.Load += new System.EventHandler(this.FrmAdmDB_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butthu;
        private System.Windows.Forms.Button buttr;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button butfo;
        private System.Windows.Forms.ComboBox cbTipo;
        private System.Windows.Forms.TextBox tbi1;
        private System.Windows.Forms.TextBox tbi2;
        private System.Windows.Forms.TextBox tbsite;
        private System.Windows.Forms.TextBox tbtag;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labI1;
        private System.Windows.Forms.Label labI2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labT;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button butImg;
        private System.Windows.Forms.Button butAdd;
        private System.Windows.Forms.TextBox tb3D;
        private System.Windows.Forms.Button but3d;
        private System.Windows.Forms.Button butEdit;
        private System.Windows.Forms.Button butLimpa;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbNome;
        private System.Windows.Forms.TextBox tbPreco;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbCod;
        private System.Windows.Forms.Button butRem;
        private System.Windows.Forms.PictureBox pb1;
        public System.Windows.Forms.TextBox tbImg;
    }
}