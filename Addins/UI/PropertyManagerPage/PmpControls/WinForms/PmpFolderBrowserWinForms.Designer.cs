
namespace Hymma.SolidTools.Addins
{
    partial class PmpFolderBrowserWinForms
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.Browse = new System.Windows.Forms.Button();
            this.Addresses = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Browse
            // 
            this.Browse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.Browse.Location = new System.Drawing.Point(427, 7);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(28, 23);
            this.Browse.TabIndex = 0;
            this.Browse.Text = "...";
            this.Browse.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // Addresses
            // 
            this.Addresses.FormattingEnabled = true;
            this.Addresses.Location = new System.Drawing.Point(3, 7);
            this.Addresses.Name = "Addresses";
            this.Addresses.Size = new System.Drawing.Size(415, 21);
            this.Addresses.TabIndex = 1;
            // 
            // PmpFolderBrowserWinForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Addresses);
            this.Controls.Add(this.Browse);
            this.Name = "PmpFolderBrowserWinForms";
            this.Size = new System.Drawing.Size(458, 36);
            this.Load += new System.EventHandler(this.PmpFolderBrowserWinForms_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.ComboBox Addresses;
    }
}
