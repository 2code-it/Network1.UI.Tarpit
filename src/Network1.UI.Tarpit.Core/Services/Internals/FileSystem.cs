using System;
using System.Collections.Generic;
using System.IO;

namespace Network1.UI.Tarpit.Core.Services.Internals
{

	internal class FileSystem : IFileSystem
	{
		public void FileAppendAllLines(string path, IEnumerable<string> contents)
			=> File.AppendAllLines(path, contents);

		public bool FileExists(string path)
			=> File.Exists(path);

		public string FileReadAllText(string path)
			=> File.ReadAllText(path);

		public void FileWriteAllText(string path, string contents)
			=> File.WriteAllText(path, contents);

		public bool DirectoryExists(string path)
			=> Directory.Exists(path);

		public void DirectoryCreateDirectory(string path)
			=> Directory.CreateDirectory(path);

		public string[] DirectoryGetFiles(string path, string searchPattern = "*.*")
			=> Directory.GetFiles(path, searchPattern);

		public string PathCombine(params string[] paths)
			=> Path.Combine(paths);

		public string PathGetFullPath(string path)
			=> Path.GetFullPath(path, AppDomain.CurrentDomain.BaseDirectory);
	}
}
