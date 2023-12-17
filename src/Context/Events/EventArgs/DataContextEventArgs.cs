namespace DataContext.Core.Context.Events.EventArg
{
	public abstract class DataContextEventArgs<TContext> : EventArgs where TContext : DbContext
	{
		public TContext Context { get; private set; }

		public DataContextEventArgs(TContext context)
		{
			Context = context;
		}
	}
}
