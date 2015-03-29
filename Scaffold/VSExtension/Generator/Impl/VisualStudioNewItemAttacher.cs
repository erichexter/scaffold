using System;
using System.IO;
using EnvDTE;
using VSLangProj;

namespace Flywheel.Generator
{
	public class VisualStudioNewItemAttacher : IVisualStudioNewItemAttacher
	{
		private readonly DTE _dte;
		private readonly IFileSystem _fileSystem;

		public VisualStudioNewItemAttacher(DTE dte,IFileSystem fileSystem)
		{
			_dte = dte;
			_fileSystem = fileSystem;
		}

		#region IVisualStudioNewItemAttacher Members

		public bool AddFileToProject(string filename)
		{
		    return AddFileToProject(filename, prjBuildAction.prjBuildActionCompile);
		}

	    public bool AddFileToProject(string filename, prjBuildAction action)
	    {
            ProjectItem projectItem = null;
            try
            {
                var project = FindProjectThatContainsTheFile(filename);
                if (project != null)
                {
                    if (_dte.SourceControl.IsItemUnderSCC(project.FileName) && !_dte.SourceControl.IsItemCheckedOut(project.FileName))
                    {
                        _dte.SourceControl.CheckOutItem(project.FileName);

                    }
                    //.IsItemCheckedOut(project.FileName)
                    projectItem = project.ProjectItems.AddFromFile(filename);
                    projectItem.Properties.Item("BuildAction").Value = action;
                    if (projectItem != null && projectItem.Name.EndsWith(".tt"))
                    {
                        projectItem.Properties.Item("CustomTool").Value = "";
                    }
                }
            }
            catch (Exception)
            {


            }
            return projectItem != null;
        }

	    private Project FindProjectThatContainsTheFile(string filename)
		{
			foreach (Project project in _dte.Solution.Projects)
			{
				if (project.Kind.Equals("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") && ProjectContainsTheFile(filename, project))
				{
					return project;
				}
			}
			return null;
		}

		#endregion

		private bool ProjectContainsTheFile(string filename, Project project)
		{
			return filename.ToLowerInvariant().Contains(_fileSystem.GetDirectoryName(project.FileName).ToLowerInvariant());
		}
	}
}