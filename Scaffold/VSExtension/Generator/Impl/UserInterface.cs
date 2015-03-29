using System.Collections.Generic;
using System.Linq;
using Flywheel.UI;

namespace Flywheel.Generator
{
	public class UserInterface : IUserInterface
	{
		private readonly IProjectConfiguration _projectConfiguration;
		private readonly ITemplateStatusController _statusController;
		private readonly ITemplateSelectionController _selectionController;

		public UserInterface(ITemplateSelectionController selectionController, IProjectConfiguration projectConfiguration, ITemplateStatusController statusController)
		{
			
			_selectionController = selectionController;
			_projectConfiguration = projectConfiguration;
			_statusController = statusController;
		}

		#region IUserInterface Members

		public void DisplayResults(TemplateRunResult[] results)
		{
			_statusController.Run(results);
		}

		public IEnumerable<string> DisplayTemplateSelection(string projectDirectory, string modelName)
		{
			_selectionController.Run(modelName, projectDirectory);

			string set = _selectionController.TemplateSet;

			string[] templates = GetTemplatesFromTheSelectionController(projectDirectory, set);

			return templates;
		}

		#endregion

		private string[] GetTemplatesFromTheSelectionController(string projectDirectory, string set)
		{
			return _selectionController.Templates.Select(s => GetFullPathFilename(projectDirectory, set, s)).ToArray();
		}

		private string GetFullPathFilename(string projectDirectory, string set, string s)
		{
			return projectDirectory + _projectConfiguration.TemplateSetDirectory + "\\" + set + "\\" + s + ".tt";
		}
	}
}