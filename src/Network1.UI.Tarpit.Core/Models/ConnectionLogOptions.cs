using CommunityToolkit.Mvvm.ComponentModel;

namespace Network1.UI.Tarpit.Core.Models
{
	public partial class ConnectionLogOptions : ObservableObject
	{
		[ObservableProperty]
		private bool _isEnabled;
		[ObservableProperty]
		private string _logDateFormat = "yyyyMMddTHH:mm:ss";
		[ObservableProperty]
		private char _logValueDelimiter = '\t';
		[ObservableProperty]
		private string _directoryPath = "./";
		[ObservableProperty]
		private string? _fileNamePrefix;
		[ObservableProperty]
		private string? _fileNameSuffixDateFormat;
	}
}
