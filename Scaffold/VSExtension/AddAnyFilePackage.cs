﻿using System;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Flywheel;
using Flywheel.VSHelpers;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace MadsKristensen.AddAnyFile
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [InstalledProductRegistration("#110", "#112", Version, IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidAddAnyFilePkgString)]
    public sealed class AddAnyFilePackage : ExtensionPointPackage
    {
        private static DTE2 _dte;
        private IoC _ioc;
        public const string Version = "0.1";

        protected override void Initialize()
        {
            _dte = GetService(typeof(DTE)) as DTE2;
            base.Initialize();

            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                CommandID menuCommandID = new CommandID(GuidList.guidAddAnyFileCmdSet, (int)PkgCmdIDList.cmdidMyCommand);
                MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
                mcs.AddCommand(menuItem);

                _ioc = new IoC(_dte);
            }
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            var generator = _ioc.CreateScaffoldingGenerator();
            generator
              .ForModelSelectedInTheSolutionExplorer()
              .SelectTemplate()
              .Generate()
              .DisplayResults();
            
            
            //UIHierarchyItem item = GetSelectedItem();



            //if (item == null)
            //    return;

            //string folder = FindFolder(item);

            //if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder))
            //    return;

            //string input = PromptForFileName(folder).TrimStart('/', '\\').Replace("/", "\\");

            //if (string.IsNullOrEmpty(input))
            //    return;

            //string file = Path.Combine(folder, input);
            //string dir = Path.GetDirectoryName(file);

            //try
            //{
            //    if (!Directory.Exists(dir))
            //        Directory.CreateDirectory(dir);
            //}
            //catch (Exception)
            //{
            //    System.Windows.Forms.MessageBox.Show("Error creating the folder: " + dir);
            //    return;
            //}
            ////find project for each file not the active project
            //if (!File.Exists(file))
            //{
            //    int position = WriteFile(file);

            //    try
            //    {
            //        AddFileToActiveProject(file);
            //        Window window = _dte.ItemOperations.OpenFile(file);

            //        // Move cursor into position
            //        if (position > 0)
            //        {
            //            TextSelection selection = (TextSelection)window.Selection;
            //            selection.CharRight(Count: position - 1);
            //        }

            //        SelectCurrentItem();
            //    }
            //    catch { /* Something went wrong. What should we do about it? */ }
            //}
            //else
            //{
            //    System.Windows.Forms.MessageBox.Show("The file '" + file + "' already exist.");
            //}
        }

        private static string PromptForFileName(string folder)
        {
            DirectoryInfo dir = new DirectoryInfo(folder);
            string message = "Please enter a file name. \r\rThe file will be placed in the folder '" + dir.Name + "'";
            return Interaction.InputBox(message, "File name", "file.txt");
        }

        private static int WriteFile(string file)
        {
            Encoding encoding = Encoding.UTF8;
            string extension = Path.GetExtension(file);

            string assembly = Assembly.GetExecutingAssembly().Location;
            string folder = Path.GetDirectoryName(assembly).ToLowerInvariant();
            string template = Path.Combine(folder, "Templates\\", extension);

            if (File.Exists(template))
            {
                string content = File.ReadAllText(template);
                int index = content.IndexOf('$');
                content = content.Remove(index, 1);
                File.WriteAllText(file, content, encoding);
                return index;
            }

            File.WriteAllText(file, string.Empty, encoding);
            return 0;
        }

        private static string FindFolder(UIHierarchyItem item)
        {
            string folder = null;

            ProjectItem projectItem = item.Object as ProjectItem;
            Project project = item.Object as Project;

            if (projectItem != null)
            {
                string fileName = projectItem.FileNames[0];

                if (File.Exists(fileName))
                {
                    folder = Path.GetDirectoryName(fileName);
                }
                else
                {
                    folder = fileName;
                }
            }
            else if (project != null)
            {
                Property prop = GetProjectRoot(project);

                if (prop != null)
                {
                    string value = prop.Value.ToString();

                    if (File.Exists(value))
                    {
                        folder = Path.GetDirectoryName(value);
                    }
                    else if (Directory.Exists(value))
                    {
                        folder = value;
                    }
                }
            }
            return folder;
        }

        private static Property GetProjectRoot(Project project)
        {
            Property prop;

            try
            {
                prop = project.Properties.Item("FullPath");
            }
            catch (ArgumentException)
            {
                try
                {
                    // MFC projects don't have FullPath, and there seems to be no way to query existence
                    prop = project.Properties.Item("ProjectDirectory");
                }
                catch (ArgumentException)
                {
                    // Installer projects have a ProjectPath.
                    prop = project.Properties.Item("ProjectPath");
                }
            }

            return prop;
        }

        private static UIHierarchyItem GetSelectedItem()
        {
            var items = (Array)_dte.ToolWindows.SolutionExplorer.SelectedItems;

            foreach (UIHierarchyItem selItem in items)
            {
                return selItem;
            }

            return null;
        }

        private static void AddFileToActiveProject(string fileName)
        {
            Project project = GetActiveProject();

            if (project == null || project.Kind == "{8BB2217D-0F2D-49D1-97BC-3654ED321F3B}") // ASP.NET 5 projects
                return;

            string projectFilePath = GetProjectRoot(project).Value.ToString();
            string projectDirPath = Path.GetDirectoryName(projectFilePath);

            if (!fileName.StartsWith(projectDirPath, StringComparison.OrdinalIgnoreCase))
                return;

            project.ProjectItems.AddFromFile(fileName);
        }

        public static Project GetActiveProject()
        {
            try
            {
                Array activeSolutionProjects = _dte.ActiveSolutionProjects as Array;

                if (activeSolutionProjects != null && activeSolutionProjects.Length > 0)
                    return activeSolutionProjects.GetValue(0) as Project;
            }
            catch (Exception)
            {
                // Pass through and return null
            }

            return null;
        }

        private static void SelectCurrentItem()
        {
            if (_dte.Version == "11.0") // This errors in VS2012 for some reason.
                return;

            System.Threading.ThreadPool.QueueUserWorkItem((o) =>
            {
                try
                {
                    _dte.ExecuteCommand("View.TrackActivityInSolutionExplorer");
                    _dte.ExecuteCommand("View.TrackActivityInSolutionExplorer");
                }
                catch { /* Ignore any exceptions */ }
            });
        }
    }
}
