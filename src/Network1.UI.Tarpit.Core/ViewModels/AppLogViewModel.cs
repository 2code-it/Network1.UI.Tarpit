using CommunityToolkit.Mvvm.Messaging;
using Network1.UI.Tarpit.Core.Models;
using Network1.UI.Tarpit.Core.Mvvm;
using Network1.UI.Tarpit.Core.Services;
using System.Threading;

namespace Network1.UI.Tarpit.Core.ViewModels
{
	public class AppLogViewModel
	{
		public AppLogViewModel(IMessenger messenger, IAppOptionsManager appOptionsManager)
		{
			_messenger = messenger;
			_appOptionsManager = appOptionsManager;

			_appOptionsManager.Load(Options);
			_messenger.Register<LogEntry, int>(this, 0, Receive);
			_messenger.Register<AppCommand, int>(this, 0, Receive);
		}

		private readonly IMessenger _messenger;
		private readonly IAppOptionsManager _appOptionsManager;
		public ViewOptions Options { get; private set; } = new ViewOptions();

		private readonly object _lock = new();

		public SyncedObservableCollection<LogEntry> Entries { get; private set; } = new SyncedObservableCollection<LogEntry>();

		public void Receive(object? sender, AppCommand command)
		{
			if (command.Name == AppCommand.OptionsChangedCommand)
			{
				_appOptionsManager.Load(Options);
			}
		}

		public void Receive(object? sender, LogEntry entry)
		{
			lock (_lock)
			{
				if (Entries.Count > 0 && Entries.Count == Options.MaxAppLogEntries)
				{
					Entries.RemoveAt(Entries.Count - 1);
				}
				Entries.Insert(0, entry);
				Thread.Sleep(1);
			}
		}
	}
}
