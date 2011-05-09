namespace DarkFalcon
{
    partial class Frm3D
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.PicPcView = new System.Windows.Forms.PictureBox();
            this.TabPcs = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.PicPcView)).BeginInit();
            this.TabPcs.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(317, 16);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Pc01";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // PicPcView
            // 
            this.PicPcView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PicPcView.Location = new System.Drawing.Point(0, 0);
            this.PicPcView.Name = "PicPcView";
            this.PicPcView.Size = new System.Drawing.Size(930, 439);
            this.PicPcView.TabIndex = 0;
            this.PicPcView.TabStop = false;
            // 
            // TabPcs
            // 
            this.TabPcs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TabPcs.Controls.Add(this.tabPage1);
            this.TabPcs.Location = new System.Drawing.Point(39, 49);
            this.TabPcs.Name = "TabPcs";
            this.TabPcs.SelectedIndex = 0;
            this.TabPcs.Size = new System.Drawing.Size(325, 42);
            this.TabPcs.TabIndex = 0;
            this.TabPcs.TabIndexChanged += new System.EventHandler(this.TabPcs_TabIndexChanged);
            this.TabPcs.SelectedIndexChanged += new System.EventHandler(this.TabPcs_TabIndexChanged);
            // 
            // Frm3D
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(930, 439);
            this.Controls.Add(this.PicPcView);
            this.Controls.Add(this.TabPcs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Frm3D";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DarkFalcon v3";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.FrmTabs_Deactivate);
            this.Load += new System.EventHandler(this.FrmTabs_Load);
            this.SizeChanged += new System.EventHandler(this.FrmTabs_SizeChanged);
            this.Activated += new System.EventHandler(this.FrmTabs_Activated);
            this.Enter += new System.EventHandler(this.FrmTabs_Enter);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmTabs_FormClosed);
            this.Leave += new System.EventHandler(this.FrmTabs_Leave);
            this.Move += new System.EventHandler(this.FrmTabs_Move_1);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmTabs_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmTabs_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.PicPcView)).EndInit();
            this.TabPcs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.PictureBox PicPcView;
        public System.Windows.Forms.TabControl TabPcs;

    }
}