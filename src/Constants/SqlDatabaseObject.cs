namespace DataContext.Core.Configuration.Constants
{
	public class SqlDatabaseObject
	{
		public const string Schema = "dbo";

		public class UserDefinedType
        {
            public const string StreetDataTable = $"{Schema}.StreetData";
			public const string AddressDataTable = $"{Schema}.AddressData";
		}

        public class StoredProcedure
        {
            public const string AddressInsert = $"{Schema}.AddressInsert";
        }
    }
}
