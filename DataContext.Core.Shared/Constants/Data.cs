namespace DataContext.Core.Configuration.Constants
{
	public class DataConstants
	{
		public const string ModuleRoot = "ModuleRootFullName";

		public const string DataContextPathVar = "DataContextPath";
		public const string DataContextNameVar = "DataContextName";

        public class ConnectionString
        {
            public const string SqlServer = "SqlServer";
        }

		public class Migration
		{
			public const string HistoryTableName = "MigrationHistory";
		}

		public class Annotation
        {
            public const string Html = "Html";
            public const string Unicode = "Unicode";
            public const string Url = "Url";
        }

        public class Discriminator
        {
            public const string EntityType = "EntityType";
        }

        public class ColumnType
        {
            public const string Tinyint = "tinyint"; // 0 to 255
            public const string Smallint = "smallint"; // 256 to 32,767
			public const string Int = "int"; // 32,768 to 2,147,483,647
			public const string Bigint = "bigint"; // 2,147,483,648 to 9,223,372,036,854,775,807
			public const string Decimal = "decimal";
            public const string Char = "char";
			public const string Nchar = "nchar";
			public const string Varchar = "varchar";
			public const string DateTime = "datetime";
			public const string Datetime2 = "datetime2";
			public const string SmallDateTime = "smalldatetime";
			public const string Time = "time";
			public const string Binary = "binary";
			public const string Varbinary = "varbinary";
			public const string Rowversion = "rowversion";
		}

		public class ColumnOrder
		{
            public const int First = 0;
			public const int Last = 199;
		}

		public class MaxLength
        {
            public const int Char1 = 1;
            public const int Char2 = 2;
            public const int Char3 = 3;

            public class Max
            {
                public const int SqlServer = 8000;
            }

			public const int FileExtension = 4;
			public const int CreditCardLastFourDigits = 4;
			public const int ReferenceEntityCode = 5;
            public const int ReferenceEntityName = 50;
            public const int ReferenceEntityDescription = 80;

            public const int ISO3166Alpha2 = 10;
            public const int ISO3166Numeric = 10;

            public const int FirstName = 20;
            public const int LastName = 30;
            public const int ShortName = 40;
            public const int FullName = 80;
            public const int StandardName = 80;
            public const int ProductName = 80;
			public const int NativeLanguageDescription = 100;

			public const int AddressName = 60;
            public const int OrganizationName = 60;
            public const int AddressAttention = 50;
            public const int Street = 80;
            public const int City = 40;
            public const int State = 25;
            public const int PostalCode = 10;

            public const int Email = 80;
            public const int Phone = 17;
            public const int PhoneExt = 6;
            public const int Title = 40;
            public const int WebSite = 80;
            public const int Url = 255;

            public const int LargeDescription = 4000;
            public const int Description = 250;
            public const int Note = 1000;
            public const int SpecialInstructions = 500;

            public const int CombinedContactJson = 200;
            public const int CombinedStreetJson = 300;
			public const int CombinedNameJson = 250;

			public const int TaxExemptNum = 25;

            // Used on sales order and sales order line
            public const int ItemDescription = 500;
            public const int VendorItemNum = 36;
            public const int ItemNum = 25;

			public const int ShippingAccountNum = 35;
            public const int ShippingRateServiceCode = 35;

            public const int OrderNumPrefix = 5;
            public const int AisleRowBin = 5;
            public const int HexColorCode = 6;

			public const int Sha512Hash = 64;
		}

        public class Scale
        {
            public const int Decimal2 = 2;
            public const int Decimal4 = 4;
            public const int Decimal6 = 6;
            public const int Decimal8 = 8;
        }

        public class Precision
        {
			public const int LocationCoordinate = 11;

			public const int Decimal2 = 2;
            public const int Decimal3 = 3;
            public const int Decimal4 = 4;
            public const int Decimal5 = 5;
            public const int Decimal6 = 6;
            public const int Decimal7 = 7;
            public const int Decimal8 = 8;
			public const int Decimal9 = 9;
			public const int Decimal10 = 10;
            public const int Decimal12 = 12;
            public const int Decimal18 = 18;


            public const int DateTime2 = 2;
            public const int DateTime4 = 4;
        }
    }
}
