using System;
using System.Collections.Generic;
using EnvDTE;

namespace Flywheel.UI.Impl
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DTE _dte;

        public ProjectRepository(DTE dte)
        {
            _dte = dte;
        }

        public IEnumerable<Project> GetAll()
        {
            foreach(Project project in _dte.Solution.Projects)
            {
                if (project.Kind.Equals("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"))
                  yield return project;
            }
        }
    }
}