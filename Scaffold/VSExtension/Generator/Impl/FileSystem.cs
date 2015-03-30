using System;
using System.Collections.Generic;
using System.IO;

namespace Flywheel.Generator
{
    public class FileSystem : IFileSystem
    {
        #region IFileSystem Members

        public string ReadFile(string filename)
        {
            return File.ReadAllText(filename);
        }

        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }


        public string WriteTempFile(string content)
        {
            var filename = GetTempFilename();
            WriteFile(filename,content);
            return filename;
        }
        public void WriteFile(string filePath, string content)
        {
            CreateDirectory(filePath);
            File.WriteAllText(filePath, content);
        }

        public string GetTempFilename()
        {
            return Path.GetTempFileName();
        }

        public IEnumerable<string> GetFilesInDirectory(string directory, string filePattern)
        {
            return Directory.GetFiles(directory, filePattern);
        }

        public IEnumerable<string> GetSubDirectories(string directory)
        {
            if (Directory.Exists(directory))
            {
                foreach (string dir in Directory.GetDirectories(directory))
                {
                    if(new DirectoryInfo(dir).Attributes!=FileAttributes.Hidden )//&&  !Path.GetFileName(dir).StartsWith("."))
                    {
                        yield return dir;
                    }
                } 
            }
            //yield return null;
        }

        public string GetFileName(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        public string GetDirectoryName(string fileName)
        {
            return Path.GetDirectoryName(fileName);
        }

        public string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        public void Copy(string targetFile, string destinationFile)
        {
            Directory.CreateDirectory(GetDirectoryName(destinationFile));
            File.Copy(targetFile, destinationFile);
        }

        public string GetFileNameWithExtension(string file)
        {
            return Path.GetFileName(file);
        }

        public void CreateDirectory(string directory)
        {
            
            Directory.CreateDirectory(GetDirectoryName(directory));
        }

        public void CopyFilesAndDirectories(string sourceDir, string targetDir)
        {
            CopyFiles(sourceDir, targetDir);
            CopySubDirs(sourceDir, targetDir);
        }

        public void Delete(string directory)
        {
            if(Directory.Exists(directory))
                Directory.Delete(directory,true);
        }

        private void CopyFiles(string sourceDir, string targetDir)
        {
            foreach (string file in GetFilesInDirectory(sourceDir,"*.*"))
            {
                var destinationFile = Path.Combine(targetDir, GetFileNameWithExtension(file));
                 Copy(file,destinationFile);
                
            }
        }

        private void CopySubDirs(string sourceDir, string targetDir)
        {
            foreach (string subDirectory in GetSubDirectories(sourceDir))
            {

                if (!GetFileName(subDirectory).Equals(string.Empty))
                {
                    var dirArray = subDirectory.Split('\\');
                    var dirname = dirArray[dirArray.Length - 1];
                    var subDirectoryTarget = Path.Combine(targetDir, dirname);
                    CopyFilesAndDirectories(subDirectory, subDirectoryTarget);
                }
            }
        }

        #endregion
    }
}