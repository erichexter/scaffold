namespace Flywheel.UI.Impl
{
    partial class InstallTemplatesView
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
            this.ddlProjects = new System.Windows.Forms.ComboBox();
            this.lbTemplateSets = new System.Windows.Forms.CheckedListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnInstall = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ddlProjects
            // 
            this.ddlProjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlProjects.FormattingEnabled = true;
            this.ddlProjects.Location = new System.Drawing.Point(13, 13);
            this.ddlProjects.Name = "ddlProjects";
            this.ddlProjects.Size = new System.Drawing.Size(267, 21);
            this.ddlProjects.TabIndex = 0;
            this.ddlProjects.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lbTemplateSets
            // 
            this.lbTemplateSets.CheckOnClick = true;
            this.lbTemplateSets.FormattingEnabled = true;
            this.lbTemplateSets.Location = new System.Drawing.Point(13, 41);
            this.lbTemplateSets.Name = "lbTemplateSets";
            this.lbTemplateSets.Size = new System.Drawing.Size(267, 184);
            this.lbTemplateSets.TabIndex = 1;
            this.lbTemplateSets.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(205, 237);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnInstall
            // 
            this.btnInstall.Location = new System.Drawing.Point(124, 237);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(75, 23);
            this.btnInstall.TabIndex = 3;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // InstallTemplatesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 272);
            this.Controls.Add(this.btnInstall);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lbTemplateSets);
            this.Controls.Add(this.ddlProjects);
            this.Name = "InstallTemplatesView";
            this.Text = "InstallTemplatesView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox ddlProjects;
        private System.Windows.Forms.CheckedListBox lbTemplateSets;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnInstall;
    }
}