using System;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Flywheel.Generator;
using Flywheel.UI;
using Flywheel.UI.Impl;
using Microsoft.VisualStudio.Shell.Interop;
//using Scaffolding.Exporter;
using IServiceProvider=Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace Flywheel.VSHelpers
{
    public class IoC : IIoC
    {
        private readonly DTE2 _dte2;

        public IoC(DTE2 dte2)
        {
            _dte2 = dte2;
        }

        public IVsMonitorSelection MonitorSelection
        {
            get { return GetService<IVsMonitorSelection>(); }
        }

        #region IIoC Members

        public TInterface GetService<TInterface>()
        {
            return (TInterface) GetService(_dte2, typeof (TInterface).GUID);
        }

        public IShouldContextMenu CreateShouldContextMenu()
        {
            return new ShouldContextMenu(new CodeWindowSelector(MonitorSelection),
                                         CreateSolutionExplorerSelector());
        }

        public IScaffoldingGenerator CreateScaffoldingGenerator()
        {
            return new ScaffoldingGenerator(new CodeWindowSelector(MonitorSelection),
                                            CreateSolutionExplorerSelector(), CreateTemplateRunner(),
                                            CreateUserInterface());
        }

        public IInstallTemplatesController CreateInstallTemplatesController()
        {            
            return new InstallTemplatesController(new InstallTemplatesView(), new ProjectRepository((DTE) _dte2),
                                                  CreateTemplateRepository(), new AddinConfiguration(new FileSystem()),
                                                  new ProjectConfiguration(),new FileSystem(),CreateVisualStudioAttacher());
        }

        //public ExportGenerator CreateExportGenerator()
        //{
        //    return new ExportGenerator(
        //        (DTE) this._dte2,
        //        new FileSystem(),
        //        new FileTokenReplacer(new FileSystem()),
        //        new MessageBox());
        //}

        #endregion

        private SolutionExplorerSelector CreateSolutionExplorerSelector()
        {
            return new SolutionExplorerSelector(MonitorSelection,
                                                (DTE) _dte2);
        }

        private ITemplateRunner CreateTemplateRunner()
        {
            return new TemplateRunner(new FileSystem(), CreateVisualStudioAttacher());
        }

        private VisualStudioNewItemAttacher CreateVisualStudioAttacher()
        {
            return new VisualStudioNewItemAttacher((DTE) _dte2, new FileSystem());
        }

        private UserInterface CreateUserInterface()
        {
            return new UserInterface(CreateTemplateSelectionController(),
                                     new ProjectConfiguration(),
                                     CreateTemplateStatusController());
        }

        private TemplateSelectionController CreateTemplateSelectionController()
        {
            return new TemplateSelectionController(new TemplateSelectionView(),
                                                   CreateTemplateRepository(), new MessageBox());
        }

        private TemplateRepository CreateTemplateRepository()
        {
            return new TemplateRepository(
                new FileSystem(),
                new ProjectConfiguration());
        }

        private TemplateStatusController CreateTemplateStatusController()
        {
            return new TemplateStatusController(new MessageBox(), new TemplateStatusView(new FileSystem()));
        }

        public object GetService(object serviceProvider, Guid guid)
        {
            object objService = null;

            IServiceProvider objIServiceProvider;

            IntPtr objIntPtr;

            int hr;

            Guid objSIDGuid;
            Guid objIIDGuid;
            objSIDGuid = guid;
            objIIDGuid = objSIDGuid;
            objIServiceProvider = (IServiceProvider) serviceProvider;
            hr = objIServiceProvider.QueryService(ref objSIDGuid, ref objIIDGuid, out objIntPtr);
            if (hr != 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
            else if (!objIntPtr.Equals(IntPtr.Zero))
            {
                objService = Marshal.GetObjectForIUnknown(objIntPtr);
                Marshal.Release(objIntPtr);
            }
            return objService;
        }
    }
}