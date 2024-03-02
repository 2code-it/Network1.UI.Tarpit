using CommunityToolkit.Mvvm.Messaging;
using Network1.UI.Tarpit.Core.Models;
using Network1.UI.Tarpit.Core.Services.Internals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Network1.UI.Tarpit.Core.Services
{
	public class ConnectionLogService : IConnectionLogService
	{
		public ConnectionLogService(IMessenger messenger, IAppOptionsManager appOptionsManager) : this(messenger, appOptionsManager, new FileSystem()) { }
		internal ConnectionLogService(IMessenger messenger, IAppOptionsManager appOptionsManager, IFileSystem fileSystem)
		{
			_messenger = messenger;
			_fileSystem = fileSystem;
			_appOptionsManager = appOptionsManager;

			_appOptionsManager.Load(Options);
			_messenger.Register<TarpitConnection[], int>(this, 0, Receive);
		}

		private readonly IMessenger _messenger;
		private readonly IAppOptionsManager _appOptionsManager;
		private readonly IFileSystem _fileSystem;

		public ConnectionLogOptions Options { get; private set; } = new ConnectionLogOptions();
		private readonly object _lock = new();

		public TarpitConnection[] GetConnections(DateTime from, DateTime to)
		{
			return new TarpitConnection[] { };
		}

		public void Receive(object? sender, TarpitConnection[] message)
		{
			if (!Options.IsEnabled) return;
			string[] items = message.Where(x => x.IsCompleted).Select(GetStringFromConnection).ToArray();
			if (items.Length == 0) return;
			FileWrite(items);
		}

		private string GetStringFromConnection(TarpitConnection status)
		{
			string[] items = new[]
			{
				status.Created.ToString(Options.LogDateFormat),
				status.RemoteEndPoint,
				status.LocalEndPoint,
				status.DurationInSeconds.ToString(),
				status.BytesSent.ToString()
			};
			return string.Join(Options.LogValueDelimiter, items);
		}

		private void FileWrite(IEnumerable<string> items)
		{
			lock (_lock)
			{
				EnsureDirectoryExists();
				_fileSystem.FileAppendAllLines(GetFilePath(), items);
			}
		}

		private void EnsureDirectoryExists()
		{
			string path = GetDirectoryPath();
			if (!Directory.Exists(path))
			{
				_fileSystem.DirectoryCreateDirectory(path);
			}
		}

		private string GetDirectoryPath()
		{
			return _fileSystem.PathGetFullPath(Options.DirectoryPath);
		}

		private string GetFilePath()
		{
			string? fileNameSuffix = Options.FileNameSuffixDateFormat is null ? null : DateTime.Now.ToString(Options.FileNameSuffixDateFormat);
			string fileName = $"{Options.FileNamePrefix}{fileNameSuffix}.txt";
			return _fileSystem.PathCombine(GetDirectoryPath(), fileName);
		}
	}
}
