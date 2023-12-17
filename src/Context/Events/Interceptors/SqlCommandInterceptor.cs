using DiagnosticSuite.Logging;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

#pragma warning disable CS8602
namespace DataContext.Core.Context.Events.Interceptors
{
	public class SqlCommandInterceptor : DbCommandInterceptor
	{
		private static Action<string> LogInfo = UtilityLogger<SqlCommandInterceptor>.Info; 
		private static Action<string, Exception> LogError = UtilityLogger<SqlCommandInterceptor>.Error;

		public override InterceptionResult<DbDataReader> ReaderExecuting(
			DbCommand command, 
			CommandEventData eventData, 
			InterceptionResult<DbDataReader> result
			)
		{
			var header = $"ReaderExecuting: {DateTime.Now.ToString("MM.dd.yyyy hh:mm:ss.ffff tt")} -- ContextId: {eventData.Context.ContextId} -- ThreadId: {Thread.CurrentThread.ManagedThreadId}\r\n";

			LogInfo($"{header}{command.CommandText}\r\n");

			return result;
		}

		public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
			DbCommand command,
			CommandEventData eventData,
			InterceptionResult<DbDataReader> result,
			CancellationToken cancellationToken = default
			)
		{
			var header = $"ReaderExecutingAsync: {DateTime.Now.ToString("MM.dd.yyyy hh:mm:ss.ffff tt")} -- ContextId: {eventData.Context.ContextId} -- ThreadId: {Thread.CurrentThread.ManagedThreadId}\r\n";

			LogInfo($"{header}{command.CommandText}\r\n");

			return new ValueTask<InterceptionResult<DbDataReader>>(result);
		}

		public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
		{
			var header = $"CommandFailed: {DateTime.Now.ToString("MM.dd.yyyy hh:mm:ss.ffff tt")} -- ContextId: {eventData.Context.ContextId} -- ThreadId: {Thread.CurrentThread.ManagedThreadId}\r\n";

			LogError($"{header}{command.CommandText}\r\n", eventData.Exception);
		}

		public override Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData, CancellationToken cancellationToken = default)
		{
			var header = $"CommandFailedAsync: {DateTime.Now.ToString("MM.dd.yyyy hh:mm:ss.ffff tt")} -- ContextId: {eventData.Context.ContextId} -- ThreadId: {Thread.CurrentThread.ManagedThreadId}\r\n";

			LogError($"{header}{command.CommandText}\r\n", eventData.Exception);

			return Task.CompletedTask;
		}
	}
}
