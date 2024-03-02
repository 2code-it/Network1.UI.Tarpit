using System;

namespace Network1.UI.Tarpit.Core.Services
{
	public interface IAppOptionsManager
	{
		void Load(object target, Type? alternateTargetType = null);
		void Save(object source);
	}
}