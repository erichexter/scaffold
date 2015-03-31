using System.Collections.Generic;

namespace Flywheel.Generator
{
	public interface IFileSystem
	{
		string ReadFile(string filename);
		bool Exists(string filePath);
		void WriteFile(string filePath, string content);
		IEnumerable<string> GetFilesInDirectory(string directory, string filePattern);
		IEnumerable<string> GetSubDirectories(string directory);
		string GetFileName(string filePath);
		string GetDirectoryName(string fileName);
		string GetFullPath(string path);
	    void Copy(string targetFile, string destinationFile);
	    string GetFileNameWithExtension(string file);
	    void CreateDirectory(string directory);
	    void CopyFilesAndDirectories(string sourceDir, string targetDir);
	    void Delete(string directory);
	    string WriteTempFile(string content);
	    string GetTempFilename();
	}
}