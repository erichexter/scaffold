//using System;
//using System.CodeDom.Compiler;
//using System.Collections.Generic;
//using System.Reflection;
//using EnvDTE;
//using Flywheel.T4Host;
//using Flywheel.VSHelpers;
//using Microsoft.VisualStudio.TextTemplating;
//using VSLangProj;

//namespace Flywheel.Generator
//{
//    public class TemplateRunner : ITemplateRunner
//    {
//        private readonly IFileSystem _fileSystem;
//        private readonly IVisualStudioNewItemAttacher _visualStudioNewItemAttacher;

//        public TemplateRunner(IFileSystem fileSystem, IVisualStudioNewItemAttacher visualStudioNewItemAttacher)
//        {
//            _fileSystem = fileSystem;
//            _visualStudioNewItemAttacher = visualStudioNewItemAttacher;
//        }


//        public TemplateRunResult[] RunTemplates(List<string> templateFilenames, ModelType model, CodeType modelCodeType)
//        {
//            var results = new List<TemplateRunResult>();

//            using (var domainResolver = new AppDomainPathResolver())
//            {
//                domainResolver.AssemblyPath = GetExecutingAssemblyPath();

//                ITextTemplatingEngine engine = new Engine();

//                TextTemplateHost host = CreateTextTemplateHost(model, modelCodeType);

//                foreach (string templateFilename in templateFilenames)
//                {
//                    TemplateRunResult result = ExecuteTemplateAndWriteToDisk(templateFilename, host, engine);

//                    if (ResultIsSuccessOrWarning(result))
//                    {
//                        if (!_visualStudioNewItemAttacher.AddFileToProject(result.OutputFilename,(prjBuildAction) result.BuildAction))
//                        {
//                            result.Result = RunResult.Error;
//                            result.Errors.Add(new Error {Description = "Could not add the file to the Visual Studio project."});
//                        }
//                    }

//                    results.Add(result);
//                }
//            }
//            return results.ToArray();
//        }


//        private bool ResultIsSuccessOrWarning(TemplateRunResult result)
//        {
//            return result.Result == RunResult.Success || result.Result == RunResult.Warning;
//        }

//        private TemplateRunResult ExecuteTemplateAndWriteToDisk(string templateFilename, TextTemplateHost host,
//                                                                ITextTemplatingEngine engine)
//        {
//            TemplateRunResult result = CreateEmptyResult(templateFilename);

//            host.TemplateFile = templateFilename;

//            string templateContent = RunTemplate(host, templateFilename, engine);

//            CollectErrorsAndWarningIntoTheResult(host, result);

//            if (TemplateIsOkToWriteToDisk(host))
//            {
//                if (!_fileSystem.Exists(host.OutputPath))
//                {
//                    result.OutputFilename = _fileSystem.GetFullPath(host.OutputPath);
//                    result.BuildAction = host.BuildAction;
//                    try
//                    {
//                        _fileSystem.WriteFile(host.OutputPath, templateContent);
//                        result.Result = RunResult.Success;
//                    }
//                    catch (Exception ex)
//                    {
//                        result.Result = RunResult.Error;
//                        result.Errors.Add(new Error {Description = "Error writing output. " + ex.Message});
//                    }
//                }
//                else
//                {
//                    result.Result = RunResult.Error;
//                    result.Errors.Add(new Error {Description = "The output file already exists. " + host.OutputPath});
//                }
//            }
//            return result;
//        }

//        private TextTemplateHost CreateTextTemplateHost(ModelType model, CodeType modelCodeType)
//        {
//            var host = new TextTemplateHost();
//            host.Model = model;
//            host.ProjectDefaultNamespace = modelCodeType.ProjectNamespace();
//            host.ProjectDirectory = modelCodeType.ProjectDirectory();
//            host.AssemblyPath = GetExecutingAssemblyPath();
//            return host;
//        }

//        private bool TemplateIsOkToWriteToDisk(TextTemplateHost host)
//        {
//            return !host.Errors.HasErrors;
//        }

//        private void CollectErrorsAndWarningIntoTheResult(TextTemplateHost host, TemplateRunResult result)
//        {
//            if (host.Errors.HasWarnings)
//            {
//                result.Result = RunResult.Warning;
//                foreach (CompilerError error in host.Errors)
//                {
//                    if (error.IsWarning)
//                    {
//                        result.Warnings.Add(new Error {Column = error.Column, Description = error.ErrorText, Line = error.Line});
//                    }
//                }
//            }

//            if (host.Errors.HasErrors)
//            {
//                result.Result = RunResult.Error;
//                foreach (CompilerError error in host.Errors)
//                {
//                    if (!error.IsWarning)
//                    {
//                        result.Errors.Add(new Error {Column = error.Column, Description = error.ErrorText, Line = error.Line});
//                    }
//                }
//            }
//        }

//        private string RunTemplate(TextTemplateHost host, string templateFilename, ITextTemplatingEngine engine)
//        {
//            string templateContent;
//            string template = _fileSystem.ReadFile(templateFilename);
//            template = template + GenerateTemplateClassMembers();
//            templateContent = engine.ProcessTemplate(template, host);
//            return templateContent;
//        }

//        private TemplateRunResult CreateEmptyResult(string templateFilename)
//        {
//            return new TemplateRunResult
//                    {
//                        Errors = new List<Error>(),
//                        Warnings = new List<Error>(),
//                        Result = RunResult.NotRun,
//                        TemplateFilename = templateFilename
//                    };
//        }

//        private string GetExecutingAssemblyPath()
//        {
//            return _fileSystem.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
//        }

//        public static string GenerateTemplateClassMembers()
//        {
//            return
//                @"
//<#+
//	public ModelType model = ((ITemplateContext)System.AppDomain.CurrentDomain.GetData(""host"")).Model;
//    public string projectDefaultNamespace  = ((ITemplateContext)System.AppDomain.CurrentDomain.GetData(""host"")).ProjectDefaultNamespace;
//    public string projectDirectory  = ((ITemplateContext)System.AppDomain.CurrentDomain.GetData(""host"")).ProjectDirectory;
//	public string OutputFilePath
//	{
//		set{AppDomain.CurrentDomain.SetData(""outputpath"",value);}
//	}
//    public int BuildAction
//    {
//        set{AppDomain.CurrentDomain.SetData(""buildaction"",value);}
//    }
//    public int EmbeddedResource
//    {
//        get
//        {
//            return 3;
//        }
//    }
//    public int None
//    {
//        get
//        {
//            return 0;
//        }
//    }
//    public int Compile
//    {
//        get
//        {
//            return 1;
//        }
//    }
//    public int Content
//    {
//        get
//        {
//            return 2;
//        }
//    }
//#>";
//        }
//    }
//}