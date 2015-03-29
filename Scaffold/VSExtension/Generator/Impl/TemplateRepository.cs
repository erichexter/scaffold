using System.Collections.Generic;
using System.Linq;
using Flywheel.Generator;

namespace Flywheel.UI
{
	public class TemplateRepository : ITemplateRepository
	{
		private readonly IFileSystem _fileSystem;
		private readonly IProjectConfiguration _projectConfiguration;

		public TemplateRepository(IFileSystem fileSystem, IProjectConfiguration projectConfiguration)
		{
			_fileSystem = fileSystem;
			_projectConfiguration = projectConfiguration;
		}

		#region ITemplateRepository Members

		public IEnumerable<string> GetTemplateSets(string projectDirectory)
		{
			return _fileSystem.GetSubDirectories(projectDirectory + _projectConfiguration.TemplateSetDirectory)
				.Select(s => s.Split('\\')
				             	.Last())
				.Where(s => !s.StartsWith("."))
				.ToArray();
		}

		public IEnumerable<string> GetTemplatesForASet(string projectDirectory, string template)
		{
			return GetFileNamesOfTheTemplateDirectory(projectDirectory, template);
		}

		#endregion

		private IEnumerable<string> GetFileNamesOfTheTemplateDirectory(string projectDirectory, string template)
		{
			return GetAllTextTemplateFiles(projectDirectory, template)
				.Select(s => _fileSystem.GetFileName(s)).ToArray();
		}

		private IEnumerable<string> GetAllTextTemplateFiles(string projectDirectory, string template)
		{
			return _fileSystem.GetFilesInDirectory(
				GetTemplateDirectory(projectDirectory, template), "*.cshtml");
		}

		private string GetTemplateDirectory(string projectDirectory, string template)
		{
			return projectDirectory + _projectConfiguration. TemplateSetDirectory + "\\" + template;
		}
	}
}