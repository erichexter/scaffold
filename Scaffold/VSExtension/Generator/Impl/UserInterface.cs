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

		public ScaffoldSelection DisplayTemplateSelection(string projectDirectory, string modelName)
		{
			_selectionController.Run(modelName, projectDirectory);

			string set = _selectionController.TemplateSet;

			string[] templates = GetTemplatesFromTheSelectionController(projectDirectory, set);

		    var result = new ScaffoldSelection()
		    {
                Name=set,
                Templates=templates
		    };
			return result;
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

    public class ScaffoldSelection
    {
        public string[] Templates { get; set; }
        public string Name { get; set; }
    }
}