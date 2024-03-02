using CommunityToolkit.Mvvm.ComponentModel;
using Network1.UI.Tarpit.Core.Services;

namespace Network1.UI.Tarpit.Core.ViewModels
{
	public partial class MetricsViewModel : ObservableObject
	{
		public MetricsViewModel(IConnectionLogService connectionLogService)
		{
			_connectionLogService = connectionLogService;
		}

		private readonly IConnectionLogService _connectionLogService;
	}
}
