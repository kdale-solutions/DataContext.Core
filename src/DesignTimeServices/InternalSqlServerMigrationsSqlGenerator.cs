using DataContext.Core.Utilities.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Update;

#nullable enable
namespace DataContext.Core.DesignTimeServices
{
	public class InternalSqlServerMigrationsSqlGenerator : SqlServerMigrationsSqlGenerator
	{
		public InternalSqlServerMigrationsSqlGenerator(
			MigrationsSqlGeneratorDependencies dependencies,
			ICommandBatchPreparer commandBatchPreparer
			) : base(
				dependencies, 
				commandBatchPreparer
				) { }

		protected override void Generate(
			CreateTableOperation operation,
			IModel? model,
			MigrationCommandListBuilder builder,
			bool terminate = true
			)
		{
			var sqlScripts = LoadScriptUtility.GetSqlFiles(operation.Name);

			if (sqlScripts == null)
			{
				base.Generate(operation, model, builder, terminate);
			}
			else
			{
				var preCreatePath = sqlScripts.FirstOrDefault(x => x.EndsWith(".pre.sql", StringComparison.OrdinalIgnoreCase));

				if (preCreatePath != null) 
				{
					Generate(preCreatePath, builder);
				}

				base.Generate(operation, model, builder, terminate);

				var postCreatePath = sqlScripts.FirstOrDefault(x => x.EndsWith(".post.sql", StringComparison.OrdinalIgnoreCase));

				if (postCreatePath != null)
				{
					Generate(postCreatePath, builder);
				}
			}
		}

		private void Generate(string filePath, MigrationCommandListBuilder builder)
		{
			var sql = LoadScriptUtility.GetSql(filePath);

			if (sql == string.Empty) return;

			builder.AppendLines(sql);
			builder.AppendLine("GO");
			builder.AppendLine(string.Empty);
		}
	}
}
