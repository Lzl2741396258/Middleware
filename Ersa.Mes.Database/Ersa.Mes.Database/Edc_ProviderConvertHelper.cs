using System;
using System.Collections.Generic;

namespace Ersa.Mes.Database;

public static class Edc_ProviderConvertHelper
{
	public const string mC_strPostgresProviderName = "Npgsql";

	public const string mC_strSqlServerProviderName = "System.Data.SqlClient";

	public const string mC_strAccessProviderName = "Microsoft.ACE.OLEDB.12.0";

	public const string mC_strSqlCeProviderName = "System.Data.SqlServerCe.4.0";

	private static readonly IDictionary<string, Enum_DatabaseType> m_dicDatabaseType = new Dictionary<string, Enum_DatabaseType>
	{
		{
			"Npgsql",
			Enum_DatabaseType.Postgres
		},
		{
			"System.Data.SqlClient",
			Enum_DatabaseType.SqlServer
		},
		{
			"System.Data.SqlServerCe.4.0",
			Enum_DatabaseType.SqlCe
		},
		{
			"Microsoft.ACE.OLEDB.12.0",
			Enum_DatabaseType.Access
		}
	};

	public static Enum_DatabaseType Fun_enuGetDatabaseType(string i_strProviderName)
	{
		if (string.IsNullOrWhiteSpace(i_strProviderName))
		{
			throw new Exception("The provider name cannot be empty");
		}
		if (!m_dicDatabaseType.ContainsKey(i_strProviderName))
		{
			throw new Exception("The provider '" + i_strProviderName + "' is invalid or Empty");
		}
		return m_dicDatabaseType[i_strProviderName];
	}

	public static string Fun_strGetDatabaseProviderName(Enum_DatabaseType i_enuDatabaseType)
	{
		return i_enuDatabaseType switch
		{
			Enum_DatabaseType.SqlServer => "System.Data.SqlClient", 
			Enum_DatabaseType.SqlExpress => "System.Data.SqlClient", 
			Enum_DatabaseType.SqlCe => "System.Data.SqlServerCe.4.0", 
			Enum_DatabaseType.Postgres => "Npgsql", 
			Enum_DatabaseType.Access => "Microsoft.ACE.OLEDB.12.0", 
			_ => throw new Exception("The database type is invalid or not supported"), 
		};
	}
}
