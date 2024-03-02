using Code2.Net.TcpTarpit;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Network1.UI.Tarpit.Core.Models;
using NSubstitute;

namespace Network1.UI.Tarpit.Core.Services.Tests
{
	[TestClass]
	public class TarpitServiceManagerTests
	{
		private ITarpitService _tarpitService = Substitute.For<ITarpitService>();
		private IAppLogService _appLogService = Substitute.For<IAppLogService>();
		private IMessenger _messenger = Substitute.For<IMessenger>();
		private IObjectMapper _objectMapper = Substitute.For<IObjectMapper>();
		private IAppOptionsManager _appOptionsManager = Substitute.For<IAppOptionsManager>();

		[TestMethod]
		public void StartService_When_ListenersCountIsZero_Expect_AppLogWarning()
		{
			ResetSubstitudes();
			TarpitServiceManager tarpitServiceManager = new TarpitServiceManager(_messenger, _tarpitService, _appOptionsManager, _appLogService, _objectMapper);
			_tarpitService.Start().Returns(0);

			tarpitServiceManager.StartService();

			_appLogService.Received(1).WriteWarning(Arg.Any<string>());
		}

		[TestMethod]
		public void StartService_When_ListenersCountIsNotZero_Expect_AppLogInfo()
		{
			ResetSubstitudes();
			TarpitServiceManager tarpitServiceManager = new TarpitServiceManager(_messenger, _tarpitService, _appOptionsManager, _appLogService, _objectMapper);
			_tarpitService.Start().Returns(1);

			tarpitServiceManager.StartService();

			_appLogService.Received(1).WriteInfo(Arg.Any<string>());
		}

		[TestMethod]
		public void StopService_When_ListenersCountIsZero_Expect_AppLogWarning()
		{
			ResetSubstitudes();
			TarpitServiceManager tarpitServiceManager = new TarpitServiceManager(_messenger, _tarpitService, _appOptionsManager, _appLogService, _objectMapper);
			_tarpitService.ListenersCount.Returns(0);

			tarpitServiceManager.StopService();

			_appLogService.Received(1).WriteWarning(Arg.Any<string>());
		}

		[TestMethod]
		public void StopService_When_ListenersCountIsNotZero_Expect_AppLogInfo()
		{
			ResetSubstitudes();
			TarpitServiceManager tarpitServiceManager = new TarpitServiceManager(_messenger, _tarpitService, _appOptionsManager, _appLogService, _objectMapper);
			_tarpitService.ListenersCount.Returns(1);

			tarpitServiceManager.StopService();

			_tarpitService.Received(1).Stop();
			_appLogService.Received(1).WriteInfo(Arg.Any<string>());
		}

		[TestMethod]
		public void OnTarpitServiceError_When_EventOccurs_Expect_AppLogError()
		{
			ResetSubstitudes();
			TarpitServiceManager tarpitServiceManager = new TarpitServiceManager(_messenger, _tarpitService, _appOptionsManager, _appLogService, _objectMapper);
			_tarpitService.Start().Returns(1);

			tarpitServiceManager.StartService();
			_tarpitService.Error += Raise.EventWith(null, new UnhandledExceptionEventArgs(new Exception(), false));

			_appLogService.Received(1).WriteError(Arg.Any<string>());
		}

		[TestMethod]
		public void OnTarpitServiceConnectionsUpdated_When_EventOccurs_Expect_MessengerMessage()
		{
			ResetSubstitudes();
			TarpitServiceManager tarpitServiceManager = new TarpitServiceManager(_messenger, _tarpitService, _appOptionsManager, _appLogService, _objectMapper);
			_tarpitService.Start().Returns(1);
			_objectMapper.Map<TarpitConnection>(Arg.Any<object>()).Returns(new TarpitConnection());

			tarpitServiceManager.StartService();
			_tarpitService.ConnectionsUpdated += Raise.EventWith(null, new ConnectionsUpdatedEventArgs(new[] { new ConnectionStatus() }));

			_messenger.Received(1).Send(Arg.Any<TarpitConnection[]>(), Arg.Any<int>());
		}

		private void ResetSubstitudes()
		{
			_tarpitService = Substitute.For<ITarpitService>();
			_appLogService = Substitute.For<IAppLogService>();
			_messenger = Substitute.For<IMessenger>();
			_objectMapper = Substitute.For<IObjectMapper>();
			_appOptionsManager = Substitute.For<IAppOptionsManager>();
		}
	}
}