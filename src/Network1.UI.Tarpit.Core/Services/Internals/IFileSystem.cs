using System.Collections.Generic;

namespace Network1.UI.Tarpit.Core.Services.Internals
{
	internal interface IFileSystem
	{
		void DirectoryCreateDirectory(string path);
		bool DirectoryExists(string path);
		string[] DirectoryGetFiles(string path, string searchPattern = "*.*");
		void FileAppendAllLines(string path, IEnumerable<string> contents);
		bool FileExists(string path);
		string FileReadAllText(string path);
		void FileWriteAllText(string path, string contents);
		string PathCombine(params string[] paths);
		string PathGetFullPath(string path);
	}
}