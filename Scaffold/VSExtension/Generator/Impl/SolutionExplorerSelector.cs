using System;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using Constants=EnvDTE.Constants;

namespace Flywheel.VSHelpers
{
	public class SolutionExplorerSelector
	{
		private readonly DTE _dte;
		private readonly IVsMonitorSelection _selectionService;

		public SolutionExplorerSelector(IVsMonitorSelection service, DTE dte)
		{
			_selectionService = service;
			_dte = dte;
		}

		public ProjectItem GetItemFromSolutionExplorer()
		{
			var uih = (UIHierarchy) _dte.Windows.Item(Constants.vsWindowKindSolutionExplorer).Object;

			foreach (UIHierarchy selectedItem in uih.SelectedItems as UIHierarchy[])
			{
				var item = (ProjectItem) selectedItem;
				return item;
			}
			return null;
		}

		public VsHierarchyItem GetItemSelectedInSolutionExplorer()
		{
			VsHierarchyItem item = null;
			uint maxValue = uint.MaxValue;
			IVsHierarchy hier = null;
			IVsMultiItemSelect ppMIS = null;

			if (_selectionService != null)
			{
				IntPtr zero = IntPtr.Zero;
				IntPtr ppSC = IntPtr.Zero;
				if (_selectionService.GetCurrentSelection(out zero, out maxValue, out ppMIS, out ppSC) < 0)
				{
					return null;
				}
				if (ppSC != IntPtr.Zero)
				{
					Marshal.Release(ppSC);
				}
				if (zero != IntPtr.Zero)
				{
					hier = Marshal.GetObjectForIUnknown(zero) as IVsHierarchy;
					Marshal.Release(zero);
				}
				if ((uint.MaxValue == maxValue) || (ppMIS != null))
				{
					return null;
				}
				if (0xfffffffd != maxValue)
				{
					item = new VsHierarchyItem(maxValue, hier);
				}
			}
			return item;
		}

		public CodeType GetSelectedClass()
		{
			//var pitem = GetItemFromSolutionExplorer();
			VsHierarchyItem item = GetItemSelectedInSolutionExplorer();
			ProjectItem pI = item.ProjectItem();

			foreach (CodeElement code in pI.FileCodeModel.CodeElements)
			{
				CodeType codeType = GetFirstClassDeclaration(code);
				if (codeType != null)
				{
					return codeType;
				}
			}
			return null;
		}

		private CodeType GetFirstClassDeclaration(CodeElement code)
		{
			if (code.IsCodeType && ((CodeType) code).Kind == vsCMElement.vsCMElementClass)
			{
				var codeClass = code as CodeClass;
				if (!codeClass.IsAbstract)
				{
					return code as CodeType;
				}
			}
			foreach (CodeElement codeElement in code.Children)
			{
				CodeType cType = GetFirstClassDeclaration(codeElement);
				if (cType != null)
				{
					return cType;
				}
			}
			return null;
		}
	}
}