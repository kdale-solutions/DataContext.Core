namespace DataContext.Core.Interfaces.Entity
{
    public interface IDisplayOrder<T> where T : struct
	{
		T? DisplayOrder { get; set; }
	}
}
