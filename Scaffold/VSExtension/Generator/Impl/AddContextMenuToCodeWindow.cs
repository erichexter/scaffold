using System.IO;
using EnvDTE;

namespace Flywheel.VSHelpers
{
	public class ShouldContextMenu : IShouldContextMenu
	{
		private readonly CodeWindowSelector _codeWindow;
		private readonly SolutionExplorerSelector _solutionExplorer;


		public ShouldContextMenu(CodeWindowSelector codeWindow, SolutionExplorerSelector solutionExplorer)
		{
			_codeWindow = codeWindow;
			_solutionExplorer = solutionExplorer;
		}

		public bool ShowCommandInCodeWindow()
		{
			VsHierarchyItem vsHierDocItem = _codeWindow.GetSelectedDocument();

			string CodeFileExtension = Path.GetExtension(vsHierDocItem.FullPath());
			if ((CodeFileExtension != ".cs") && (CodeFileExtension != ".vb"))
			{
				return false;
			}

			return _codeWindow.GetSelectedClass() != null;
		}

		public bool ShowCommandInSolutionExplorer()
		{
			VsHierarchyItem vsHierDocItem = _solutionExplorer.GetItemSelectedInSolutionExplorer();
			ProjectItem projectItem = vsHierDocItem.ProjectItem();
			//var projectItem = _solutionExplorer.GetItemFromSolutionExplorer();
			if (projectItem.Name != null && Path.GetExtension(projectItem.get_FileNames(0)) == ".cs")
			{
				return true;
			}

			return projectItem != null;
		}
	}
}