using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Web;

namespace DataContext.Core.Configuration.ValueConverters
{
    public class HtmlEncodingValueConverter
    {
        private ValueConverter<string, string> _convertValue;

        public ValueConverter<string, string> ConvertValue => _convertValue;

        public HtmlEncodingValueConverter()
        {
            _convertValue = new ValueConverter<string, string>
            (
                v => HttpUtility.HtmlEncode(v),
                v => v
            );
        }
    }
}
