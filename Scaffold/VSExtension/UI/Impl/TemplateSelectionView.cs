using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Flywheel.UI
{
	public partial class TemplateSelectionView : Form, ITemplateSelectionView
	{
		public TemplateSelectionView()
		{
			InitializeComponent();
		}

		#region ITemplateSelectionView Members

		public Action TemplateSetSelected { get; set; }

		public string TemplateSet { get; set; }

		public Action TemplatesSelected { get; set; }

		public Action CancelDialog { get; set; }

		public string[] Templates { get
			{
			return lstTemplateList.CheckedItems.Cast<string>().ToArray();
			} 
		}
		public Action TemplateChecked{ get; set;}

		void ITemplateSelectionView.ShowDialog()
		{
			ShowDialog();
		}

		public void BindTemplateSets(IEnumerable<string> templateSets)
		{
			//cbTemplateSets.BeginUpdate();
			cbTemplateSets.DataSource = templateSets;
			//cbTemplateSets.EndUpdate();
		}

		public void BindTemplates(IEnumerable<string> templatesForASet)
		{
			//lstTemplateList.BeginUpdate();
			lstTemplateList.DataSource = templatesForASet;
			//lstTemplateList.EndUpdate();
			for (int i = 0; i < lstTemplateList.Items.Count; i++)
			{
				lstTemplateList.SetItemChecked(i, true);
			}
			
			lstTemplateList.Refresh();
		}

		public void SetTitle(string name)
		{
			Text = name;
		}

		public void EnableEnter()
		{
			btnCreate.Enabled = true;
		}

		public void DisableEnter()
		{
			btnCreate.Enabled = false;
		}

		#endregion

		private void cbTemplateSets_SelectedIndexChanged(object sender, EventArgs e)
		{
			TemplateSet = cbTemplateSets.Text;
			TemplateSetSelected.Invoke();
		}

		private void btnCreate_Click(object sender, EventArgs e)
		{
			
			TemplatesSelected.Invoke();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			CancelDialog.Invoke();
		}

		private void lstTemplateList_ItemCheck(object sender, MouseEventArgs e)
		{
			BeginInvoke(TemplateChecked);
			//TemplateChecked.Invoke();
			//TemplateChecked.BeginInvoke(null, null);
		}
	}
}