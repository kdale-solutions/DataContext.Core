using DiagnosticSuite.EventHandling.Observers;
using DiagnosticSuite.Logging.PackageApi;
using Global.Configuration;
using Global.Utilities;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataContext.Core.Context.Events
{
	public class DbDiagnosticEventObserver : EventObserverBase<KeyValuePair<string, object>>
	{
		private readonly ILoggingHandler<DbDiagnosticEventObserver> _logger;

		public DbDiagnosticEventObserver()
		{
			_logger = HostUtility.Resolve<ILoggingHandler<DbDiagnosticEventObserver>>();
		}

		/// <summary>
		/// Register Diagnostic Listeners below.
		/// </summary>
		/// <param name="value">
		/// </param>
		public override void OnNext(KeyValuePair<string, object> next)
		{
			if (next.Key.IsEqualTo(CoreEventId.SaveChangesFailed.Name))
			{
				var eventData = ((DbContextErrorEventData)next.Value);

				_logger.Error($"{CoreEventId.SaveChangesFailed.Name}");
			}
		}
	}
}
