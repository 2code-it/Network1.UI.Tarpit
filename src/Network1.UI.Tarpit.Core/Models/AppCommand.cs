namespace Network1.UI.Tarpit.Core.Models
{
	public class AppCommand
	{
		public AppCommand(string name) { Name = name; }

		public const string ShowOptionsViewCommand = "ShowOptionsView";
		public const string HideOptionsViewCommand = "HideOptionsView";
		public const string OptionsChangedCommand = "OptionsChanged";

		public string Name { get; set; }
		public string? Parameter { get; set; }
	}
}
