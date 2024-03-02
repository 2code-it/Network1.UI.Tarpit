using Network1.UI.Tarpit.Core.Models;
using System;

namespace Network1.UI.Tarpit.Core.Services
{
	public interface IConnectionLogService
	{
		TarpitConnection[] GetConnections(DateTime from, DateTime to);
	}
}