using System;

namespace Flywheel.Generator
{
	public interface ITemplateStatusView
	{
		void UpdateResults(TemplateRunResult[] results);
		void ShowDialog();
		void Close();
		Action CancelClicked { get; set; }
		void SetTitle(string title);
	}
}