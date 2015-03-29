using System.Collections.Generic;
using EnvDTE;

namespace Flywheel.UI.Impl
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetAll();
    }
}