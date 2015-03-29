using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EnvDTE;

namespace Flywheel.UI.Impl
{
    public partial class InstallTemplatesView : Form, IInstallTemplatesView
    {
        public InstallTemplatesView()
        {
            InitializeComponent();
        }

        #region IInstallTemplatesView Members

        public void BindProjects(IEnumerable<string> projects)
        {
            ddlProjects.DataSource = projects;
        }

        public void BindTemplateSets(IEnumerable<string> templateSets)
        {
            lbTemplateSets.DataSource = templateSets;
            for (int i = 0; i < lbTemplateSets.Items.Count; i++)
            {
                lbTemplateSets.SetItemChecked(i, true);
            }
            lbTemplateSets.Refresh();
        }

        public Action CancelClicked { get; set; }

        public Action AcceptClicked { get; set; }

        public string GetSelectedProject()
        {
            return ddlProjects.SelectedItem.ToString();
        }

        public string[] GetSelectedTemplateSets()
        {
            return lbTemplateSets.CheckedItems.Cast<string>().ToArray();
        }

        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelClicked.Invoke();
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            AcceptClicked.Invoke();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        void IInstallTemplatesView.Show()
        {
            this.ShowDialog();
        }
    }
}