using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.OLE.Interop;
using IServiceProvider=System.IServiceProvider;

namespace Flywheel.VSHelpers
{
	public static class NativeMethods
	{
		#region __PBRP enum

		public enum __PBRP
		{
			PBRP_DontSaveChanges = 2,
			PBRP_PromptForSave = 3,
			PBRP_SaveChanges = 1,
			PBRP_SaveDocumentsOnly = 4
		}

		#endregion

		public const int BUFFER_E_RELOAD_OCCURRED = -2147217399;
		public const int CLSCTX_INPROC_SERVER = 1;
		public const string CLSID_AccessibilityChecker = "{B82A64BF-BEFB-4261-A81F-C06CFBB8997B}";
		public const string CLSID_HtmedPackage = "{1B437D20-F8FE-11D2-A6AE-00104BCC7269}";
		public const string CLSID_VsTextImage = "{66B88230-2363-4992-B740-B0450A6F51CA}";
		public const string CLSID_WebPublisher = "{DAAB8F73-E950-4c19-9787-A13D442EAAB7}";
		public const int DVASPECT_CONTENT = 1;
		public const int E_ABORT = -2147467260;
		public const int E_ACCESSDENIED = -2147024891;
		public const int E_FAIL = -2147467259;
		public const int E_HANDLE = -2147024890;
		public const int E_INVALIDARG = -2147024809;
		public const int E_NOINTERFACE = -2147467262;
		public const int E_NOTIMPL = -2147467263;
		public const int E_OUTOFMEMORY = -2147024882;
		public const int E_PENDING = -2147483638;
		public const int E_POINTER = -2147467261;
		public const int E_UNEXPECTED = -2147418113;
		public const int ERROR_FILE_NOT_FOUND = -2147024894;
		public const int ERROR_PATH_NOT_FOUND = -2147024893;
		public const int GHND = 0x42;
		public const int GMEM_DDESHARE = 0x2000;
		public const int GMEM_DISCARDABLE = 0x100;
		public const int GMEM_DISCARDED = 0x4000;
		public const int GMEM_FIXED = 0;
		public const int GMEM_INVALID_HANDLE = 0x8000;
		public const int GMEM_LOCKCOUNT = 0xff;
		public const int GMEM_LOWER = 0x1000;
		public const int GMEM_MODIFY = 0x80;
		public const int GMEM_MOVEABLE = 2;
		public const int GMEM_NOCOMPACT = 0x10;
		public const int GMEM_NODISCARD = 0x20;
		public const int GMEM_NOT_BANKED = 0x1000;
		public const int GMEM_NOTIFY = 0x4000;
		public const int GMEM_SHARE = 0x2000;
		public const int GMEM_VALID_FLAGS = 0x7f72;
		public const int GMEM_ZEROINIT = 0x40;
		public const int GPTR = 0x40;
		public const string GUID_DataEnvironment = "{CD138AD4-A0BF-4681-8FA7-B6D57D55C4DB}";
		public const int ILD_MASK = 0x10;
		public const int ILD_NORMAL = 0;
		public const int ILD_ROP = 0x40;
		public const int ILD_TRANSPARENT = 1;
		public const int IMAGE_BITMAP = 0;
		public const int LR_CREATEDIBSECTION = 0x2000;
		public const int OLE_E_PROMPTSAVECANCELLED = -2147221492;
		public const int S_FALSE = 1;
		public const int S_OK = 0;
		public const int SMTO_ABORTIFHUNG = 2;
		public const int TYMED_HGLOBAL = 1;
		public const int WM_CLOSE = 0x10;
		public const int WM_QUERYENDSESSION = 0x11;
		public const int WM_USER = 0x400;
		public static readonly Guid IID_IObjectWithSite = typeof (IObjectWithSite).GUID;
		public static readonly Guid IID_IServiceProvider = typeof (IServiceProvider).GUID;
		public static readonly Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");

		[DllImport("KERNEL32", EntryPoint = "RtlMoveMemory", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern void CopyMemoryW(IntPtr pdst, string psrc, int cb);

		[DllImport("gdi32.dll")]
		public static extern int DeleteObject(IntPtr hObject);

		//[DllImport("user32")]
		//public static extern int EnumWindows(EnumWindowsCB func, IntPtr lParam);

		public static bool Failed(int hr)
		{
			return (hr < 0);
		}

		[DllImport("user32")]
		public static extern int GetWindowThreadProcessId(HandleRef hwnd, out int lpdwProcessId);

		[DllImport("KERNEL32", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GlobalAlloc(int uFlags, int dwBytes);

		[DllImport("KERNEL32", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true,
			ExactSpelling = true)]
		public static extern IntPtr GlobalLock(IntPtr h);

		[DllImport("KERNEL32", EntryPoint = "GlobalUnlock", CallingConvention = CallingConvention.StdCall,
			CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		public static extern bool GlobalUnLock(IntPtr h);

		[DllImport("comctl32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImageList_Draw(HandleRef himl, int i, HandleRef hdcDst, int x, int y, int fStyle);

		[DllImport("comctl32.dll", CharSet = CharSet.Auto)]
		public static extern int ImageList_GetImageCount(HandleRef himl);

		[DllImport("user32")]
		public static extern int IsWindow(HandleRef hWnd);

		[DllImport("user32.dll")]
		public static extern IntPtr LoadImage(IntPtr hinst, string lpszName, int uType, int cxDesired, int cyDesired,
		                                      int fuLoad);

		[DllImport("user32")]
		public static extern IntPtr PostMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);

		[DllImport("USER32", EntryPoint = "RegisterClipboardFormatW", CallingConvention = CallingConvention.StdCall,
			CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		public static extern ushort RegisterClipboardFormat(string format);

		[DllImport("user32")]
		public static extern IntPtr SendMessageTimeout(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam, int flags,
		                                               int timeout, out IntPtr pdwResult);

		[DllImport("user32", CharSet = CharSet.Auto)]
		public static extern IntPtr SetFocus(IntPtr hWnd);

		public static bool Succeeded(int hr)
		{
			return (hr >= 0);
		}

		public static int ThrowOnFailure(int hr)
		{
			if (Failed(hr))
			{
				Marshal.ThrowExceptionForHR(hr);
			}
			return hr;
		}
	}
}