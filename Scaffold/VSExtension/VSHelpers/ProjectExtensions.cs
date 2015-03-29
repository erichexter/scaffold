using System.Collections.Generic;
using EnvDTE;

namespace Flywheel.VSHelpers
{
    public static class ProjectExtensions
    {
        public static bool IsACodeProject(this Project value)
        {
            return value.Kind.Equals("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}");
        }

        public static IEnumerable<Project> GetCodeProjects(this Projects projects)
        {
            foreach (Project project in projects)
            {
                if (project.IsACodeProject())
                {
                    yield return project;
                }
            }
        }
    }
}