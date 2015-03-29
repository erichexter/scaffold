using System;
using System.Linq;
using System.Windows.Forms;
using Flywheel.Generator;

namespace Flywheel.UI
{
	public partial class TemplateStatusView : Form, ITemplateStatusView
	{
		private readonly IFileSystem _fileSystem;

		public TemplateStatusView(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem;
			InitializeComponent();
		}

		#region ITemplateStatusView Members

		public void UpdateResults(TemplateRunResult[] results)
		{
			dgItems.DataSource = results.Select(result => new
			                                              	{
			                                              		result.Result,
			                                              		Template = _fileSystem.GetFileName(result.TemplateFilename),
			                                              		File = result.OutputFilename,
			                                              		Errors = string.Join("\r\n",
			                                              		                     result.Errors.Select(error => error.Description)
			                                              		                     	.ToArray())
			                                              	}).ToArray()
				;
		}

		void ITemplateStatusView.ShowDialog()
		{
			ShowDialog();
		}

		public Action CancelClicked { get; set; }

		#endregion

		public void SetTitle(string name)
		{
			Text = name;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			CancelClicked.Invoke();
		}
	}
}