using DataContext.Core.Configuration.Constants;
using Global.Utilities;
using Global.ValueTypes;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace DataContext.Core.Utilities.KeyConstraints
{
	public static class ReferenceEntityUtility
    {
        private static readonly string _referenceEntityDirectoryPath;

        private static ConcurrentDictionary<string, object> _referenceEntityModelCache;
        
        private static JsonSerializerOptions _jsonSerializerOptions;

        static ReferenceEntityUtility()
        {
            _referenceEntityDirectoryPath = $"{
                Environment.GetEnvironmentVariable(DataConstants.DataContextPathVar)
                }DataModel.Configuration\\Entities\\Reference\\";

			_referenceEntityModelCache = new ConcurrentDictionary<string, object>();
            _jsonSerializerOptions = InternalJsonSerializerOptions.Default;
		}

		public static K GetIdByCode<K>(string code) where K : struct, IConvertible => 
            GetAllRecords<K>()
            .FirstOrDefault(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase))
            .Id;

        public static string GetCodeById<K>(K id) where K : struct, IConvertible => 
            GetAllRecords<K>()
            .FirstOrDefault(x => x.Id.Equals(id))
            .Code;

        private static string GetJsonFilePath(string entityName) => 
            $"{_referenceEntityDirectoryPath}{entityName}.json";

		public static ReadOnlyCollection<IdCodeNameModel<K>> GetAllRecords<K>() where K : struct, IConvertible
		{
            var keyName = typeof(K).Name;
            var typeName = keyName.Substring(0, keyName.Length - 2);

            if (_referenceEntityModelCache.TryGetValue(typeName, out var _idCodeNameModels))
            {
                return (ReadOnlyCollection<IdCodeNameModel<K>>)_idCodeNameModels;
			}

            using var jsonDocument = JsonDocument.Parse(File.OpenRead(GetJsonFilePath(typeName)));
            
			var res = JsonSerializer.Deserialize<List<IdCodeNameModel<K>>>(jsonDocument.RootElement.GetProperty("records"), _jsonSerializerOptions).AsReadOnly();

            _referenceEntityModelCache.TryAdd(typeName, res);

            return res;
        }
    }
}
