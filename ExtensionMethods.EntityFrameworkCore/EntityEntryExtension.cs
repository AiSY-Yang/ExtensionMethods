using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#if NETCOREAPP3_0_OR_GREATER
namespace ExtensionMethods.EntityFrameworkCore
{
	public static class EntityEntryExtension
	{
		public static void Detached(this IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry> entities)
		{
			entities.ToList().ForEach(e => e.State = Microsoft.EntityFrameworkCore.EntityState.Detached);
		}
	}
}

#endif