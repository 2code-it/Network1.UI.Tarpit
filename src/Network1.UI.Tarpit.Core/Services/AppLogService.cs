using CommunityToolkit.Mvvm.Messaging;
using Network1.UI.Tarpit.Core.Models;
using System;

namespace Network1.UI.Tarpit.Core.Services
{
	public class AppLogService : IAppLogService
	{
		public AppLogService(IMessenger messenger)
		{
			_messenger = messenger;
		}

		private readonly IMessenger _messenger;
		private const string _severityInfo = "info";
		private const string _severityWarning = "warning";
		private const string _severityError = "error";

		public void WriteInfo(string message)
			=> Write(DateTime.Now, _severityInfo, message);

		public void WriteWarning(string message)
			=> Write(DateTime.Now, _severityWarning, message);

		public void WriteError(string message)
			=> Write(DateTime.Now, _severityError, message);

		public void Write(DateTime created, string severity, string message)
		{
			LogEntry logEntry = new LogEntry
			{
				Created = created,
				Severity = severity,
				Message = message
			};
			_messenger.Send(logEntry, 0);
		}
	}
}
