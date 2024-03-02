using Code2.Net.TcpTarpit;
using CommunityToolkit.Mvvm.Messaging;
using Network1.UI.Tarpit.Core.Mvvm;
using Network1.UI.Tarpit.Core.Services;
using Network1.UI.Tarpit.Core.ViewModels;
using Network1.UI.Tarpit.WinApp.Views;
using System.Windows;

namespace Network1.UI.Tarpit.WinApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			ConfigureViewModels();

			MainWindowView mainWindowView = new MainWindowView();
			mainWindowView.Show();
			base.OnStartup(e);
		}

		private void ConfigureViewModels()
		{
			IMessenger messenger = WeakReferenceMessenger.Default;
			IObjectMapper objectMapper = new ObjectMapper();
			IAppOptionsManager appOptionsManager = new AppOptionsManager(objectMapper);
			IAppLogService appLogService = new AppLogService(messenger);

			IAppControl appControl = new Services.AppControl();
			ITarpitService tarpitService = new TarpitService();
			ITarpitServiceManager tarpitServiceManager = new TarpitServiceManager(messenger, tarpitService, appOptionsManager, appLogService, objectMapper);
			IConnectionLogService connectionLogService = new ConnectionLogService(messenger, appOptionsManager);

			ViewModelLocator.RegisterViewModel(new MainWindowViewModel(messenger, appControl, tarpitServiceManager));
			ViewModelLocator.RegisterViewModel(new AppLogViewModel(messenger, appOptionsManager));
			ViewModelLocator.RegisterViewModel(new OptionsViewModel(messenger, appOptionsManager, appLogService));
			ViewModelLocator.RegisterViewModel(new ConnectionsViewModel(messenger, appOptionsManager));
			ViewModelLocator.RegisterViewModel(new MetricsViewModel(connectionLogService));
		}
	}
}
