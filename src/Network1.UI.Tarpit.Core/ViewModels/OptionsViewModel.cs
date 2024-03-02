using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Network1.UI.Tarpit.Core.Models;
using Network1.UI.Tarpit.Core.Services;

namespace Network1.UI.Tarpit.Core.ViewModels
{
	public partial class OptionsViewModel : ObservableObject
	{
		public OptionsViewModel(IMessenger messenger, IAppOptionsManager appOptionsManager, IAppLogService appLogService)
		{
			_messenger = messenger;
			_appOptionsManager = appOptionsManager;
			_appLogService = appLogService;

			_messenger.Register<AppCommand, int>(this, 0, Receive);

			ViewOptions.PropertyChanged += OnOptionChanged;
			TarpitOptions.PropertyChanged += OnOptionChanged;
			ConnectionLogOptions.PropertyChanged += OnOptionChanged;
			ViewOptions.ErrorsChanged += OnErrorsChanged;
			TarpitOptions.ErrorsChanged += OnErrorsChanged;
		}

		private readonly IMessenger _messenger;
		private readonly IAppOptionsManager _appOptionsManager;
		private readonly IAppLogService _appLogService;

		public ViewOptions ViewOptions { get; private set; } = new ViewOptions();
		public TarpitOptions TarpitOptions { get; private set; } = new TarpitOptions();
		public ConnectionLogOptions ConnectionLogOptions { get; private set; } = new ConnectionLogOptions();

		[ObservableProperty]
		private bool _hasChanged;
		[ObservableProperty]
		private bool _hasErrors;
		[ObservableProperty]
		private bool _isVisible;

		[RelayCommand]
		private void Save()
		{
			if (HasErrors)
			{
				_appLogService.WriteError("Can't save invalid options");
				return;
			}
			_appOptionsManager.Save(ViewOptions);
			_appOptionsManager.Save(TarpitOptions);
			_appOptionsManager.Save(ConnectionLogOptions);
			HasChanged = false;
			IsVisible = false;
			_appLogService.WriteInfo("Options saved");
			_messenger.Send(new AppCommand(AppCommand.OptionsChangedCommand));
		}

		[RelayCommand]
		private void Close()
		{
			IsVisible = false;
		}

		public void Receive(object? sender, AppCommand command)
		{
			if (command.Name == AppCommand.ShowOptionsViewCommand)
			{
				IsVisible = true;
			}
			if (command.Name == AppCommand.HideOptionsViewCommand)
			{
				IsVisible = false;
			}
		}

		private void LoadOptions()
		{
			_appOptionsManager.Load(ViewOptions);
			_appOptionsManager.Load(TarpitOptions);
			_appOptionsManager.Load(ConnectionLogOptions);
		}

		private void OnErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
		{
			HasErrors = ViewOptions.HasErrors || TarpitOptions.HasErrors;
		}

		partial void OnIsVisibleChanged(bool value)
		{
			if (value)
			{
				LoadOptions();
				HasChanged = false;
				HasErrors = false;
			}
		}

		private void OnOptionChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			HasChanged = true;
		}
	}
}
