using DataContext.Core.Interfaces.KeyConstraints;
using DataContext.Core.Utilities.KeyConstraints;
using DataContext.Core.ValueTypes;
using System.Globalization;
using System.Text.Json.Serialization;

#nullable enable
#pragma warning disable CS8605
namespace DataContext.Core.Entities
{
	[JsonConverter(typeof(KeyConstraintConverter<KeyConstraintPlaceholder>))]
	public readonly struct KeyConstraintPlaceholder : IKeyConstraint
	{
		public ReferenceEntityCode<KeyConstraintPlaceholder> Value
		{
			get => ReferenceEntityUtility.GetCodeById<KeyConstraintPlaceholder>(_providerKey);
			init => _providerKey = ReferenceEntityUtility.GetIdByCode<KeyConstraintPlaceholder>(value);
		}

		private readonly byte _providerKey;
		public readonly byte ProviderKey => _providerKey;

		public KeyConstraintPlaceholder(bool isDefault = true)
		{
			this._providerKey = default(byte);
		}

		public KeyConstraintPlaceholder(ReferenceEntityCode<KeyConstraintPlaceholder> value)
		{
			this._providerKey = ReferenceEntityUtility.GetIdByCode<KeyConstraintPlaceholder>(value.Value);
		}

		public KeyConstraintPlaceholder(byte value)
		{
			this._providerKey = value;
		}

		#region Object overrides/other methods

		public static KeyConstraintPlaceholder Parse(object input)
		{
			return new KeyConstraintPlaceholder((byte)Convert.ChangeType(input, typeof(byte)));
		}

		public bool HasValue()
		{
			return !(this.Value != default ^ this._providerKey != default);
		}

		public override bool Equals(object? obj)
		{
			return !(obj != null ^ this._providerKey == ((KeyConstraintPlaceholder)obj)._providerKey);
		}

		public override int GetHashCode()
		{
			return this._providerKey.GetHashCode() ^ this.Value.Value.GetHashCode();
		}

		public override string ToString()
		{
			return this.Value;
		}

		public static implicit operator KeyConstraintPlaceholder(byte value)
		{
			return new KeyConstraintPlaceholder(value);
		}

		public static implicit operator byte(KeyConstraintPlaceholder value)
		{
			return value.ProviderKey;
		}

		public static implicit operator KeyConstraintPlaceholder(ReferenceEntityCode<KeyConstraintPlaceholder> value)
		{
			return new KeyConstraintPlaceholder(value);
		}

		public static implicit operator ReferenceEntityCode<KeyConstraintPlaceholder>(KeyConstraintPlaceholder value)
		{
			return value.Value;
		}

		public static bool operator ==(KeyConstraintPlaceholder left, KeyConstraintPlaceholder right)
		{
			return left._providerKey == right._providerKey;
		}

		public static bool operator !=(KeyConstraintPlaceholder left, KeyConstraintPlaceholder right)
		{
			return left._providerKey != right._providerKey;
		}

		public KeyConstraintPlaceholder ToUpper()
		{
			return this.Value.ToUpper();
		}

		public KeyConstraintPlaceholder ToUpper(CultureInfo culture)
		{
			return this.Value.ToUpper(culture);
		}

		public KeyConstraintPlaceholder ToUpperInvariant()
		{
			return this.Value.ToUpper();
		}

		public KeyConstraintPlaceholder ToLower()
		{
			return this.Value.ToLower();
		}

		public KeyConstraintPlaceholder ToLower(CultureInfo culture)
		{
			return this.Value.ToLower(culture);
		}

		public KeyConstraintPlaceholder ToLowerInvariant()
		{
			return this.Value.ToLowerInvariant();
		}

		#endregion

		#region IConvertible

		public TypeCode GetTypeCode() => Type.GetTypeCode(this.GetType());
		public bool ToBoolean(IFormatProvider? provider) => this._providerKey != default;
		public byte ToByte(IFormatProvider? provider) => this._providerKey;
		public char ToChar(IFormatProvider? provider) => throw new InvalidCastException(InvalidCastExceptionMessage(typeof(char).Name));
		public DateTime ToDateTime(IFormatProvider? provider) => throw new InvalidCastException(InvalidCastExceptionMessage(typeof(DateTime).Name));
		public decimal ToDecimal(IFormatProvider? provider) => Convert.ToDecimal(this._providerKey);
		public double ToDouble(IFormatProvider? provider) => Convert.ToDouble(this._providerKey);
		public short ToInt16(IFormatProvider? provider) => Convert.ToInt16(this._providerKey);
		public int ToInt32(IFormatProvider? provider) => Convert.ToInt32(this._providerKey);
		public long ToInt64(IFormatProvider? provider) => Convert.ToInt64(this._providerKey);
		public sbyte ToSByte(IFormatProvider? provider) => Convert.ToSByte(this._providerKey);
		public float ToSingle(IFormatProvider? provider) => Convert.ToSingle(this._providerKey);
		public string ToString(IFormatProvider? provider) => this.ToString();
		public object ToType(Type conversionType, IFormatProvider? provider) => Convert.ChangeType(this, conversionType, provider);
		public ushort ToUInt16(IFormatProvider? provider) => Convert.ToUInt16(this._providerKey);
		public uint ToUInt32(IFormatProvider? provider) => Convert.ToUInt32(this._providerKey);
		public ulong ToUInt64(IFormatProvider? provider) => Convert.ToUInt64(this._providerKey);

		private static string InvalidCastExceptionMessage(string castToType) =>
			$"Unable to cast KeyConstraintPlaceholder to {castToType}.";

		#endregion
	}
}

