namespace DataContext.Core.EqualityComparers
{
	public class DbContextIdEqualityComparer : IEqualityComparer<DbContextId>
	{
		public bool Equals(DbContextId x, DbContextId y)
		{
			return x.Equals(y);
		}

		public int GetHashCode(DbContextId x)
		{
			return x.GetHashCode();
		}
	}
}
