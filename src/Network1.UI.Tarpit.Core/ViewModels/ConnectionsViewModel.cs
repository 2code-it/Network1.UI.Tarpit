using CommunityToolkit.Mvvm.Messaging;
using Network1.UI.Tarpit.Core.Models;
using Network1.UI.Tarpit.Core.Mvvm;
using Network1.UI.Tarpit.Core.Services;
using System.Linq;
using System.Threading;

namespace Network1.UI.Tarpit.Core.ViewModels
{
	public class ConnectionsViewModel
	{
		public ConnectionsViewModel(IMessenger messenger, IAppOptionsManager appOptionsManager)
		{
			_messenger = messenger;
			_appOptionsManager = appOptionsManager;

			_appOptionsManager.Load(Options);
			_messenger.Register<TarpitConnection[], int>(this, 0, Receive);
			_messenger.Register<AppCommand, int>(this, 0, Receive);
		}


		private readonly IMessenger _messenger;
		private readonly IAppOptionsManager _appOptionsManager;
		public ViewOptions Options { get; private set; } = new ViewOptions();
		private readonly object _lock = new();

		public SyncedObservableCollection<TarpitConnection> Connections { get; private set; } = new SyncedObservableCollection<TarpitConnection>();

		public void Receive(object? sender, AppCommand command)
		{
			if (command.Name == AppCommand.OptionsChangedCommand)
			{
				_appOptionsManager.Load(Options);
			}
		}

		public void Receive(object? sender, TarpitConnection[] connections)
		{
			foreach (var connection in connections)
			{
				TarpitConnection? subject = Connections.FirstOrDefault(x => x.Id == connection.Id);
				if (subject is null)
				{
					EnsureConnectionSpace();
					AddConnection(connection);
				}
				else
				{
					UpdateConnection(connection, subject);
				}
			}
		}

		private void UpdateConnection(TarpitConnection source, TarpitConnection target)
		{
			target.BytesSent = source.BytesSent;
			target.IsCompleted = source.IsCompleted;
			target.DurationInSeconds = source.DurationInSeconds;
		}

		private void AddConnection(TarpitConnection connection)
		{
			lock (_lock)
			{
				Connections.Insert(0, connection);
				Thread.Sleep(1);
			}
		}

		private void EnsureConnectionSpace()
		{
			lock (_lock)
			{
				if (Connections.Count == 0 || Connections.Count < Options.MaxConnectionEntries) return;
				Connections.RemoveAt(Connections.Count - 1);
			}
		}
	}
}
