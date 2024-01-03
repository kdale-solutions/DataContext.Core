using DataContext.Core.Interfaces.KeyConstraints;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataContext.Core.Utilities.KeyConstraints
{
	public class KeyConstraintConverter<K> : JsonConverter<K> where K : struct, IKeyConstraint
	{
		public override K Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			var methodInfo = typeof(K).GetMethod("Parse");

			if (methodInfo == null) throw new JsonException($"Unable to locate {typeof(K).Name}.Parse(object).");

			return (K)methodInfo.Invoke(null, new object[] { reader.GetInt32() });
		}

		public override void Write(
			Utf8JsonWriter writer,
			K keyConstraint,
			JsonSerializerOptions options)
		{
			writer.WriteStringValue(keyConstraint.ToString());
		}
	}
}
