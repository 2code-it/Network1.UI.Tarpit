using Code2.Net.TcpTarpit;
using CommunityToolkit.Mvvm.Messaging;
using Network1.UI.Tarpit.Core.Models;
using System;
using System.Linq;

namespace Network1.UI.Tarpit.Core.Services
{
	public class TarpitServiceManager : ITarpitServiceManager
	{
		public TarpitServiceManager(IMessenger messenger, ITarpitService tarpitService, IAppOptionsManager appOptionsManager, IAppLogService appLogService, IObjectMapper objectMapper)
		{
			_messenger = messenger;
			_tarpitService = tarpitService;
			_appOptionsManager = appOptionsManager;
			_appLogService = appLogService;
			_objectMapper = objectMapper;
		}

		private readonly ITarpitService _tarpitService;
		private readonly IMessenger _messenger;
		private readonly IAppLogService _appLogService;
		private readonly IObjectMapper _objectMapper;
		private readonly IAppOptionsManager _appOptionsManager;

		public bool IsServiceRunning => _tarpitService.ListenersCount > 0;

		public int StartService()
		{
			_appOptionsManager.Load(_tarpitService.Options, typeof(TarpitOptions));
			int listenerCount = _tarpitService.Start();
			if (listenerCount == 0)
			{
				_appLogService.WriteWarning("Tarpit did not start, not listening on any port");
				return listenerCount;
			}

			_tarpitService.Error += OnTarpitServiceError;
			_tarpitService.ConnectionsUpdated += OnTarpitServiceConnectionsUpdated;
			_appLogService.WriteInfo($"Tarpit service started with {listenerCount} listeners");
			return listenerCount;
		}

		public void StopService()
		{
			if (!IsServiceRunning)
			{
				_appLogService.WriteWarning("Can't stop, service not started");
				return;
			}
			_tarpitService.Error -= OnTarpitServiceError;
			_tarpitService.ConnectionsUpdated -= OnTarpitServiceConnectionsUpdated;
			_tarpitService.Stop();
			_appLogService.WriteInfo("Tarpit service stopped");
		}

		private void OnTarpitServiceConnectionsUpdated(object? sender, ConnectionsUpdatedEventArgs e)
		{
			TarpitConnection[] connections = e.Connections.Select(GetTarpitConnection).ToArray();
			_messenger.Send(connections, 0);
		}

		private void OnTarpitServiceError(object? sender, UnhandledExceptionEventArgs e)
		{
			Exception exception = (Exception)e.ExceptionObject;
			_appLogService.WriteError($"{nameof(TarpitService)}: {exception.Message}");
		}

		private TarpitConnection GetTarpitConnection(ConnectionStatus connectionStatus)
		{
			TarpitConnection connection = _objectMapper.Map<TarpitConnection>(connectionStatus);
			connection.DurationInSeconds = (int)Math.Round((DateTime.Now - connectionStatus.Created).TotalSeconds);
			return connection;
		}
	}
}
