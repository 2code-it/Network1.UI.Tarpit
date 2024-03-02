using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Network1.UI.Tarpit.Core.Models
{
	public partial class TarpitConnection : ObservableObject
	{
		public int Id { get; set; }
		public DateTime Created { get; set; }
		public string LocalEndPoint { get; set; } = string.Empty;
		public string RemoteEndPoint { get; set; } = string.Empty;

		[ObservableProperty]
		private int _bytesSent;
		[ObservableProperty]
		private bool _isCompleted;
		[ObservableProperty]
		private int _durationInSeconds;
	}
}
