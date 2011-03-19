namespace DarkFalcon_v3
{
    partial class FrmInterface2
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
            this.components = new System.ComponentModel.Container();
            this.listBoxC = new System.Windows.Forms.ListBox();
            this.imageListC = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // listBoxC
            // 
            this.listBoxC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxC.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxC.HorizontalExtent = 4;
            this.listBoxC.Location = new System.Drawing.Point(1, 1);
            this.listBoxC.MultiColumn = true;
            this.listBoxC.Name = "listBoxC";
            this.listBoxC.Size = new System.Drawing.Size(570, 186);
            this.listBoxC.Sorted = true;
            this.listBoxC.TabIndex = 1;
            this.listBoxC.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxC_DrawItem);
            this.listBoxC.SelectedIndexChanged += new System.EventHandler(this.listBoxC_SelectedIndexChanged);
            // 
            // imageListC
            // 
            this.imageListC.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListC.ImageSize = new System.Drawing.Size(130, 130);
            this.imageListC.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // FrmInterface2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(574, 189);
            this.Controls.Add(this.listBoxC);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmInterface2";
            this.Text = "Form1";
            this.TransparencyKey = System.Drawing.Color.Lime;
            this.Load += new System.EventHandler(this.FrmInterface2_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxC;
        private System.Windows.Forms.ImageList imageListC;
    }
}