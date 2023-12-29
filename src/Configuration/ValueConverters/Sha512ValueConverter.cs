using DataContext.Core.Utilities.Security;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text;

namespace DataContext.Core.Configuration.ValueConverters
{
	public class Sha512ValueConverter : ValueConverter<string, byte[]>
	{
		public Sha512ValueConverter()
			: base(
				  v => v.HashString(),
				  v => Encoding.ASCII.GetString(v)
				  ) 
		{ }
    }
}
