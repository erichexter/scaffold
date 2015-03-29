namespace Flywheel.UI
{
	partial class TemplateSelectionView
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
			this.cbTemplateSets = new System.Windows.Forms.ComboBox();
			this.lstTemplateList = new System.Windows.Forms.CheckedListBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnCreate = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cbTemplateSets
			// 
			this.cbTemplateSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbTemplateSets.FormattingEnabled = true;
			this.cbTemplateSets.Location = new System.Drawing.Point(18, 14);
			this.cbTemplateSets.Name = "cbTemplateSets";
			this.cbTemplateSets.Size = new System.Drawing.Size(275, 21);
			this.cbTemplateSets.TabIndex = 0;
			this.cbTemplateSets.SelectedIndexChanged += new System.EventHandler(this.cbTemplateSets_SelectedIndexChanged);
			// 
			// lstTemplateList
			// 
			this.lstTemplateList.CheckOnClick = true;
			this.lstTemplateList.FormattingEnabled = true;
			this.lstTemplateList.Location = new System.Drawing.Point(18, 49);
			this.lstTemplateList.Name = "lstTemplateList";
			this.lstTemplateList.Size = new System.Drawing.Size(275, 214);
			this.lstTemplateList.Sorted = true;
			this.lstTemplateList.TabIndex = 1;
			this.lstTemplateList.ThreeDCheckBoxes = true;
			this.lstTemplateList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstTemplateList_ItemCheck);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(218, 272);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnCreate
			// 
			this.btnCreate.Location = new System.Drawing.Point(131, 272);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.Size = new System.Drawing.Size(75, 23);
			this.btnCreate.TabIndex = 2;
			this.btnCreate.Text = "Create";
			this.btnCreate.UseVisualStyleBackColor = true;
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// TemplateSelectionView
			// 
			this.AcceptButton = this.btnCreate;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(314, 307);
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lstTemplateList);
			this.Controls.Add(this.cbTemplateSets);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TemplateSelectionView";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "TemplateSelection";
			this.TopMost = true;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox cbTemplateSets;
		private System.Windows.Forms.CheckedListBox lstTemplateList;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnCreate;
	}
}