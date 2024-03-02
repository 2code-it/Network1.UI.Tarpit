using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Network1.UI.Tarpit.Core.Models
{
	public partial class ViewOptions : ObservableValidator
	{
		[ObservableProperty]
		[NotifyDataErrorInfo]
		[Range(1, 1000, ErrorMessage = "Expected value range 1-1000")]
		private int _maxConnectionEntries;
		[ObservableProperty]
		[NotifyDataErrorInfo]
		[Range(1, 1000, ErrorMessage = "Expected value range 1-1000")]
		private int _maxAppLogEntries;
	}
}
