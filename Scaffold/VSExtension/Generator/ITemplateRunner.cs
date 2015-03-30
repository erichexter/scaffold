using System.Collections.Generic;
using System.Diagnostics;
using System.Security.AccessControl;
using EnvDTE;

namespace Flywheel.Generator
{
	public interface ITemplateRunner
	{
		TemplateRunResult[] RunTemplates(List<string> templateFilenames, ModelType model, CodeType modelCodeType);
	}

    public class TemplateRunner : ITemplateRunner
    {
        private readonly IProcessRunner _processRunner;
        private readonly IVisualStudioNewItemAttacher _visualStudioNewItemAttacher;
        private readonly IFileSystem _fileSystem;

        public TemplateRunner(IProcessRunner processRunner  , IVisualStudioNewItemAttacher visualStudioNewItemAttacher,IFileSystem fileSystem)
        {
            _processRunner = processRunner;
            _visualStudioNewItemAttacher = visualStudioNewItemAttacher;
            _fileSystem = fileSystem;
        }

        public TemplateRunResult[] RunTemplates(List<string> templateFilenames, ModelType model, CodeType modelCodeType)
        {
            var modeljson = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var modelfile = _fileSystem.WriteTempFile(modeljson);
            Debug.WriteLine("nothing to do in RunTemplates");
            var args = string.Format("{0} {1} {2} {3}", "scaffoldname", "rootfolder", modelfile, "projectnamespace");
            string[] results=_processRunner.Run("scaffold.exe",args);
            _fileSystem.Delete(modelfile);
            foreach (var file in results)
            {
                if (_fileSystem.Equals(file))
                {
                    _visualStudioNewItemAttacher.AddFileToProject(file);
                }    
            }
            
            return new TemplateRunResult[0];
        }
    }

    public interface IProcessRunner
    {
        string[] Run(string scaffoldExe, string arguments);
    }

    public class ProcessRunner : IProcessRunner
    {
        public string[] Run(string filename, string arguments)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = filename;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            return;
        }
    }
}