using System;
using System.Linq;
using System.Reflection;

namespace Network1.UI.Tarpit.Core.Services
{
	public class ObjectMapper : IObjectMapper
	{
		public T Map<T>(object source) where T : new()
		{
			T result = Activator.CreateInstance<T>()!;
			Map(source, result);
			return result;
		}

		public void Map(object source, object target)
		{
			PropertyInfo[] propertiesTarget = target.GetType().GetProperties().Where(x => x.CanWrite && ValueTypeOrStringFilter(x)).ToArray();
			PropertyInfo[] propertiesSource = source.GetType().GetProperties().Where(x => x.CanRead && ValueTypeOrStringFilter(x)).ToArray();
			foreach (PropertyInfo propertyTarget in propertiesTarget)
			{
				PropertyInfo? propertyIn = propertiesSource.FirstOrDefault(x => x.Name == propertyTarget.Name && x.PropertyType == propertyTarget.PropertyType);
				if (propertyIn is null) continue;
				propertyTarget.SetValue(target, propertyIn.GetValue(source));
			}
		}

		private bool ValueTypeOrStringFilter(PropertyInfo property)
		{
			Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
			return type.IsValueType || type == typeof(string);
		}

	}
}
