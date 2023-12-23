using Global.Utilities;

#nullable disable
namespace DataContext.Core.Utilities.Migrations
{
	public static class LoadScriptUtility
	{
		private static readonly string _filePathBase = string.Empty;

		private static readonly EnumerationOptions _enumerationOptions = new()
		{
			MatchType = MatchType.Simple,
			MatchCasing = MatchCasing.CaseInsensitive,
			MaxRecursionDepth = 8,
			RecurseSubdirectories = true,
			ReturnSpecialDirectories = false
		};

		static LoadScriptUtility()
		{
			var appDomainDir = AppDomain.CurrentDomain.BaseDirectory;

			if (!string.IsNullOrEmpty(appDomainDir))
			{
				_filePathBase = $"{appDomainDir}..\\..\\..\\..\\";
			}
		}

		public static string[] GetSqlFiles(string entityName)
		{
			return Directory.GetFiles(_filePathBase, $"*{entityName}.sql", _enumerationOptions);
		}

		public static string GetSql(string filePath)
		{
			using var fileStream = File.OpenText(filePath);

			return fileStream.ReadToEnd();
		}
	}
}
