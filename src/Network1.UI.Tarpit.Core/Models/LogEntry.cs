using System;

namespace Network1.UI.Tarpit.Core.Models
{
	public class LogEntry
	{
		public DateTime Created { get; set; }
		public string Severity { get; set; } = string.Empty;
		public string Message { get; set; } = string.Empty;
	}
}
