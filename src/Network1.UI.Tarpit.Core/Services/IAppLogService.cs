using System;

namespace Network1.UI.Tarpit.Core.Services
{
	public interface IAppLogService
	{
		void Write(DateTime created, string severity, string message);
		void WriteError(string message);
		void WriteInfo(string message);
		void WriteWarning(string message);
	}
}