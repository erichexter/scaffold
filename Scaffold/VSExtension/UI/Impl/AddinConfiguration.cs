using Flywheel.Generator;

namespace Flywheel.UI.Impl
{
    public class AddinConfiguration : IAddinConfiguration
    {
        private readonly IFileSystem _fileSystem;

        public AddinConfiguration(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            AddinDirectory = _fileSystem.GetDirectoryName(this.GetType().Assembly.Location)+@"\";
        }

        public string AddinDirectory { get; set; }
    }
}