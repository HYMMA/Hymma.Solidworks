
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
            this.ComboBoxDropDown = new System.Windows.Forms.ComboBox();
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
<<<<<<< Updated upstream:Addins/UI/PropertyManagerPage/PmpControls/WinForms/PmpFolderBrowserWinForms.Designer.cs
            this.Addresses.FormattingEnabled = true;
            this.Addresses.Location = new System.Drawing.Point(3, 7);
            this.Addresses.Name = "Addresses";
            this.Addresses.Size = new System.Drawing.Size(415, 21);
            this.Addresses.TabIndex = 1;
=======
            this.ComboBoxDropDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComboBoxDropDown.FormattingEnabled = true;
            this.ComboBoxDropDown.Location = new System.Drawing.Point(0, 0);
            this.ComboBoxDropDown.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.ComboBoxDropDown.Name = "Addresses";
            this.ComboBoxDropDown.Size = new System.Drawing.Size(361, 21);
            this.ComboBoxDropDown.TabIndex = 1;
>>>>>>> Stashed changes:Hymma.WinForms/FolderBrowserCombo.Designer.cs
            // 
            // PmpFolderBrowserWinForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
<<<<<<< Updated upstream:Addins/UI/PropertyManagerPage/PmpControls/WinForms/PmpFolderBrowserWinForms.Designer.cs
            this.Controls.Add(this.Addresses);
            this.Controls.Add(this.Browse);
            this.Name = "PmpFolderBrowserWinForms";
            this.Size = new System.Drawing.Size(458, 36);
            this.Load += new System.EventHandler(this.PmpFolderBrowserWinForms_Load);
=======
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ComboBoxDropDown);
            this.Controls.Add(this.Browse);
            this.Name = "FolderBrowserCombo";
            this.Size = new System.Drawing.Size(393, 22);
>>>>>>> Stashed changes:Hymma.WinForms/FolderBrowserCombo.Designer.cs
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button Browse;
        public System.Windows.Forms.ComboBox ComboBoxDropDown;
    }
}
