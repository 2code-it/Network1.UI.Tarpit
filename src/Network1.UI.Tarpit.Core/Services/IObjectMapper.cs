namespace Network1.UI.Tarpit.Core.Services
{
	public interface IObjectMapper
	{
		T Map<T>(object source) where T : new();

		void Map(object source, object target);

	}
}