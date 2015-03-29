using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Flywheel.VSHelpers
{
	public class CodeWindowSelector : ICodeWindowSelector
	{
		private readonly IVsMonitorSelection _selection;

		public CodeWindowSelector(IVsMonitorSelection selection)
		{
			_selection = selection;
		}

		public VsHierarchyItem GetSelectedDocument()
		{
			return VsHierarchyItem.SelectedDocument(_selection);
		}

		public CodeType GetSelectedClass()
		{
			VsHierarchyItem vsHierDocItem = GetSelectedDocument();
			IVsTextView currentSelectedTextView = GetCurrentSelectedTextView(_selection);
			IVsTextLines textLinesFromView = GetTextLinesFromView(currentSelectedTextView);
			TextPoint currentTextPoint = GetCurrentTextPoint(currentSelectedTextView, textLinesFromView);
			return GetCurrentCodeElement(currentTextPoint, vsHierDocItem, vsCMElement.vsCMElementClass) as CodeType;
		}

		private bool VerifyInClass(VsHierarchyItem vsHierDocItem, CodeElement codeElement)
		{
			//string fullName = codeElement.FullName;
			//if (codeElement)
			//{

			//}
			//string name = codeElement.Name;
			//string _containingClassFullName = fullName.Substring(0, (fullName.Length - name.Length) - 1);
			//var classCodeElement =
			//    vsHierDocItem.Project().CodeModel.CodeTypeFromFullName(_containingClassFullName) as CodeElement;
			return IsInheritedFromObject(codeElement);
		}

		private bool IsInheritedFromObject(CodeElement classCodeElement)
		{
			if (classCodeElement.IsCodeType)
			{
				var type = classCodeElement as CodeType;
				foreach (CodeElement element in type.Bases)
				{
					if (element.FullName.StartsWith("System.Object"))
					{
						return true;
					}
				}
				foreach (CodeElement element2 in type.Bases)
				{
					if (IsInheritedFromObject(element2))
					{
						return true;
					}
				}
			}
			return false;
		}

		private CodeElement GetCurrentCodeElement(TextPoint textPoint, VsHierarchyItem vsHierDocItem, vsCMElement scope)
		{
			var fileCodeModel = vsHierDocItem.ProjectItem().FileCodeModel as FileCodeModel2;
			CodeElement element = null;
			try
			{
				element = fileCodeModel.CodeElementFromPoint(textPoint, scope);
			}
			catch
			{
			}
			return element;
		}


		public IVsCodeWindow SelectedCodeWindow(IVsMonitorSelection service)
		{
			object obj2;
			object obj3;
			if ((((service != null) && NativeMethods.Succeeded(service.GetCurrentElementValue(2, out obj2))) &&
			     ((obj2 != null) && NativeMethods.Succeeded(service.GetCurrentElementValue(1, out obj3)))) && (obj3 != null))
			{
				var frame = obj2 as IVsWindowFrame;
				var frame2 = obj3 as IVsWindowFrame;
				if ((frame != null) && (frame == frame2))
				{
					object obj4;
					frame2.GetProperty(-3001, out obj4);
					try
					{
						return (IVsCodeWindow) obj4;
					}
					catch
					{
					}
				}
			}
			return null;
		}

		private IVsTextView GetCurrentSelectedTextView(IVsMonitorSelection service)
		{
			IVsCodeWindow window = SelectedCodeWindow(service);

			IVsTextView ppView = null;
			window.GetPrimaryView(out ppView);
			return ppView;
		}

		private IVsTextLines GetTextLinesFromView(IVsTextView vsTextView)
		{
			IVsTextLines ppBuffer = null;
			vsTextView.GetBuffer(out ppBuffer);
			return ppBuffer;
		}

		private TextPoint GetCurrentTextPoint(IVsTextView vsTextView, IVsTextLines vsTextLines)
		{
			int num;
			int num2;
			int num3;
			int num4;
			int num5;
			vsTextView.GetCaretPos(out num, out num2);
			vsTextView.GetNearestPosition(num, num2, out num3, out num4);
			vsTextView.GetLineAndColumn(num3, out num, out num5);
			object ppTextPoint = null;
			vsTextLines.CreateTextPoint(num, num5, out ppTextPoint);
			return (ppTextPoint as TextPoint);
		}
	}
}