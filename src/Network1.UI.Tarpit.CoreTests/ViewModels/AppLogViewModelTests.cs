using CommunityToolkit.Mvvm.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Network1.UI.Tarpit.Core.Models;
using Network1.UI.Tarpit.Core.Services;
using NSubstitute;

namespace Network1.UI.Tarpit.Core.ViewModels.Tests
{
	[TestClass]
	public class AppLogViewModelTests
	{
		[TestMethod]
		public void Receive_When_OptionsChangedCommandReceived_Expect_ViewOptionsReload()
		{
			IMessenger messenger = Substitute.For<IMessenger>();
			IAppOptionsManager appOptionsManager = Substitute.For<IAppOptionsManager>();
			AppLogViewModel appLogViewModel = new AppLogViewModel(messenger, appOptionsManager);

			appLogViewModel.Receive(null, new AppCommand(AppCommand.OptionsChangedCommand));

			appOptionsManager.Received(2).Load(Arg.Any<ViewOptions>());
		}

		[TestMethod]
		public void Receive_When_LogEntryReceived_Expect_EntriesNotExceedingMaxAppLogEntries()
		{
			IMessenger messenger = Substitute.For<IMessenger>();
			IAppOptionsManager appOptionsManager = Substitute.For<IAppOptionsManager>();
			AppLogViewModel appLogViewModel = new AppLogViewModel(messenger, appOptionsManager);
			appLogViewModel.Options.MaxAppLogEntries = 5;

			for (int i = 0; i < appLogViewModel.Options.MaxAppLogEntries + 1; i++)
			{
				appLogViewModel.Receive(null, new LogEntry());
			}

			Assert.AreEqual(appLogViewModel.Entries.Count, appLogViewModel.Options.MaxAppLogEntries);
		}
	}
}