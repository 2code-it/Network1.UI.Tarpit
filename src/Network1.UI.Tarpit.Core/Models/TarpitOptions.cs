using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Network1.UI.Tarpit.Core.Models
{
	public partial class TarpitOptions : ObservableValidator
	{
		[ObservableProperty]
		[NotifyDataErrorInfo]
		[Required(ErrorMessage = "Local IP address is required")]
		private string? _listenAddress;
		[ObservableProperty]
		[NotifyDataErrorInfo]
		[Required(ErrorMessage = "A list of ports / port ranges is required")]
		private string? _ports;
		[ObservableProperty]
		private bool _useIPv4Only;
		[ObservableProperty]
		[NotifyDataErrorInfo]
		[Range(100, 5000, ErrorMessage = "Delay between writes should be in range of 100-5000")]
		private int _writeIntervalInMs;
		[ObservableProperty]
		[NotifyDataErrorInfo]
		[Range(1, 500, ErrorMessage = "Write size should be in range of 1-500")]
		private int _writeSize;
		[ObservableProperty]
		[NotifyDataErrorInfo]
		[Range(1, 30, ErrorMessage = "Connection status updates should be in range of 1-30")]
		private int _updateIntervalInSeconds;
		[ObservableProperty]
		[NotifyDataErrorInfo]
		[Range(100, 20000, ErrorMessage = "Connection timeout should be in range of 100-20000")]
		private int _timeoutInSeconds;
		[ObservableProperty]
		private string? _responseFile;
		[ObservableProperty]
		private string? _responseText;
	}
}
