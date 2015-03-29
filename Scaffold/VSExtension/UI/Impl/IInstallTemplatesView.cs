using System;
using System.Collections.Generic;
using EnvDTE;

namespace Flywheel.UI.Impl
{
    public interface IInstallTemplatesView
    {
        void BindProjects(IEnumerable<string> projects);
        void BindTemplateSets(IEnumerable<string> templateSets);
        void Show();
        Action CancelClicked { get; set; }
        Action AcceptClicked { get; set; }
        void Close();
        string GetSelectedProject();
        string[] GetSelectedTemplateSets();
    }
}