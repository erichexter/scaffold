using System.Collections.Generic;
using System.Linq;
using Flywheel.Generator;

namespace Flywheel.UI
{
	public class TemplateSelectionController : ITemplateSelectionController
	{
		private readonly ITemplateRepository _templateRepository;
		private readonly IMessageBox _messagebox;
		private readonly ITemplateSelectionView _view;
		private string _projectDirectory;

		public TemplateSelectionController(ITemplateSelectionView view, ITemplateRepository templateRepository, IMessageBox messagebox)
		{
			_view = view;
			_templateRepository = templateRepository;
			_messagebox = messagebox;
			_view.TemplateSetSelected = TemplateSetSelected;
			_view.TemplatesSelected = EnterSelected;
			_view.CancelDialog = EscapeSelected;
			_view.TemplateChecked = TemplateChecked;
		}

		#region ITemplateSelectionController Members

		public string[] Templates { get; set; }
		public string TemplateSet { get; set; }

		public void Run(string modelName, string projectDirectory)
		{
			_projectDirectory = projectDirectory;
			_view.SetTitle("Generate from Model: " + modelName);
			_view.BindTemplateSets(_templateRepository.GetTemplateSets(projectDirectory));
			_view.ShowDialog();
		}

		public void TemplateChecked()
		{
			if(_view.Templates.Length>0)
			{
				_view.EnableEnter();
			}
			else
			{
				_view.DisableEnter();
			}

		}

		public void TemplateSetSelected()
		{
			TemplateSet = _view.TemplateSet;

			IEnumerable<string> templatesForASet = _templateRepository.GetTemplatesForASet(_projectDirectory, _view.TemplateSet);

			_view.BindTemplates(templatesForASet);
			
			if(templatesForASet.Count()>0)
			{
				_view.EnableEnter();
			}
			
		}

		public void EnterSelected()
		{
			if(_view.Templates.Length<=0)
			{
				_messagebox.ShowError("A Template must be checked in order to proceed.");
			}
			else
			{
				Templates = _view.Templates;
				_view.Close();				
			}
		}

		public void EscapeSelected()
		{
			Templates = new string[0];
			_view.Close();
		}

		#endregion
	}
}