using Global.Utilities;

#nullable enable
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
				_filePathBase = $"{appDomainDir}..\\..\\..\\..\\Migrations.Configuration\\Entities\\";
			}
		}

		public static string GetSql(string entityName)
		{
			var fileInfo = Directory.GetFiles(_filePathBase, $"*{entityName}.sql", _enumerationOptions);

			if (fileInfo.IsNullOrEmpty()) return string.Empty;

			using var fileStream = File.OpenText(fileInfo[0]);

			return fileStream.ReadToEnd();
		}
	}
}
