using System.Collections.Specialized;
using System.Data.Common;
using System.Data.OleDb;

namespace Ersa.Mes.Database.Interface;

public class Edc_AccessProvider : Edc_BasicProvider, Inf_DatabaseProvider
{
	public string Pro_ConnectionString { get; }

	public Edc_AccessProvider(NameValueCollection i_fdcAppSettings)
		: base(i_fdcAppSettings)
	{
	}

	protected override DbConnectionStringBuilder Fun_CreateConnectionStringBuilder()
	{
		DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
		builder.ConnectionString = "Data Source=c:\\MyData\\MyDb.mdb";
		builder.Add("Provider", "Microsoft.Jet.Oledb.4.0");
		builder.Add("Jet OLEDB:Database Password", "123");
		builder.Add("Jet OLEDB: System Database", "c:\\MyData\\Workgroup.mdb");
		builder.Add("Jet OLEDB:Database Locking Mode", 1);
		return builder;
	}

	public DbConnection Fun_fdcGetConnection()
	{
		return new OleDbConnection(Pro_ConnectionString);
	}
}
