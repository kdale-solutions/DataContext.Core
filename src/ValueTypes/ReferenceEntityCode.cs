using DataContext.Core.Interfaces.Entity;
using System.Globalization;

namespace DataContext.Core.ValueTypes
{
    public struct ReferenceEntityCode<K> where K : struct
    {
        private string _value;
        public string Value
        {
            get => _value;
            set => _value = value;
        }

        public ReferenceEntityCode(string value)
        {
            _value = value;
        }

        public static implicit operator ReferenceEntityCode<K>(string value)
        {
            return new ReferenceEntityCode<K>(value);
        }

        public static implicit operator string(ReferenceEntityCode<K> value)
        {
            return value.Value;
        }

        public new ReferenceEntityCode<K> ToString()
        {
            return _value;
        }

        public ReferenceEntityCode<K> ToUpper()
        {
            return _value.ToUpper();
        }

        public ReferenceEntityCode<K> ToUpper(CultureInfo culture)
        {
            return _value.ToUpper(culture);
        }

        public ReferenceEntityCode<K> ToUpperInvariant()
        {
            return _value.ToUpper();
        }

        public ReferenceEntityCode<K> ToLower()
        {
            return _value.ToLower();
        }

        public ReferenceEntityCode<K> ToLower(CultureInfo culture)
        {
            return _value.ToLower(culture);
        }

        public ReferenceEntityCode<K> ToLowerInvariant()
        {
            return _value.ToLowerInvariant();
        }
    }
}
