using System.Collections.Generic;
using System.Diagnostics;
using EnvDTE;
using Newtonsoft.Json;
using Scaffold;
using Process = System.Diagnostics.Process;

namespace Flywheel.Generator
{
    public interface ITemplateRunner
    {
        TemplateRunResult[] RunTemplates(ScaffoldSelection templateFilenames, ScaffoldModel model, CodeType modelCodeType);
    }

    public class TemplateRunner : ITemplateRunner
    {
        private readonly IFileSystem _fileSystem;
        private readonly IProcessRunner _processRunner;
        private readonly IVisualStudioNewItemAttacher _visualStudioNewItemAttacher;

        public TemplateRunner(IProcessRunner processRunner, IVisualStudioNewItemAttacher visualStudioNewItemAttacher,
            IFileSystem fileSystem)
        {
            _processRunner = processRunner;
            _visualStudioNewItemAttacher = visualStudioNewItemAttacher;
            _fileSystem = fileSystem;
        }

        public TemplateRunResult[] RunTemplates(ScaffoldSelection scaffoldSelection, ScaffoldModel model,
            CodeType modelCodeType)
        {
            string resultfilename = _fileSystem.GetTempFilename();
            string modeljson = JsonConvert.SerializeObject(model);
            string modelfile = _fileSystem.WriteTempFile(modeljson);
            Debug.WriteLine("nothing to do in RunTemplates");

            string args = string.Format("{0} {1} {2}", scaffoldSelection.Name, modelfile, resultfilename);
            _processRunner.Run("scaffold.exe", args);

            //Scaffold.Program.Main(new string[] {scaffoldSelection.Name,modelfile,resultfilename});

            _fileSystem.Delete(modelfile);
            string resultsfile = _fileSystem.ReadFile(resultfilename);
            var results = JsonConvert.DeserializeObject<string[]>(resultsfile);
            foreach (string file in results)
            {
                if (_fileSystem.Exists(file))
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
            var process = new Process();
            process.StartInfo.FileName = filename;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            return new string[0];
        }
    }
}