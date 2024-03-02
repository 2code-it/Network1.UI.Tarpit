using System;
using System.Globalization;
using System.Windows.Data;

namespace Network1.UI.Tarpit.WinApp.Converters
{
	public class CompletedToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool completed = (bool)value;
			return completed ? "#888888" : "#000000";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
