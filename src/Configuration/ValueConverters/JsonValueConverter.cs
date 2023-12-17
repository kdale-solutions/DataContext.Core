using Global.Utilities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace DataContext.Core.Configuration.ValueConverters
{
	public class JsonValueConverter<T> : ValueConverter<T, string> where T : class
    {
        public JsonValueConverter()
            : base(
				  v => JsonSerializer.Serialize(v, InternalJsonSerializerOptions.Default),
				  v => JsonSerializer.Deserialize<T>(v, InternalJsonSerializerOptions.Default)
				  ) { }
    }
}
