using System.Collections.Specialized;
using System.Data.Common;
using Npgsql;

namespace Ersa.Mes.Database.Interface;

public class Edc_PostgresProvider : Edc_BasicProvider, Inf_DatabaseProvider
{
	private DbConnectionStringBuilder m_ConnectionStringBuilder;

	public string Pro_ConnectionString
	{
		get
		{
			if (m_ConnectionStringBuilder == null)
			{
				m_ConnectionStringBuilder = Fun_CreateConnectionStringBuilder();
			}
			return m_ConnectionStringBuilder.ConnectionString;
		}
	}

	public Edc_PostgresProvider(NameValueCollection i_fdcAppSettings)
		: base(i_fdcAppSettings)
	{
	}

	protected override DbConnectionStringBuilder Fun_CreateConnectionStringBuilder()
	{
		DbConnectionStringBuilder a_Builder = new DbConnectionStringBuilder();
		a_Builder.Add("Server", "");
		a_Builder.Add("Port", "");
		a_Builder.Add("User Id", "");
		a_Builder.Add("Password", "");
		a_Builder.Add("Pooling", "");
		a_Builder.Add("MinPoolSize", "");
		a_Builder.Add("MaxPoolSize", "");
		a_Builder.Add("CommandTimeout", "");
		a_Builder.Add("Timeout", "");
		return a_Builder;
	}

	public DbConnection Fun_fdcGetConnection()
	{
		return new NpgsqlConnection(Pro_ConnectionString);
	}
}
