using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Network1.UI.Tarpit.Core.Models;
using Network1.UI.Tarpit.Core.Services;

namespace Network1.UI.Tarpit.Core.ViewModels
{
	public partial class MainWindowViewModel : ObservableObject
	{
		public MainWindowViewModel(IMessenger messenger, IAppControl appControl, ITarpitServiceManager tarpitServiceManager)
		{
			_messenger = messenger;
			_appControl = appControl;
			_tarpitServiceManager = tarpitServiceManager;
		}

		private const string _toggleServiceStartCaption = "Start";
		private const string _toggleServiceStopCaption = "Stop";

		private readonly IMessenger _messenger;
		private readonly IAppControl _appControl;
		private readonly ITarpitServiceManager _tarpitServiceManager;

		[ObservableProperty]
		private string _toggleServiceCaption = _toggleServiceStartCaption;

		[RelayCommand]
		private void ToggleService()
		{
			if (ToggleServiceCaption == _toggleServiceStartCaption)
			{
				int listenerCount = _tarpitServiceManager.StartService();
				if (listenerCount < 1) return;
				ToggleServiceCaption = _toggleServiceStopCaption;
			}
			else
			{
				_tarpitServiceManager.StopService();
				ToggleServiceCaption = _toggleServiceStartCaption;
			}
		}

		[RelayCommand]
		private void Exit()
		{
			if (_tarpitServiceManager.IsServiceRunning) _tarpitServiceManager.StopService();
			_appControl.Shutdown();
		}

		[RelayCommand]
		private void ShowOptions()
		{
			_messenger.Send(new AppCommand(AppCommand.ShowOptionsViewCommand), 0);
		}

	}
}
