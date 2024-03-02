namespace Network1.UI.Tarpit.Core.Models
{
	public class AppOptions
	{
		public ViewOptions ViewOptions { get; set; } = new ViewOptions();
		public TarpitOptions TarpitOptions { get; set; } = new TarpitOptions();
		public ConnectionLogOptions ConnectionLogOptions { get; set; } = new ConnectionLogOptions();
	}
}
