
using System;
namespace Soulgame.StateManagement
{
	public interface IAutoOpenState
	{
		bool CanOpen();
		
		bool AutoClear();
	}
}

