using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataContext.Core.Configuration.ValueComparers
{
	public class EnumValueComparer<TEnum> : ValueComparer<TEnum> 
		where TEnum : struct, Enum, IConvertible
	{
		public EnumValueComparer()
			: base(
				  (x, y) => Convert.ToByte(x).Equals(Convert.ToByte(y)),
				  x => x.GetHashCode()
				  )
		{ }
	}
}
