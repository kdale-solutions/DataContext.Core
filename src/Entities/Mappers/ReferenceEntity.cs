using Global.Models;
using Global.ValueTypes;
using System.Linq.Expressions;

namespace DataContext.Core.Entities
{
	public abstract partial class ReferenceEntity<K>
		where K : struct, IConvertible
	{
		public static Expression<Func<ReferenceEntity<K>, IdCodeNameModel<K>>> ToIdCodeNameModel = (x) => new IdCodeNameModel<K>
		{
			Id = x.Id,
			Code = x.Code,
			Name = x.Name
		};
	}
}
