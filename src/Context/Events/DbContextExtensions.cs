using DataContext.Core.EqualityComparers;
using System.Collections.Concurrent;

namespace DataContext.Core.Utilities
{
	public static class DbContextExtensions
	{
		private static ConcurrentDictionary<DbContextId, CancellationTokenSource> _cancellationTokens;

		static DbContextExtensions()
		{
			_cancellationTokens = new ConcurrentDictionary<DbContextId, CancellationTokenSource>(new DbContextIdEqualityComparer());
		}

		public static CancellationToken GetCancellationToken(this DbContext dbContext)
		{
			var cancellationToken = new CancellationToken();
			var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			
			if (!_cancellationTokens.TryGetValue(dbContext.ContextId, out var _tokenSource))
			{
				_cancellationTokens.TryAdd(dbContext.ContextId, cancellationTokenSource);
			}
			else
			{
				_cancellationTokens.TryUpdate(dbContext.ContextId, cancellationTokenSource, _tokenSource);
			}

			return cancellationToken;
		}

		public static Task RequestCancellation(this DbContext dbContext)
		{
			if (_cancellationTokens.TryGetValue(dbContext.ContextId, out var _tokenSource))
			{
				return _tokenSource.CancelAsync();
			}

			return Task.FromResult(0);
		}
	}
}
