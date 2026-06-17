using System;
using System.Collections.Specialized;
using System.Reflection;
using Ersa.Mes.Database.Interface;

namespace Ersa.Mes.Database;

public static class Edc_ProviderFactory
{
	public static Inf_DatabaseProvider Fun_fdcCreateDatabaseProvider<T>(object obj)
	{
		NameValueCollection i_fdcAppsetings = new NameValueCollection();
		PropertyInfo[] properties = obj.GetType().GetProperties();
		foreach (PropertyInfo item in properties)
		{
			i_fdcAppsetings.Add(item.Name, item.GetValue(obj).ToString());
		}
		string test1 = i_fdcAppsetings["MinPoolSize"];
		NameValueCollection a_ValueCollection = Fun_fdcGetDatabaseSettingList(i_fdcAppsetings);
		Enum_DatabaseType a_enuDatabaseType = Edc_ProviderConvertHelper.Fun_enuGetDatabaseType(a_ValueCollection["ProviderName"]);
		switch (a_enuDatabaseType)
		{
		case Enum_DatabaseType.SqlServer:
			return new Edc_SqlserverProvider(i_fdcAppsetings);
		case Enum_DatabaseType.SqlExpress:
			return new Edc_SqlserverProvider(i_fdcAppsetings);
		case Enum_DatabaseType.SqlCe:
		case Enum_DatabaseType.Postgres:
			return new Edc_PostgresProvider(i_fdcAppsetings);
		case Enum_DatabaseType.Access:
			return new Edc_AccessProvider(i_fdcAppsetings);
		default:
			throw new Exception($"The provider '{a_enuDatabaseType}' is  invalid or not supported");
		}
	}

	private static NameValueCollection Fun_fdcGetDatabaseSettingList(NameValueCollection i_fdcAppsetings)
	{
		NameValueCollection a_ValueCollection = new NameValueCollection();
		if (i_fdcAppsetings["ProviderName"] != null)
		{
			a_ValueCollection.Add("ProviderName", i_fdcAppsetings["ProviderName"]);
			a_ValueCollection.Add("Server", i_fdcAppsetings["Server"]);
			a_ValueCollection.Add("Port", i_fdcAppsetings["Port"]);
			a_ValueCollection.Add("UserId", i_fdcAppsetings["UserId"]);
			a_ValueCollection.Add("Password", i_fdcAppsetings["Password"]);
			a_ValueCollection.Add("DataDirectory", i_fdcAppsetings["DataDirectory"]);
			a_ValueCollection.Add("BinaryDirectory", i_fdcAppsetings["BinaryDirectory"]);
			a_ValueCollection.Add("MinPoolSize", i_fdcAppsetings["MinPoolSize"]);
			a_ValueCollection.Add("MaxPoolSize", i_fdcAppsetings["MaxPoolSize"]);
			a_ValueCollection.Add("ConnectionLifeTime", i_fdcAppsetings["ConnectionLifeTime"]);
			a_ValueCollection.Add("CommandTimeout", i_fdcAppsetings["CommandTimeout"]);
		}
		return a_ValueCollection;
	}
}
