using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using IServiceProvider=System.IServiceProvider;

namespace Flywheel.VSHelpers
{
	public static class Util
	{
		public static int CompareCase(string str1, string str2)
		{
			return string.Compare(str1, str2, StringComparison.Ordinal);
		}

		public static int CompareNoCase(string str1, string str2)
		{
			return string.Compare(str1, str2, StringComparison.OrdinalIgnoreCase);
		}


		public static InterfaceType CreateInstance<InterfaceType>(IServiceProvider serviceProvider, Guid clsid)
			where InterfaceType : class
		{
			InterfaceType objectForIUnknown = default(InterfaceType);
			if (clsid != Guid.Empty)
			{
				var service = GetService<ILocalRegistry>(serviceProvider);
				if (service == null)
				{
					return objectForIUnknown;
				}
				IntPtr zero = IntPtr.Zero;
				Guid riid = NativeMethods.IID_IUnknown;
				try
				{
					service.CreateInstance(clsid, null, ref riid, 1, out zero);
				}
				catch
				{
				}
				if (!(zero != IntPtr.Zero))
				{
					return objectForIUnknown;
				}
				try
				{
					objectForIUnknown = Marshal.GetObjectForIUnknown(zero) as InterfaceType;
				}
				catch
				{
				}
				try
				{
					Marshal.Release(zero);
				}
				catch
				{
				}
			}
			return objectForIUnknown;
		}

		public static InterfaceType CreateInstance<InterfaceType>(IServiceProvider serviceProvider, string clsid)
			where InterfaceType : class
		{
			InterfaceType local = default(InterfaceType);
			if (!string.IsNullOrEmpty(clsid))
			{
				Guid empty = Guid.Empty;
				try
				{
					empty = new Guid(clsid);
				}
				catch
				{
				}
				if (empty != Guid.Empty)
				{
					local = CreateInstance<InterfaceType>(serviceProvider, empty);
				}
			}
			return local;
		}


		public static InterfaceType CreateSitedInstance<InterfaceType>(
			Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider,
			Guid clsid) where InterfaceType : class
		{
			return CreateSitedInstance<InterfaceType>(new ServiceProvider(serviceProvider), clsid);
		}

		public static InterfaceType CreateSitedInstance<InterfaceType>(IServiceProvider serviceProvider, Guid clsid)
			where InterfaceType : class
		{
			var local = CreateInstance<InterfaceType>(serviceProvider, clsid);
			if (local == null)
			{
				return local;
			}
			var site = local as IObjectWithSite;
			var service = GetService<IServiceProvider>(serviceProvider);
			if ((site != null) && (service != null))
			{
				site.SetSite(service);
				return local;
			}
			return default(InterfaceType);
		}

		public static string EnsureTrailingBackSlash(string str)
		{
			if ((str != null) && !str.EndsWith(@"\", StringComparison.Ordinal))
			{
				str = str + @"\";
			}
			return str;
		}

		public static string EnsureTrailingSlash(string str)
		{
			if ((str != null) && !str.EndsWith("/", StringComparison.Ordinal))
			{
				str = str + "/";
			}
			return str;
		}

		public static bool EqualsCase(string str1, string str2)
		{
			if (str1 == null)
			{
				return (str2 == null);
			}
			if (str2 == null)
			{
				return false;
			}
			if (str1.Length != str2.Length)
			{
				return false;
			}
			return (CompareNoCase(str1, str2) == 0);
		}

		public static bool EqualsNoCase(string str1, string str2)
		{
			if (str1 == null)
			{
				return (str2 == null);
			}
			if (str2 == null)
			{
				return false;
			}
			if (str1.Length != str2.Length)
			{
				return false;
			}
			return (CompareCase(str1, str2) == 0);
		}

		public static ICollection FilterGenericTypes(ICollection types)
		{
			if ((types == null) || (types.Count == 0))
			{
				return types;
			}
			var list = new ArrayList(types.Count);
			foreach (Type type in types)
			{
				if (!type.ContainsGenericParameters)
				{
					list.Add(type);
				}
			}
			return list;
		}


		public static string GetDefaultUILocale()
		{
			return "1033";
		}


		public static string GetLocalRegRoot(IServiceProvider serviceProvider)
		{
			object obj2;
			GetService<IVsShell>(serviceProvider).GetProperty(-9002, out obj2);
			if ((obj2 != null) && (obj2 is string))
			{
				return (string) obj2;
			}
			return null;
		}

		public static ValueType GetLocalRegValue<ValueType>(IServiceProvider serviceProvider, string key, string valueName,
		                                                    ValueType defaultValue)
		{
			ValueType local = defaultValue;
			if (!string.IsNullOrEmpty(key))
			{
				string localRegRoot = GetLocalRegRoot(serviceProvider);
				if (string.IsNullOrEmpty(localRegRoot))
				{
					return local;
				}
				string keyName = @"HKEY_LOCAL_MACHINE\" + EnsureTrailingBackSlash(localRegRoot) + key;
				try
				{
					object obj2 = Registry.GetValue(keyName, valueName, defaultValue);
					if (obj2 != null)
					{
						local = (ValueType) obj2;
					}
				}
				catch
				{
				}
			}
			return local;
		}


		public static IVsHierarchy GetSelectedHierarchy(Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider)
		{
			return GetSelectedHierarchy(new ServiceProvider(serviceProvider));
		}

		public static IVsHierarchy GetSelectedHierarchy(IServiceProvider serviceProvider)
		{
			IVsHierarchy objectForIUnknown = null;
			if (serviceProvider != null)
			{
				var service = GetService<IVsMonitorSelection>(serviceProvider);
				if (service == null)
				{
					return objectForIUnknown;
				}
				uint maxValue = uint.MaxValue;
				IVsMultiItemSelect ppMIS = null;
				IntPtr zero = IntPtr.Zero;
				IntPtr ppSC = IntPtr.Zero;
				service.GetCurrentSelection(out zero, out maxValue, out ppMIS, out ppSC);
				if (ppSC != IntPtr.Zero)
				{
					Marshal.Release(ppSC);
				}
				if (((zero != IntPtr.Zero) && (((int) zero) != -1)) && (((int) zero) != -2))
				{
					objectForIUnknown = Marshal.GetObjectForIUnknown(zero) as IVsHierarchy;
					Marshal.Release(zero);
				}
			}
			return objectForIUnknown;
		}


		public static InterfaceType GetService<InterfaceType>(
			Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider)
			where InterfaceType : class
		{
			return GetService<InterfaceType>(new ServiceProvider(serviceProvider));
		}

		public static InterfaceType GetService<InterfaceType>(IServiceProvider serviceProvider) where InterfaceType : class
		{
			InterfaceType service = default(InterfaceType);
			try
			{
				service = serviceProvider.GetService(typeof (InterfaceType)) as InterfaceType;
			}
			catch
			{
			}
			return service;
		}

		public static InterfaceType GetService<InterfaceType, ServiceType>(IServiceProvider serviceProvider)
			where InterfaceType : class where ServiceType : class
		{
			InterfaceType service = default(InterfaceType);
			try
			{
				service = serviceProvider.GetService(typeof (ServiceType)) as InterfaceType;
			}
			catch
			{
			}
			return service;
		}


		public static string GetUILocale(IServiceProvider serviceProvider)
		{
			string str = "1033";
			IUIHostLocale2 service = GetService<IUIHostLocale2, IUIHostLocale>(serviceProvider);
			if (service != null)
			{
				uint plcid = 0x409;
				if (NativeMethods.Succeeded(service.GetUILocale(out plcid)))
				{
					str = plcid.ToString(CultureInfo.InvariantCulture);
				}
			}
			return str;
		}


		public static ValueType GetUserLocalRegValue<ValueType>(IServiceProvider serviceProvider, string key, string valueName,
		                                                        ValueType defaultValue)
		{
			ValueType local = defaultValue;
			if (!string.IsNullOrEmpty(key))
			{
				string localRegRoot = GetLocalRegRoot(serviceProvider);
				if (string.IsNullOrEmpty(localRegRoot))
				{
					return local;
				}
				string keyName = @"HKEY_CURRENT_USER\" + EnsureTrailingBackSlash(localRegRoot) + key;
				try
				{
					object obj2 = Registry.GetValue(keyName, valueName, defaultValue);
					if (obj2 != null)
					{
						local = (ValueType) obj2;
					}
				}
				catch
				{
				}
			}
			return local;
		}


		public static string LoadPackageResourceString(IServiceProvider serviceProvider, ref Guid packageID, string resourceID)
		{
			string pbstrValue = null;
			IVsResourceManager service = GetService<IVsResourceManager, SVsResourceManager>(serviceProvider);
			if (service != null)
			{
				service.LoadResourceString(ref packageID, -1, resourceID, out pbstrValue);
			}
			return pbstrValue;
		}


		public static string MakeRelativePath(string fullPath, string basePath)
		{
			string str = Path.DirectorySeparatorChar.ToString();
			string str2 = basePath;
			string str3 = fullPath;
			string str4 = null;
			if (!str2.EndsWith(str, StringComparison.Ordinal))
			{
				str2 = str2 + str;
			}
			str3 = str3.ToLowerInvariant();
			str2 = str2.ToLowerInvariant();
			while (!string.IsNullOrEmpty(str2))
			{
				if (str3.StartsWith(str2, StringComparison.Ordinal))
				{
					str4 = str4 + fullPath.Remove(0, str2.Length);
					if (str4 == str)
					{
						str4 = "";
					}
					return str4;
				}
				str2 = str2.Remove(str2.Length - 1);
				int num = str2.LastIndexOf(str, StringComparison.Ordinal);
				if (-1 != num)
				{
					str2 = str2.Remove(num + 1);
					str4 = str4 + ".." + str;
				}
				else
				{
					return fullPath;
				}
			}
			return fullPath;
		}

		public static string MakeRelativeUrl(string fullPath, string basePath)
		{
			string str = null;
			string str2 = MakeRelativePath(fullPath, basePath);
			if (!string.IsNullOrEmpty(str2))
			{
				str = str2.Replace(Path.DirectorySeparatorChar, '/');
			}
			return str;
		}


		public static bool SetUserLocalRegValue(IServiceProvider serviceProvider, string key, string valueName,
		                                        object objValue)
		{
			bool flag = false;
			if (!string.IsNullOrEmpty(key))
			{
				string localRegRoot = GetLocalRegRoot(serviceProvider);
				if (string.IsNullOrEmpty(localRegRoot))
				{
					return flag;
				}
				string keyName = @"HKEY_CURRENT_USER\" + EnsureTrailingBackSlash(localRegRoot) + key;
				try
				{
					Registry.SetValue(keyName, valueName, objValue);
					flag = true;
				}
				catch
				{
				}
			}
			return flag;
		}

		public static bool SplitUrl(string pszUrl, out string strServer, out string strWeb)
		{
			strServer = null;
			strWeb = null;
			bool flag = false;
			int index = pszUrl.IndexOf("://", StringComparison.Ordinal);
			if (index < 0)
			{
				return flag;
			}
			index += 3;
			if (index >= pszUrl.Length)
			{
				return flag;
			}
			index = pszUrl.IndexOf('/', index);
			if (index <= 0)
			{
				return flag;
			}
			strServer = pszUrl.Substring(0, index + 1);
			if (((pszUrl.Length - index) - 1) <= 0)
			{
				strWeb = "";
			}
			else
			{
				strWeb = pszUrl.Substring(index + 1, (pszUrl.Length - index) - 1);
			}
			strWeb = StripTrailingSlash(strWeb);
			return true;
		}

		public static string StripTrailingBackSlash(string str)
		{
			if ((str != null) && str.EndsWith(@"\", StringComparison.Ordinal))
			{
				str = str.Substring(0, str.Length - 1);
			}
			return str;
		}

		public static string StripTrailingSlash(string str)
		{
			if ((str != null) && str.EndsWith("/", StringComparison.Ordinal))
			{
				str = str.Substring(0, str.Length - 1);
			}
			return str;
		}
	}
}