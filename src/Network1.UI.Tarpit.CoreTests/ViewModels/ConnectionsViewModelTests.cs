using CommunityToolkit.Mvvm.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Network1.UI.Tarpit.Core.Models;
using Network1.UI.Tarpit.Core.Services;
using NSubstitute;

namespace Network1.UI.Tarpit.Core.ViewModels.Tests
{
	[TestClass]
	public class ConnectionsViewModelTests
	{
		[TestMethod]
		public void Receive_When_OptionsChangedCommandReceived_Expect_ViewOptionsReload()
		{
			IMessenger messenger = Substitute.For<IMessenger>();
			IAppOptionsManager appOptionsManager = Substitute.For<IAppOptionsManager>();
			ConnectionsViewModel connectionsViewModel = new ConnectionsViewModel(messenger, appOptionsManager);

			connectionsViewModel.Receive(null, new AppCommand(AppCommand.OptionsChangedCommand));

			appOptionsManager.Received(2).Load(Arg.Any<ViewOptions>());
		}

		[TestMethod]
		public void Receive_When_ConnectionsReceived_Expect_ConnectionsNotExceedingMaxConnectionEntries()
		{
			IMessenger messenger = Substitute.For<IMessenger>();
			IAppOptionsManager appOptionsManager = Substitute.For<IAppOptionsManager>();
			ConnectionsViewModel connectionsViewModel = new ConnectionsViewModel(messenger, appOptionsManager);
			connectionsViewModel.Options.MaxConnectionEntries = 5;

			for (int i = 0; i < connectionsViewModel.Options.MaxConnectionEntries + 1; i++)
			{
				connectionsViewModel.Receive(null, new[] { new TarpitConnection() { Id = i } });
			}

			Assert.AreEqual(connectionsViewModel.Connections.Count, connectionsViewModel.Options.MaxConnectionEntries);
		}

		[TestMethod]
		public void Receive_When_ExistingConnectionReceived_Expect_ConnectionUpdate()
		{
			IMessenger messenger = Substitute.For<IMessenger>();
			IAppOptionsManager appOptionsManager = Substitute.For<IAppOptionsManager>();
			ConnectionsViewModel connectionsViewModel = new ConnectionsViewModel(messenger, appOptionsManager);
			connectionsViewModel.Options.MaxConnectionEntries = 5;

			TarpitConnection connection1 = new TarpitConnection() { Id = 1 };
			TarpitConnection connection2 = new TarpitConnection() { Id = 1, BytesSent = 10, IsCompleted = true, DurationInSeconds = 10 };

			connectionsViewModel.Receive(null, new[] { connection1 });
			connectionsViewModel.Receive(null, new[] { connection2 });

			Assert.AreEqual(connection1.BytesSent, connection2.BytesSent);
			Assert.AreEqual(connection1.IsCompleted, connection2.IsCompleted);
			Assert.AreEqual(connection1.DurationInSeconds, connection2.DurationInSeconds);
		}
	}
}