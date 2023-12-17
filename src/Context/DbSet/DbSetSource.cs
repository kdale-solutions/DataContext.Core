using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Concurrent;

#pragma warning disable EF1001, CS8603
namespace DataContext.Core.Context.DbSet
{
	public class DbSetSource<TContext> : DbSetSource where TContext : DbContext
	{
		private TContext _context;
		private readonly Type _contextType;

		private ConcurrentDictionary<string, ConcurrentDictionary<string, object>> _dbSetCache;

		public DbSetSource(TContext context)
			: base()
		{
			_context = context;
			_contextType = typeof(TContext);
			_dbSetCache = new ConcurrentDictionary<string, ConcurrentDictionary<string, object>>();
		}

		public InternalDbSet<TEntity> Create<TEntity>(Func<TContext, object> newSetFn)
			where TEntity : class
		{
			var _dbSets = GetContextCache();

			var entityType = typeof(TEntity);

			if (_dbSets.TryGetValue(entityType.Name, out var _set))
			{
				return _set as InternalDbSet<TEntity>;
			}
			else
			{
				var _newSet = newSetFn(_context);

				_dbSets.TryAdd(entityType.Name, _newSet);

				return _newSet as InternalDbSet<TEntity>;
			}
		}

		private ConcurrentDictionary<string, object> GetContextCache()
		{
			if (_dbSetCache.TryGetValue(_contextType.Name, out var _dbSets))
			{
				return _dbSets;
			}
			else
			{
				if (_dbSetCache.TryAdd(_contextType.Name, new ConcurrentDictionary<string, object>()))
				{
					return _dbSetCache[_contextType.Name];
				}
			}

			throw new Exception();
		}

		public override object Create(DbContext context, Type type)
		{
			return base.Create(context, type);
		}
	}
}
