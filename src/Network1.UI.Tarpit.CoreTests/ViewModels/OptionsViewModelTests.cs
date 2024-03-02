using CommunityToolkit.Mvvm.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Network1.UI.Tarpit.Core.Models;
using Network1.UI.Tarpit.Core.Services;
using NSubstitute;

namespace Network1.UI.Tarpit.Core.ViewModels.Tests
{
	[TestClass]
	public class OptionsViewModelTests
	{
		[TestMethod]
		public void Receive_When_OnShowOptionsViewCommand_Expect_IsVisibleTrue()
		{
			IMessenger messenger = Substitute.For<IMessenger>();
			IAppOptionsManager appOptionsManager = Substitute.For<IAppOptionsManager>();
			IAppLogService appLogService = Substitute.For<IAppLogService>();
			OptionsViewModel optionsViewModel = new OptionsViewModel(messenger, appOptionsManager, appLogService);

			optionsViewModel.Receive(null, new AppCommand(AppCommand.ShowOptionsViewCommand));

			Assert.IsTrue(optionsViewModel.IsVisible);
		}

		[TestMethod]
		public void Receive_When_OnHideOptionsViewCommand_Expect_IsVisibleFalse()
		{
			IMessenger messenger = Substitute.For<IMessenger>();
			IAppOptionsManager appOptionsManager = Substitute.For<IAppOptionsManager>();
			IAppLogService appLogService = Substitute.For<IAppLogService>();
			OptionsViewModel optionsViewModel = new OptionsViewModel(messenger, appOptionsManager, appLogService);
			optionsViewModel.IsVisible = true;

			optionsViewModel.Receive(null, new AppCommand(AppCommand.HideOptionsViewCommand));

			Assert.IsFalse(optionsViewModel.IsVisible);
		}

		[TestMethod]
		public void IsVisible_When_IsVisibleSetToTrue_Expect_OptionsLoaded()
		{
			IMessenger messenger = Substitute.For<IMessenger>();
			IAppOptionsManager appOptionsManager = Substitute.For<IAppOptionsManager>();
			IAppLogService appLogService = Substitute.For<IAppLogService>();
			OptionsViewModel optionsViewModel = new OptionsViewModel(messenger, appOptionsManager, appLogService);

			optionsViewModel.IsVisible = true;

			appOptionsManager.Received(3).Load(Arg.Any<object>());
		}

		[TestMethod]
		public void Save_When_OnOptionChanged_Expect_HasChangedTrue()
		{
			IMessenger messenger = Substitute.For<IMessenger>();
			IAppOptionsManager appOptionsManager = Substitute.For<IAppOptionsManager>();
			IAppLogService appLogService = Substitute.For<IAppLogService>();
			OptionsViewModel optionsViewModel = new OptionsViewModel(messenger, appOptionsManager, appLogService);

			optionsViewModel.TarpitOptions.Ports = "100-200";

			Assert.IsTrue(optionsViewModel.HasChanged);
		}

		[TestMethod]
		public void Save_When_HasError_Expect_ErrorLogMessage()
		{
			IMessenger messenger = Substitute.For<IMessenger>();
			IAppOptionsManager appOptionsManager = Substitute.For<IAppOptionsManager>();
			IAppLogService appLogService = Substitute.For<IAppLogService>();
			OptionsViewModel optionsViewModel = new OptionsViewModel(messenger, appOptionsManager, appLogService);
			optionsViewModel.TarpitOptions.ListenAddress = "127.0.0.1";
			optionsViewModel.TarpitOptions.Ports = "1";
			optionsViewModel.TarpitOptions.TimeoutInSeconds = 500;
			optionsViewModel.TarpitOptions.WriteIntervalInMs = 200;
			optionsViewModel.TarpitOptions.WriteSize = 2;
			optionsViewModel.TarpitOptions.UpdateIntervalInSeconds = 10;
			optionsViewModel.TarpitOptions.Ports = null;

			optionsViewModel.SaveCommand.Execute(null);

			appLogService.Received(1).WriteError(Arg.Any<string>());
		}

		[TestMethod]
		public void Save_When_HasNoError_Expect_InfoLogMessageAndOptionsSaved()
		{
			IMessenger messenger = Substitute.For<IMessenger>();
			IAppOptionsManager appOptionsManager = Substitute.For<IAppOptionsManager>();
			IAppLogService appLogService = Substitute.For<IAppLogService>();
			OptionsViewModel optionsViewModel = new OptionsViewModel(messenger, appOptionsManager, appLogService);
			optionsViewModel.TarpitOptions.ListenAddress = "127.0.0.1";
			optionsViewModel.TarpitOptions.Ports = "1";
			optionsViewModel.TarpitOptions.TimeoutInSeconds = 500;
			optionsViewModel.TarpitOptions.WriteIntervalInMs = 200;
			optionsViewModel.TarpitOptions.WriteSize = 2;
			optionsViewModel.TarpitOptions.UpdateIntervalInSeconds = 10;

			optionsViewModel.SaveCommand.Execute(null);

			appLogService.Received(1).WriteInfo(Arg.Any<string>());
			appOptionsManager.Received(3).Save(Arg.Any<object>());
		}

	}
}