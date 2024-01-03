using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.RegularExpressions;

namespace DataContext.Core.Configuration.ValueConverters
{
    public class HexColorCodeValueConverter
    {
        private Regex _validHexColorCodePattern = new Regex(@"^#?[0-9A-F]{6}/i");

        private ValueConverter<string, string> _convertValue;
        public ValueConverter<string, string> ConvertValue => _convertValue;

        public HexColorCodeValueConverter()
        {
            _convertValue = new ValueConverter<string, string>
            (
                v => _validHexColorCodePattern.IsMatch(v) && v.Length == 7
                    ? v.Substring(1) : v,
                v => v.ToString().Insert(0, "#")
            );
        }
    }
}
