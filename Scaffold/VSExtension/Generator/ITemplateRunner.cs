using System.Collections.Generic;
using System.Diagnostics;
using EnvDTE;

namespace Flywheel.Generator
{
	public interface ITemplateRunner
	{
		TemplateRunResult[] RunTemplates(List<string> templateFilenames, ModelType model, CodeType modelCodeType);
	}

    public class TemplateRunner : ITemplateRunner
    {
        private readonly IFileSystem _fileSystem;
        private readonly IVisualStudioNewItemAttacher _visualStudioNewItemAttacher;

        public TemplateRunner(IFileSystem fileSystem, IVisualStudioNewItemAttacher visualStudioNewItemAttacher)
        {
            _fileSystem = fileSystem;
            _visualStudioNewItemAttacher = visualStudioNewItemAttacher;
        }

        public TemplateRunResult[] RunTemplates(List<string> templateFilenames, ModelType model, CodeType modelCodeType)
        {
            Debug.WriteLine("nothing to do in RunTemplates");
            return new TemplateRunResult[0];
        }
    }
}