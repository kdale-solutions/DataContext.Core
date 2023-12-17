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

    //public static class AddressTypeKeyUtility<T> where T : ReferenceEntity
    //{
    //    public static ValueConverter<AddressTypeKey<T>, byte> ConvertValue => new ValueConverter<AddressTypeKey<T>, byte>
    //        (
    //            v => (byte)v,
    //            v => new AddressTypeKey<T>(v)
    //        );

    //    public static ValueComparer<AddressTypeKey<T>> Compare => new ValueComparer<AddressTypeKey<T>>(
    //        (x, y) => x == y,
    //        x => x.GetHashCode(),
    //        x => x
    //        );
    //}
}
