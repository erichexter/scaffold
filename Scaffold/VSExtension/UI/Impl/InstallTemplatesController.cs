using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using Flywheel.Generator;

namespace Flywheel.UI.Impl
{
    public class InstallTemplatesController : IInstallTemplatesController
    {
        private readonly IAddinConfiguration _addinConfiguration;
        private readonly IFileSystem _fileSystem;
        private readonly IProjectConfiguration _projectConfiguration;
        private readonly IProjectRepository _projectRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly IInstallTemplatesView _view;
        private readonly IVisualStudioNewItemAttacher _visualStudioNewItemAttacher;

        public InstallTemplatesController(IInstallTemplatesView view, IProjectRepository projectRepository,
                                          ITemplateRepository templateRepository, IAddinConfiguration addinConfiguration,
                                          IProjectConfiguration projectConfiguration, IFileSystem fileSystem,
                                          IVisualStudioNewItemAttacher visualStudioNewItemAttacher)
        {
            _view = view;
            _projectRepository = projectRepository;
            _templateRepository = templateRepository;
            _addinConfiguration = addinConfiguration;
            _projectConfiguration = projectConfiguration;
            _fileSystem = fileSystem;
            _visualStudioNewItemAttacher = visualStudioNewItemAttacher;
            _view.CancelClicked = CanelClicked;
            _view.AcceptClicked = AcceptClicked;
        }

        #region IInstallTemplatesController Members

        public string[] TemplateSets { get; private set; }

        public string ProjectName { get; private set; }

        public void Run()
        {
            string[] projects = _projectRepository.GetAll().Select(project => project.Name).ToArray();
            _view.BindProjects(projects);
            _view.BindTemplateSets(_templateRepository.GetTemplateSets(_addinConfiguration.AddinDirectory));
            _view.Show();
        }

        #endregion

        private void AcceptClicked()
        {
            _view.Close();
            List<string> files = CopyFilesIntoProject(_view.GetSelectedProject(), _view.GetSelectedTemplateSets());
            AttachFilesToVisualStudio(files);            
        }

        private void AttachFilesToVisualStudio(List<string> files)
        {
            foreach (string file in files)
            {
                _visualStudioNewItemAttacher.AddFileToProject(file);
            }
        }

        private List<string> CopyFilesIntoProject(string projectName, string[] templateSets)
        {
            var files = new List<string>();
            Project targetProject =
                _projectRepository.GetAll().Where(project => project.Name == projectName).FirstOrDefault();
            string targetProjectPath = _fileSystem.GetDirectoryName(targetProject.FileName)+@"\";
            foreach (string templateSet in templateSets)
            {
                string targetPath = targetProjectPath + _projectConfiguration.TemplateSetDirectory + @"\" + templateSet +
                                    @"\";

                foreach (string file in GetTemplateFiles(templateSet))
                {
                    string destinationFile = targetPath + _fileSystem.GetFileNameWithExtension(file);
                    if (!_fileSystem.Exists(destinationFile))
                    {
                        _fileSystem.Copy(file, destinationFile);
                        files.Add(destinationFile);
                    }
                }
            }
            return files;
        }

        private IEnumerable<string> GetTemplateFiles(string templateSet)
        {
            return _fileSystem.GetFilesInDirectory(
                _addinConfiguration.AddinDirectory + _projectConfiguration.TemplateSetDirectory + @"\" +
                templateSet, "*.*");
        }

        private void CanelClicked()
        {
            _view.Close();
        }
    }
}