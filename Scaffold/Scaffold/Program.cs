using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using RazorEngine;
using RazorEngine.Templating;

namespace Scaffold
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var files = new List<string>();
            string scaffoldName = args[0];
            string rootFolder = args[1];
            string modelJson = args[2];
            string projectNamespace = args[3];

            var scaffoldPath = "Scaffolds\\"+scaffoldName+"\\";

            var configurationJson = File.OpenText(scaffoldPath+"scaffold.json").ReadToEnd();
            var configuration = JsonConvert.DeserializeObject<ScaffoldConfiguration>(configurationJson);

            var model = new ScaffoldModel(){
                ProjectNamespace = projectNamespace,
                Model = new ModelType() { Name = modelJson}
            };

            foreach (var templateConfiguration in configuration.templates)
            {
                var template = File.OpenText(scaffoldPath+templateConfiguration.name).ReadToEnd();

                string result = Engine.Razor.RunCompile(template, templateConfiguration.name, typeof(ScaffoldModel), model);

                string destinationPath = Engine.Razor.RunCompile(templateConfiguration.destination, templateConfiguration.name+"Path", typeof(string), model.Model.Name);

                var outputPath = Path.Combine(rootFolder, destinationPath);

                Directory.CreateDirectory( Path.GetDirectoryName( outputPath));
                File.WriteAllText(outputPath,result);
                files.Add(Path.GetFullPath(  outputPath));
            }
            //return full file paths. vs2013 can use this to add files to projects.
            Console.Write(JsonConvert.SerializeObject(files));
            
        }
    }
} 