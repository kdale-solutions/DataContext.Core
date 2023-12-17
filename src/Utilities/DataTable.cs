using DataContext.Core.Interfaces.Entity;
using Global.Utilities;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace DataContext.Core.Utilities
{
	public static class DataTableExtensions
	{
		private static readonly string _nullableTypeName;

		static DataTableExtensions()
		{
			_nullableTypeName = typeof(Nullable<>).Name;
		}

		public static DataTable ToDataTable<T>(this T instance, string tableName) where T : class, IDataTableParameter
		{
			var dataTable = new DataTable(tableName);
			
			return dataTable.AddDataRow(instance, instance.GetType().GetProperties());
		}

		public static DataTable ToDataTable<T>(this IEnumerable<T> instance, string tableName) where T : class, IDataTableParameter
		{
			var dataTable = new DataTable(tableName);

			var count = instance.Count();

			for ( var i = 0; i < count; i++)
			{
				var item = instance.ElementAt(i);

				dataTable.AddDataRow(item, item.GetType().GetProperties(), i == 0);
			}

			return dataTable;
		}

		public static DataTable ToDataTable<T>(this T instance, string tableName, params string[] includedProperties) where T : class, IDataTableParameter
		{
			var dataTable = new DataTable(tableName);

			var properties = (from property in instance.GetType().GetProperties()
							 where includedProperties.Contains(property.Name)
							 select property)
							 .ToArray();

			if (properties.IsNullOrEmpty())
			{ 
				return null; 
			}

			return dataTable.AddDataRow(instance, properties);
		}

		public static DataTable AddDataRow<T>(this DataTable dataTable, T instance, PropertyInfo[] properties, bool firstRow = true) where T : class, IDataTableParameter
		{
			var row = dataTable.NewRow();

			foreach (var property in properties)
			{
				if (firstRow)
				{
					var column = property.CreateDataColumn(instance);

					if (column != null)
					{
						dataTable.Columns.Add(column);
						row[column.ColumnName] = column.DefaultValue;
					}
				}
				else
				{
					row[property.Name] = property.GetPropertyValue(instance);
				}
			}

			if (dataTable.Columns.Count == 0) return null;

			dataTable.Rows.Add(row);

			return dataTable;
		}

		public static DataColumn CreateDataColumn<T>(this PropertyInfo propertyInfo, T instance) where T : class, IDataTableParameter
		{
			var propertyType = propertyInfo.PropertyType;

			(var propertyValue, var allowDbNull) = propertyInfo.GetPropertyValueAndNullability(instance);

			return new DataColumn
			{
				ColumnName = propertyInfo.Name,
				DefaultValue = propertyValue,
				DataType = propertyType,
				AllowDBNull = allowDbNull
			};
		}

		private static object GetPropertyValue<T>(this PropertyInfo propertyInfo, T instance) where T : class, IDataTableParameter
		{
			(var propertyValue, var allowDbNull) = propertyInfo.GetPropertyValueAndNullability(instance);

			return propertyValue;
		}

		private static (object, bool) GetPropertyValueAndNullability<T>(this PropertyInfo propertyInfo, T instance) where T : class, IDataTableParameter
		{
			var propertyType = propertyInfo.PropertyType;

			var isString = propertyType.Name.IsEqualTo(nameof(String));
			var isNullableType = propertyType.Name.IsEqualToIgnoreCase(_nullableTypeName);
			var allowDbNull = isString || isNullableType;

			if (isNullableType)
			{
				propertyType = propertyInfo.PropertyType.GenericTypeArguments.FirstOrDefault();
			}

			var propertyValue = propertyInfo.GetValue(instance);

			if ((propertyValue == null && !allowDbNull) ||
				(propertyValue != null && !propertyType.IsPrimitive &&
				!isString))
			{
				return (null, allowDbNull);
			}
			else
			{
				return (propertyValue, allowDbNull);
			}
		}


		public static SqlParameter ToSqlParameter(this DataTable dataTable, string parameterName, string typeName)
		{
			return new SqlParameter
			{
				ParameterName = $"@{parameterName}",
				TypeName = typeName,
				SqlDbType = SqlDbType.Structured,
				Value = dataTable,
			};
		}
	}
}
