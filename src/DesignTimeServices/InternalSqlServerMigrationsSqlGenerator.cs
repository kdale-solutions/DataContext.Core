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
			base.Generate(operation, model, builder, terminate);

			Generate(operation, builder);
		}

		private void Generate(ITableMigrationOperation operation, MigrationCommandListBuilder builder)
		{
			var sql = LoadScriptUtility.GetSql(operation.Table);

			if (sql == string.Empty) return;

			builder.AppendLines(sql);
			builder.AppendLine("GO");
			builder.AppendLine(string.Empty);
		}
	}
}
