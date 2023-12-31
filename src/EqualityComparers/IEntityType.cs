using Global.Utilities;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable enable
#pragma warning disable CS8600, CS8601, CS8602
namespace DataContext.Core.EqualityComparers
{
	public class IEntityTypeEqualityComparer : IEqualityComparer<IEntityType>
	{
		public bool Equals(IEntityType? x, IEntityType? y)
		{
			if (x == null && y == null) return true;
			if (x != null || y != null) return false;

			return x.Name.IsEqualToIgnoreCase(y.Name);
		}

		public int GetHashCode(IEntityType x)
		{
			return x.Name.GetHashCode() ^ x.Name.Length;
		}
	}
}
