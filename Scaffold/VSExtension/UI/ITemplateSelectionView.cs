using System;
using System.Collections.Generic;

namespace Flywheel.UI
{
	public interface ITemplateSelectionView
	{
		Action TemplateSetSelected { get; set; }
		string TemplateSet { get; set; }
		Action TemplatesSelected { get; set; }
		Action CancelDialog { get; set; }
		string[] Templates { get;  }
		Action TemplateChecked { get; set; }
		void ShowDialog();
		void BindTemplateSets(IEnumerable<string> templateSets);
		void BindTemplates(IEnumerable<string> templatesForASet);
		void Close();
		void SetTitle(string name);
		void EnableEnter();
		void DisableEnter();
	}
}