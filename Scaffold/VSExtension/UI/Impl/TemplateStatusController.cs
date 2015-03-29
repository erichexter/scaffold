using System.Linq;

namespace Flywheel.Generator
{
	public class TemplateStatusController : ITemplateStatusController
	{
		private readonly IMessageBox _messageBox;
		private readonly ITemplateStatusView _view;

		public TemplateStatusController(IMessageBox messageBox,ITemplateStatusView view)
		{
			_messageBox = messageBox;
			_view = view;
			_view.CancelClicked = CancelClicked;
		}

		private void CancelClicked()
		{
			_view.Close();
		}

		public void Run(TemplateRunResult[] results)
		{
			var errorCount = results.SelectMany(result => result.Errors).Count();
			if(errorCount<=0)
				_view.SetTitle("Generation Results");
			else
			{
				_view.SetTitle(string.Format("{0} - {1} Errors","Generation Results",errorCount));
			}
			_view.UpdateResults(results);
			_view.ShowDialog();
		}
		public void Complete()
		{
			_view.Close();
		}
		private string GetFilesCreated(TemplateRunResult[] results)
		{
			return string.Join("\r\n",
			                   results.Select(result => "Created file: " + result.OutputFilename).ToArray());
		}

		private string GetErrorDescriptions(TemplateRunResult[] results)
		{
			return string.Join("\r\n",
			                   results.SelectMany(result => result.Errors.Select(error => error.Description)).
			                   	ToArray());
		}

	}
}