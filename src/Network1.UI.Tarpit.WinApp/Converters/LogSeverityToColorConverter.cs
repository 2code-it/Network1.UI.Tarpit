using System;
using System.Globalization;
using System.Windows.Data;

namespace Network1.UI.Tarpit.WinApp.Converters
{
	public class LogSeverityToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string severity = (string)value;
			return true switch
			{
				true when severity == "error" => "#FF0000",
				true when severity == "warn" => "##FFEE60",
				_ => "#000000"
			};
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
