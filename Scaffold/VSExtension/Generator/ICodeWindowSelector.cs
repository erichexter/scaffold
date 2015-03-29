using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Flywheel.VSHelpers
{
	public interface ICodeWindowSelector
	{
		VsHierarchyItem GetSelectedDocument();
		CodeType GetSelectedClass();
		IVsCodeWindow SelectedCodeWindow(IVsMonitorSelection service);
	}
}