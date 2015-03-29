using System;
using System.IO;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using VSLangProj;
using IServiceProvider=Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace Flywheel.VSHelpers
{
	public class VsHierarchyItem
	{
		#region Delegates

		public delegate int ProcessItemDelegate(VsHierarchyItem item, object callerObject, out object newCallerObject);

		#endregion

		private readonly IVsHierarchy _hier;
		private readonly uint _vsitemid;
		private int _isFile;
		private int _isFolder;
		private int _isReferencesVFolder;
		private ServiceProvider _serviceProvider;

		public VsHierarchyItem(IVsHierarchy hier)
		{
			_isFile = -1;
			_isFolder = -1;
			_isReferencesVFolder = -1;
			_vsitemid = 0xfffffffe;
			_hier = hier;
		}

		public VsHierarchyItem(uint id, IVsHierarchy hier)
		{
			_isFile = -1;
			_isFolder = -1;
			_isReferencesVFolder = -1;
			_vsitemid = id;
			_hier = hier;
		}

		public IVsHierarchy Hierarchy
		{
			get { return _hier; }
		}

		public References References
		{
			get
			{
				VSProject vSProject = VSProject;
				if (vSProject != null)
				{
					return vSProject.References;
				}
				return null;
			}
		}

		public uint VsItemID
		{
			get { return _vsitemid; }
		}

		public VSProject VSProject
		{
			get
			{
				if (IsRootNode())
				{
					try
					{
						var project = ExtObject() as Project;
						if (project != null)
						{
							return (project.Object as VSProject);
						}
					}
					catch
					{
					}
				}
				return null;
			}
		}

		public object BrowseObject()
		{
			return GetPropHelper(__VSHPROPID.VSHPROPID_BrowseObject);
		}

		public prjBuildAction BuildAction()
		{
			EnvDTE.Properties properties = Properties();
			if (properties != null)
			{
				Property property = properties.Item("BuildAction");
				if (property != null)
				{
					object obj2 = property.Value;
					if (obj2 != null)
					{
						return (prjBuildAction) obj2;
					}
				}
			}
			return prjBuildAction.prjBuildActionNone;
		}

		public CodeModel CodeModel()
		{
			CodeModel codeModel = null;
			Project project = Project();
			if (project != null)
			{
				codeModel = project.CodeModel;
			}
			return codeModel;
		}

		public VsHierarchyItem CreateChildFolder(string folderName)
		{
			if (!string.IsNullOrEmpty(folderName) && IsFolder())
			{
				ProjectItems items = ProjectItems();
				if (items != null)
				{
					items.AddFolder(folderName, null);
					return GetChildFolder(folderName);
				}
			}
			return null;
		}

		//public IVsPersistDocData CreateDocumentData()
		//{
		//    if (IsFile())
		//    {
		//        string str = FullPath();
		//        if (!string.IsNullOrEmpty(str))
		//        {
		//            var data = Util.CreateSitedInstance<IVsPersistDocData>(typeof(VsTextBufferClass).GUID);
		//            if ((data != null) && NativeMethods.Succeeded(data.LoadDocData(str)))
		//            {
		//                return data;
		//            }
		//        }
		//    }
		//    return null;
		//}

		public static VsHierarchyItem CreateFromMoniker(string moniker, IVsHierarchy hier)
		{
			VsHierarchyItem item = null;
			if (!string.IsNullOrEmpty(moniker) && (hier != null))
			{
				var project = hier as IVsProject;
				if (project != null)
				{
					int pfFound = 0;
					uint maxValue = uint.MaxValue;
					var pdwPriority = new VSDOCUMENTPRIORITY[1];
					if ((NativeMethods.Succeeded(project.IsDocumentInProject(moniker, out pfFound, pdwPriority, out maxValue)) &&
					     (pfFound != 0)) && (maxValue != uint.MaxValue))
					{
						item = new VsHierarchyItem(maxValue, hier);
					}
				}
			}
			return item;
		}

		//public static VsHierarchyItem CreateFromMonikerOnly(string moniker)
		//{
		//    VsHierarchyItem item = null;
		//    if (!string.IsNullOrEmpty(moniker))
		//    {
		//        var service = Util.GetService<IVsUIShellOpenDocument>();
		//        if (service != null)
		//        {
		//            int num;
		//            IServiceProvider provider;
		//            IVsUIHierarchy ppUIH = null;
		//            uint pitemid = 0;
		//            if (
		//                (NativeMethods.Succeeded(service.IsDocumentInAProject(moniker, out ppUIH, out pitemid, out provider, out num)) &&
		//                 (ppUIH != null)) && (pitemid != 0))
		//            {
		//                item = new VsHierarchyItem(pitemid, ppUIH);
		//            }
		//        }
		//    }
		//    return item;
		//}

		public string DefaultNamespace()
		{
			return (GetPropHelper(__VSHPROPID.VSHPROPID_DefaultNamespace) as string);
		}

		public IVsWindowFrame DocumentFrame()
		{
			IVsWindowFrame ppWindowFrame = null;
			var pHierCaller = _hier as IVsUIHierarchy;
			if ((pHierCaller != null) && IsFile())
			{
				var service = GetService<IVsUIShellOpenDocument>();
				if (service != null)
				{
					IVsUIHierarchy hierarchy2;
					int num2;
					Guid empty = Guid.Empty;
					uint grfIDO = 0;
					var pitemidOpen = new uint[1];
					service.IsDocumentOpen(pHierCaller, _vsitemid, FullPath(), ref empty, grfIDO, out hierarchy2, pitemidOpen,
					                       out ppWindowFrame, out num2);
				}
			}
			return ppWindowFrame;
		}

		public DTE DTE()
		{
			DTE dTE = null;
			VsHierarchyItem item = Root();
			if (item != null)
			{
				ProjectItems items = item.ProjectItems();
				if (items != null)
				{
					dTE = items.DTE;
				}
			}
			return dTE;
		}

		public VsHierarchyItem EnsureChildFolder(string folderName)
		{
			VsHierarchyItem childFolder = null;
			if (!string.IsNullOrEmpty(folderName) && IsFolder())
			{
				childFolder = GetChildFolder(folderName);
				if (childFolder == null)
				{
					childFolder = CreateChildFolder(folderName);
				}
			}
			return childFolder;
		}

		public object ExtObject()
		{
			return GetPropHelper(__VSHPROPID.VSHPROPID_ExtObject);
		}

		public VsHierarchyItem FirstChild(bool fVisible)
		{
			uint id = FirstChildId(fVisible);
			if (id != uint.MaxValue)
			{
				return new VsHierarchyItem(id, _hier);
			}
			return null;
		}

		public uint FirstChildId(bool fVisible)
		{
			object propHelper;
			if (fVisible)
			{
				propHelper = GetPropHelper(__VSHPROPID.VSHPROPID_FirstVisibleChild);
			}
			else
			{
				propHelper = GetPropHelper(__VSHPROPID.VSHPROPID_FirstChild);
			}
			if (propHelper is int)
			{
				return (uint) ((int) propHelper);
			}
			if (propHelper is uint)
			{
				return (uint) propHelper;
			}
			return uint.MaxValue;
		}

		public string FullPath()
		{
			string str = "";
			try
			{
				object obj2 = ExtObject();
				if (obj2 is Project)
				{
					var project = obj2 as Project;
					if (project != null)
					{
						str = project.Properties.Item("FullPath").Value as string;
					}
					return str;
				}
				if (obj2 is ProjectItem)
				{
					var item = obj2 as ProjectItem;
					if (item == null)
					{
						return str;
					}
					str = item.Properties.Item("FullPath").Value as string;
				}
			}
			catch
			{
			}
			return str;
		}

		public VsHierarchyItem GetChildFolder(string folderName)
		{
			if (!string.IsNullOrEmpty(folderName) && IsFolder())
			{
				bool fVisible = false;
				for (VsHierarchyItem item = FirstChild(fVisible); item != null; item = item.NextSibling(fVisible))
				{
					if (item.IsFolder())
					{
						string str = item.Name();
						if ((!string.IsNullOrEmpty(str) && (str.Length == folderName.Length)) &&
						    (string.Compare(str, folderName, StringComparison.OrdinalIgnoreCase) == 0))
						{
							return item;
						}
					}
				}
			}
			return null;
		}

		public VsHierarchyItem GetChildOfName(string itemName)
		{
			for (VsHierarchyItem item = FirstChild(false); item != null; item = item.NextSibling(false))
			{
				if (Util.EqualsNoCase(item.Name(), itemName))
				{
					return item;
				}
			}
			return null;
		}

		//public string GetDocumentText()
		//{
		//    string str = null;
		//    IVsPersistDocData data = null;
		//    try
		//    {
		//        int num;
		//        IVsTextLines runningDocumentTextBuffer = GetRunningDocumentTextBuffer();
		//        if (runningDocumentTextBuffer == null)
		//        {
		//            data = CreateDocumentData();
		//            runningDocumentTextBuffer = data as IVsTextLines;
		//        }
		//        if (runningDocumentTextBuffer == null)
		//        {
		//            return str;
		//        }
		//        var stream = runningDocumentTextBuffer as IVsTextStream;
		//        if ((stream == null) || !NativeMethods.Succeeded(stream.GetSize(out num)))
		//        {
		//            return str;
		//        }
		//        if (num > 0)
		//        {
		//            IntPtr pszDest = Marshal.AllocCoTaskMem((num + 1) * 2);
		//            try
		//            {
		//                if (NativeMethods.Succeeded(stream.GetStream(0, num, pszDest)))
		//                {
		//                    str = Marshal.PtrToStringUni(pszDest);
		//                }
		//                return str;
		//            }
		//            finally
		//            {
		//                Marshal.FreeCoTaskMem(pszDest);
		//            }
		//        }
		//        str = string.Empty;
		//    }
		//    finally
		//    {
		//        if (data != null)
		//        {
		//            data.Close();
		//        }
		//    }
		//    return str;
		//}

		public Guid GetGuidPropHelper(__VSHPROPID propid)
		{
			Guid empty;
			try
			{
				_hier.GetGuidProperty(_vsitemid, (int) propid, out empty);
			}
			catch (Exception)
			{
				empty = Guid.Empty;
			}
			return empty;
		}

		public Project GetProjectByName(string projectName)
		{
			Solution solution = Solution();
			if (solution != null)
			{
				foreach (Project project in solution.Projects)
				{
					if (Util.CompareNoCase(project.Name, projectName) == 0)
					{
						return project;
					}
				}
			}
			return null;
		}

		private object GetPropHelper(__VSHPROPID propid)
		{
			return GetPropHelper(_vsitemid, (int) propid);
		}

		private object GetPropHelper(__VSHPROPID2 propid)
		{
			return GetPropHelper(_vsitemid, (int) propid);
		}

		private object GetPropHelper(uint itemid, int propid)
		{
			try
			{
				object pvar = null;
				if (_hier != null)
				{
					_hier.GetProperty(itemid, propid, out pvar);
				}
				return pvar;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public IntPtr GetRunningDocData()
		{
			IntPtr zero = IntPtr.Zero;
			if (IsFile())
			{
				string str = FullPath();
				if (!string.IsNullOrEmpty(str))
				{
					var service = GetService<IVsRunningDocumentTable>();
					if (service != null)
					{
						uint num;
						IVsHierarchy hierarchy;
						uint num2;
						_VSRDTFLAGS _vsrdtflags = _VSRDTFLAGS.RDT_NoLock;
						service.FindAndLockDocument((uint) _vsrdtflags, str, out hierarchy, out num, out zero, out num2);
					}
				}
			}
			return zero;
		}

		public IVsPersistDocData GetRunningDocumentData()
		{
			IVsPersistDocData objectForIUnknown = null;
			IntPtr zero = IntPtr.Zero;
			try
			{
				zero = GetRunningDocData();
				if (zero != IntPtr.Zero)
				{
					objectForIUnknown = Marshal.GetObjectForIUnknown(zero) as IVsPersistDocData;
				}
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Marshal.Release(zero);
				}
			}
			return objectForIUnknown;
		}

		public IVsTextLines GetRunningDocumentTextBuffer()
		{
			IVsTextLines ppTextBuffer = null;
			IVsPersistDocData runningDocumentData = GetRunningDocumentData();
			if (runningDocumentData != null)
			{
				ppTextBuffer = runningDocumentData as IVsTextLines;
				if (ppTextBuffer == null)
				{
					var provider = runningDocumentData as IVsTextBufferProvider;
					if (provider != null)
					{
						provider.GetTextBuffer(out ppTextBuffer);
					}
				}
			}
			return ppTextBuffer;
		}

		public InterfaceType GetService<InterfaceType>() where InterfaceType : class
		{
			InterfaceType service = default(InterfaceType);
			try
			{
				if (_serviceProvider == null)
				{
					IServiceProvider sp = Site();
					if (sp != null)
					{
						_serviceProvider = new ServiceProvider(sp);
					}
				}
				if (_serviceProvider != null)
				{
					service = _serviceProvider.GetService(typeof (InterfaceType)) as InterfaceType;
				}
			}
			catch
			{
			}
			return service;
		}

		public bool HasAncestor(VsHierarchyItem item)
		{
			for (VsHierarchyItem item2 = Parent(); item2 != null; item2 = item2.Parent())
			{
				if (item2.VsItemID == item.VsItemID)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsBuildActionCompile()
		{
			return (BuildAction() == prjBuildAction.prjBuildActionCompile);
		}

		public bool IsDirty()
		{
			ProjectItem item = ProjectItem();
			if (item != null)
			{
				Document document = item.Document;
				if ((document != null) && !document.Saved)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsExpandable()
		{
			object propHelper = GetPropHelper(__VSHPROPID.VSHPROPID_Expandable);
			if (propHelper is bool)
			{
				return (bool) propHelper;
			}
			return ((propHelper is int) && (((int) propHelper) != 0));
		}

		public bool IsFile()
		{
			if (_isFile == -1)
			{
				Guid guidPropHelper = GetGuidPropHelper(__VSHPROPID.VSHPROPID_TypeGuid);
				_isFile = (guidPropHelper.CompareTo(VSConstants.GUID_ItemType_PhysicalFile) == 0) ? 1 : 0;
			}
			return (_isFile == 1);
		}

		public bool IsFolder()
		{
			if (_isFolder == -1)
			{
				if (IsRootNode())
				{
					_isFolder = 1;
				}
				else
				{
					Guid guidPropHelper = GetGuidPropHelper(__VSHPROPID.VSHPROPID_TypeGuid);
					_isFolder = (guidPropHelper.CompareTo(VSConstants.GUID_ItemType_PhysicalFolder) == 0) ? 1 : 0;
				}
			}
			return (_isFolder == 1);
		}

		public bool IsInStartupProject()
		{
			Project project = Project();
			if (project != null)
			{
				string uniqueName = project.UniqueName;
				if (!string.IsNullOrEmpty(uniqueName))
				{
					string[] strArray = StartupProjects();
					if (strArray != null)
					{
						foreach (string str2 in strArray)
						{
							if (str2 == uniqueName)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool IsLinkFile()
		{
			object propHelper = GetPropHelper(__VSHPROPID2.VSHPROPID_IsLinkFile);
			if (propHelper is bool)
			{
				return (bool) propHelper;
			}
			return ((propHelper is int) && (((int) propHelper) != 0));
		}

		public bool IsMemberItem()
		{
			object propHelper = GetPropHelper(__VSHPROPID.VSHPROPID_IsNonMemberItem);
			if ((propHelper != null) && (propHelper is bool))
			{
				return !((bool) propHelper);
			}
			return true;
		}

		public bool IsReferencesNode()
		{
			if (_isReferencesVFolder == -1)
			{
				_isReferencesVFolder = 0;
				if (ParentId() == 0xfffffffe)
				{
					string str = Name();
					if (((!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty("References")) &&
					     ((str.Length == "References".Length) &&
					      (string.Compare(str, "References", StringComparison.OrdinalIgnoreCase) == 0))) &&
					    (GetGuidPropHelper(__VSHPROPID.VSHPROPID_TypeGuid).CompareTo(VSConstants.GUID_ItemType_VirtualFolder) == 0))
					{
						_isReferencesVFolder = 1;
					}
				}
			}
			return (_isReferencesVFolder == 1);
		}

		public bool IsRootNode()
		{
			return (_vsitemid == 0xfffffffe);
		}

		public string Name()
		{
			object propHelper = GetPropHelper(__VSHPROPID.VSHPROPID_ProjectName);
			if (propHelper is string)
			{
				return (string) propHelper;
			}
			return string.Empty;
		}

		public VsHierarchyItem NextSibling(bool fVisible)
		{
			uint id = NextSiblingId(fVisible);
			if (id != uint.MaxValue)
			{
				return new VsHierarchyItem(id, _hier);
			}
			return null;
		}

		public uint NextSiblingId(bool fVisible)
		{
			object propHelper;
			if (fVisible)
			{
				propHelper = GetPropHelper(__VSHPROPID.VSHPROPID_NextVisibleSibling);
			}
			else
			{
				propHelper = GetPropHelper(__VSHPROPID.VSHPROPID_NextSibling);
			}
			if (propHelper is int)
			{
				return (uint) ((int) propHelper);
			}
			if (propHelper is uint)
			{
				return (uint) propHelper;
			}
			return uint.MaxValue;
		}

		public IVsWindowFrame Open()
		{
			return Open(VSConstants.LOGVIEWID_Primary);
		}

		public IVsWindowFrame Open(Guid logicalView)
		{
			IVsWindowFrame ppWindowFrame = null;
			if ((_hier != null) && IsFile())
			{
				IntPtr zero = IntPtr.Zero;
				string pszMkDocument = FullPath();
				try
				{
					zero = GetRunningDocData();
					var service = GetService<IVsUIShellOpenDocument>();
					if (service != null)
					{
						__VSOSEFLAGS __vsoseflags = __VSOSEFLAGS.OSE_OpenAsNewFile;
						__vsoseflags |= __VSOSEFLAGS.OSE_ChooseBestStdEditor;
						IServiceProvider psp = Site();
						IVsUIHierarchy pHier = UIHierarchy();
						if ((psp != null) && (pHier != null))
						{
							service.OpenStandardEditor((uint) __vsoseflags, pszMkDocument, ref logicalView, "%3", pHier, _vsitemid, zero, psp,
							                           out ppWindowFrame);
						}
					}
				}
				finally
				{
					if (zero != IntPtr.Zero)
					{
						Marshal.Release(zero);
					}
				}
				if (ppWindowFrame != null)
				{
					ppWindowFrame.Show();
				}
			}
			return ppWindowFrame;
		}

		public IVsWindowFrame OpenCode()
		{
			return Open(VSConstants.LOGVIEWID_Code);
		}

		public IVsWindowFrame OpenDesigner()
		{
			return Open(VSConstants.LOGVIEWID_Designer);
		}

		public VsHierarchyItem Parent()
		{
			uint id = ParentId();
			if (id != uint.MaxValue)
			{
				return new VsHierarchyItem(id, _hier);
			}
			return null;
		}

		public uint ParentId()
		{
			object propHelper = GetPropHelper(__VSHPROPID.VSHPROPID_LAST);
			if (propHelper is int)
			{
				return (uint) ((int) propHelper);
			}
			if (propHelper is uint)
			{
				return (uint) propHelper;
			}
			return uint.MaxValue;
		}

		public Project Project()
		{
			Project containingProject = null;
			VsHierarchyItem item = Root();
			if (item != null)
			{
				ProjectItems items = item.ProjectItems();
				if (items != null)
				{
					containingProject = items.ContainingProject;
				}
			}
			return containingProject;
		}

		public string ProjectDir()
		{
			object propHelper = GetPropHelper(__VSHPROPID.VSHPROPID_ProjectDir);
			if (propHelper is string)
			{
				return (string) propHelper;
			}
			return string.Empty;
		}

		public ProjectItem ProjectItem()
		{
			return (ExtObject() as ProjectItem);
		}

		public ProjectItems ProjectItems()
		{
			if (IsRootNode())
			{
				var project = ExtObject() as Project;
				if (project != null)
				{
					return project.ProjectItems;
				}
			}
			else
			{
				ProjectItem item = ProjectItem();
				if (item != null)
				{
					return item.ProjectItems;
				}
			}
			return null;
		}

		public string ProjRelativePath()
		{
			string str = null;
			string str2 = Util.EnsureTrailingBackSlash(Root().ProjectDir());
			string str3 = FullPath();
			if (!string.IsNullOrEmpty(str2) && !string.IsNullOrEmpty(str3))
			{
				str = Util.MakeRelativePath(str3, str2);
			}
			return str;
		}

		public string ProjRelativeUrl()
		{
			string str = null;
			string str2 = ProjRelativePath();
			if (!string.IsNullOrEmpty(str2))
			{
				str = str2.Replace(Path.DirectorySeparatorChar, '/');
			}
			return str;
		}

		public EnvDTE.Properties Properties()
		{
			object obj2 = ExtObject();
			if (obj2 is Project)
			{
				return ((Project) obj2).Properties;
			}
			if (obj2 is ProjectItem)
			{
				return ((ProjectItem) obj2).Properties;
			}
			return null;
		}

		public VsHierarchyItem Root()
		{
			return new VsHierarchyItem(0xfffffffe, _hier);
		}

		public static VsHierarchyItem SelectedDocument(IVsMonitorSelection service)
		{
			IVsHierarchy hier = null;
			object obj2;
			object obj3;
			uint id = 0;
			//var service = Util.GetService<IVsMonitorSelection>();
			if ((((service != null) && NativeMethods.Succeeded(service.GetCurrentElementValue(2, out obj2))) &&
			     ((obj2 != null) && NativeMethods.Succeeded(service.GetCurrentElementValue(1, out obj3)))) && (obj3 != null))
			{
				var frame = obj2 as IVsWindowFrame;
				var frame2 = obj3 as IVsWindowFrame;
				if ((frame != null) && (frame == frame2))
				{
					object obj4;
					object obj5;
					frame.GetProperty(-4005, out obj4);
					frame.GetProperty(-4006, out obj5);
					try
					{
						hier = (IVsHierarchy) obj4;
						id = (uint) ((int) obj5);
					}
					catch
					{
					}
				}
			}
			if ((hier != null) && (id != 0))
			{
				return new VsHierarchyItem(id, hier);
			}
			return null;
		}

		public IVsWindowFrame Show()
		{
			IVsWindowFrame frame = DocumentFrame();
			if (frame != null)
			{
				frame.Show();
				return frame;
			}
			return Open();
		}

		public IServiceProvider Site()
		{
			IServiceProvider ppSP = null;
			if (_hier != null)
			{
				_hier.GetSite(out ppSP);
			}
			return ppSP;
		}

		public Solution Solution()
		{
			Solution solution = null;
			DTE dte = DTE();
			if (dte != null)
			{
				solution = dte.Solution;
			}
			return solution;
		}

		public SolutionBuild SolutionBuild()
		{
			SolutionBuild solutionBuild = null;
			Solution solution = Solution();
			if (solution != null)
			{
				solutionBuild = solution.SolutionBuild;
			}
			return solutionBuild;
		}

		public string[] StartupProjects()
		{
			string[] strArray = null;
			SolutionBuild build = SolutionBuild();
			if (build != null)
			{
				var startupProjects = build.StartupProjects as Array;
				if ((startupProjects != null) && (startupProjects.Length > 0))
				{
					strArray = new string[startupProjects.Length];
					startupProjects.CopyTo(strArray, 0);
				}
			}
			return strArray;
		}

		public override string ToString()
		{
			return Name();
		}

		public IVsUIHierarchy UIHierarchy()
		{
			return (_hier as IVsUIHierarchy);
		}

		public IOleUndoManager UndoManager()
		{
			IVsTextLines runningDocumentTextBuffer = GetRunningDocumentTextBuffer();
			if (runningDocumentTextBuffer != null)
			{
				IOleUndoManager ppUndoManager = null;
				runningDocumentTextBuffer.GetUndoManager(out ppUndoManager);
				return ppUndoManager;
			}
			return null;
		}

		public int WalkDepthFirst(bool fVisible, ProcessItemDelegate processCallback, object callerObject)
		{
			if (processCallback != null)
			{
				object obj2;
				int num = processCallback(this, callerObject, out obj2);
				if (num != 0)
				{
					return num;
				}
				if (obj2 == null)
				{
					obj2 = callerObject;
				}
				if (IsExpandable())
				{
					int num2 = 0;
					for (VsHierarchyItem item = FirstChild(fVisible); item != null; item = item.NextSibling(fVisible))
					{
						num2 = item.WalkDepthFirst(fVisible, processCallback, obj2);
						if (num2 == -1)
						{
							return num2;
						}
					}
				}
			}
			return 0;
		}
	}
}