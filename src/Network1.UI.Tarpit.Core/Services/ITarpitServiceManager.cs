namespace Network1.UI.Tarpit.Core.Services
{
	public interface ITarpitServiceManager
	{
		bool IsServiceRunning { get; }

		int StartService();
		void StopService();
	}
}