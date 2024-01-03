using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Web;

namespace DataContext.Core.Configuration.ValueConverters
{
    public class UrlEncodingValueConverter
    {
        private ValueConverter<string, string> _convertValue;
        public ValueConverter<string, string> ConvertValue => _convertValue;

        public UrlEncodingValueConverter()
        {
            _convertValue = new ValueConverter<string, string>
            (
                v => HttpUtility.UrlEncode(v),
                v => v
            );
        }
    }
}
