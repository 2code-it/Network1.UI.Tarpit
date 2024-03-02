using System.Collections.Generic;

namespace Network1.UI.Tarpit.Core.Mvvm
{
	public class ViewModelLocator
	{
		private static IDictionary<string, object> _registeredViewModels = new Dictionary<string, object>();

		public static void RegisterViewModel(object viewModel)
		{
			_registeredViewModels.Add(viewModel.GetType().Name, viewModel);
		}

		public object? this[string viewModelTypeName]
		{
			get
			{
				return _registeredViewModels[viewModelTypeName];
			}
		}
	}
}
