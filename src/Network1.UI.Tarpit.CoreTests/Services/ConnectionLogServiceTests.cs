using CommunityToolkit.Mvvm.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Network1.UI.Tarpit.Core.Models;
using Network1.UI.Tarpit.Core.Services.Internals;
using NSubstitute;

namespace Network1.UI.Tarpit.Core.Services.Tests
{
	[TestClass]
	public class ConnectionLogServiceTests
	{
		[TestMethod]
		[DataRow(true, true)]
		[DataRow(true, false)]
		[DataRow(false, true)]
		[DataRow(false, false)]
		public void Receive_When_LogOptionIsEnabledAndConnectionCompleted_Expect_LogFileWrite(bool isEnabled, bool isCompleted)
		{
			IFileSystem fileSystem = Substitute.For<IFileSystem>();
			IMessenger messenger = Substitute.For<IMessenger>();
			IAppOptionsManager appOptionsManager = Substitute.For<IAppOptionsManager>();

			ConnectionLogService connectionLogService = new ConnectionLogService(messenger, appOptionsManager, fileSystem);
			connectionLogService.Options.IsEnabled = isEnabled;

			connectionLogService.Receive(null, new[] { new TarpitConnection { IsCompleted = isCompleted } });
			int expectedReceived = isEnabled && isCompleted ? 1 : 0;
			fileSystem.Received(expectedReceived).FileAppendAllLines(Arg.Any<string>(), Arg.Any<IEnumerable<string>>());
		}

		[TestMethod]
		public void Receive_When_NonExistingLogDirectory_Expect_LogDirectoryCreate()
		{
			IFileSystem fileSystem = Substitute.For<IFileSystem>();
			IMessenger messenger = Substitute.For<IMessenger>();
			IAppOptionsManager appOptionsManager = Substitute.For<IAppOptionsManager>();

			fileSystem.DirectoryExists(Arg.Any<string>()).Returns(x => false);

			ConnectionLogService connectionLogService = new ConnectionLogService(messenger, appOptionsManager, fileSystem);
			connectionLogService.Options.IsEnabled = true;
			connectionLogService.Receive(null, new[] { new TarpitConnection { IsCompleted = true } });

			fileSystem.Received(1).DirectoryCreateDirectory(Arg.Any<string>());
		}

		[TestMethod]
		public void Receive_When_FileFormatOptionsSet_Expect_FileNameAccordingToOptions()
		{
			string filePrefix = "log_";
			string fileDateSuffix = "yyyyMMdd";
			IFileSystem fileSystem = Substitute.For<IFileSystem>();
			IMessenger messenger = Substitute.For<IMessenger>();
			IAppOptionsManager appOptionsManager = Substitute.For<IAppOptionsManager>();

			string fileName = string.Empty;
			fileSystem.DirectoryExists(Arg.Any<string>()).Returns(x => true);
			fileSystem.PathCombine(Arg.Do<string[]>(x => fileName = x[1]));

			string fileNameExpected = $"{filePrefix}{DateTime.Now.ToString(fileDateSuffix)}.txt";

			ConnectionLogService connectionLogService = new ConnectionLogService(messenger, appOptionsManager, fileSystem);
			connectionLogService.Options.IsEnabled = true;
			connectionLogService.Options.FileNamePrefix = filePrefix;
			connectionLogService.Options.FileNameSuffixDateFormat = fileDateSuffix;

			connectionLogService.Receive(null, new[] { new TarpitConnection { IsCompleted = true } });

			Assert.AreEqual(fileNameExpected, fileName);
		}

		[TestMethod]
		public void Receive_When_LogLineOptionsSet_Expect_LogLineAccordingToOptions()
		{
			char logDelimiter = '_';
			string logDateFormat = "yyyyMMddTHH:mm:ss";
			DateTime connectionCreated = DateTime.Now;

			IFileSystem fileSystem = Substitute.For<IFileSystem>();
			IMessenger messenger = Substitute.For<IMessenger>();
			IAppOptionsManager appOptionsManager = Substitute.For<IAppOptionsManager>();

			string logLineActual = string.Empty;
			fileSystem.FileAppendAllLines(Arg.Any<string>(), Arg.Do<IEnumerable<string>>(x => logLineActual = x.First()));

			string logLineExpected = $"{connectionCreated.ToString(logDateFormat)}{logDelimiter}{logDelimiter}{logDelimiter}0{logDelimiter}0";

			ConnectionLogService connectionLogService = new ConnectionLogService(messenger, appOptionsManager, fileSystem);
			connectionLogService.Options.IsEnabled = true;
			connectionLogService.Options.LogValueDelimiter = logDelimiter;
			connectionLogService.Options.LogDateFormat = logDateFormat;

			connectionLogService.Receive(null, new[] { new TarpitConnection { IsCompleted = true, Created = connectionCreated } });

			Assert.AreEqual(logLineExpected, logLineActual);
		}
	}
}