using DataContext.Core.Configuration.Constants;
using Global.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

#pragma warning disable EF1001
namespace DataContext.Core.DesignTimeServices
{
	public class MigrationHistoryRepository : SqlServerHistoryRepository
    {
        private readonly string _notes;
        private readonly string _userName;

        public MigrationHistoryRepository(HistoryRepositoryDependencies dependencies)
            : base(dependencies)
        {
            _userName = HostUtility.Configuration["--username"];

            _notes = HostUtility.Configuration["--notes"];
        }

        protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history)
        {
            base.ConfigureTable(history);

            history.Property<short>("Id")
                .IsRequired()
                .HasColumnType(DataConstants.ColumnType.Smallint)
                .UseIdentityColumn()
                .HasColumnOrder(DataConstants.ColumnOrder.First);

            history.HasKey("Id");

            history.Property<string>("UserName")
                .HasColumnType(DataConstants.ColumnType.Varchar)
                .HasMaxLength(DataConstants.MaxLength.StandardName)
                .IsUnicode(false);

            history.Property<string>("Notes")
                .HasColumnType(DataConstants.ColumnType.Varchar)
                .HasMaxLength(DataConstants.MaxLength.Description)
                .IsUnicode(false);

            history.Property(x => x.MigrationId)
                .IsUnicode(false);

            history.Property(x => x.ProductVersion)
                .IsUnicode(false);
        }

        public override string GetInsertScript(HistoryRow row)
        {
            return $"INSERT INTO [{DataConstants.Migration.HistoryTableName}]([MigrationId],[ProductVersion],[Notes],[UserName])\r\nVALUES('{row.MigrationId}','{row.ProductVersion}','{_notes ?? null}','{_userName}');\r\n";
        }
    }
}
