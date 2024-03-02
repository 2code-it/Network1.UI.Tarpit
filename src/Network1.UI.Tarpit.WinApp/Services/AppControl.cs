using Network1.UI.Tarpit.Core.Services;

namespace Network1.UI.Tarpit.WinApp.Services
{
	public class AppControl : IAppControl
	{
		public void Shutdown()
		{
			App.Current.Shutdown();
		}
	}
}
