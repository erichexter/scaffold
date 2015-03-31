using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using RazorEngine;
using RazorEngine.Templating;

namespace Scaffold
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var files = new List<string>();
            string scaffoldName = args[0];            
            string modelFile = args[1];
            string resultFile = args[2];

            var modelJson = File.OpenText(modelFile).ReadToEnd();
            var model = JsonConvert.DeserializeObject<ScaffoldModel>(modelJson);

            var scaffoldPath =model.ProjectDirectory + "\\Scaffolds\\" + scaffoldName + "\\";

            var configurationJson = File.OpenText(scaffoldPath+"scaffold.json").ReadToEnd();
            var configuration = JsonConvert.DeserializeObject<ScaffoldConfiguration>(configurationJson);


            foreach (var templateConfiguration in configuration.templates)
            {
                try
                {
                    var template = File.OpenText(scaffoldPath + templateConfiguration.name).ReadToEnd();

                    string result = Engine.Razor.RunCompile(template, templateConfiguration.name, typeof (ScaffoldModel),
                        model);

                    string destinationPath = Engine.Razor.RunCompile(templateConfiguration.destination,
                        templateConfiguration.name + "Path", typeof (string), model.Model.Name);

                    var outputPath = Path.Combine(model.ProjectDirectory, destinationPath);

                    Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                    File.WriteAllText(outputPath, result);
                    files.Add(Path.GetFullPath(outputPath));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Console.WriteLine(e.Message);
                }
            }

            //return full file paths. vs2013 can use this to add files to projects.
            File.WriteAllText(resultFile, JsonConvert.SerializeObject(files));
            //Console.ReadLine();
        }
    }
} 