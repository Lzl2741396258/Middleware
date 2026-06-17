using System;
using System.Configuration;
using System.Data;
using Npgsql;

namespace Ersa.Mes.Database;

public static class Edc_AccessHelper
{
	private static string _strConnectString;

	public static string m_strConnectString
	{
		get
		{
			if (string.IsNullOrEmpty(_strConnectString))
			{
				_strConnectString = ConfigurationManager.AppSettings["VF4"];
			}
			return _strConnectString;
		}
	}

	public static int ExecuteSQL(string i_strSql)
	{
		int num = -1;
		using (NpgsqlConnection connection = new NpgsqlConnection(m_strConnectString))
		{
			using NpgsqlCommand command = new NpgsqlCommand(i_strSql, connection);
			try
			{
				connection.Open();
				num = command.ExecuteNonQuery();
			}
			catch (NpgsqlException ex)
			{
				throw new Exception("Execute sql error...Details:'" + ex.Message + "'");
			}
			finally
			{
				connection.Close();
			}
		}
		return num;
	}

	public static DataSet ExecuteQuery(string i_strSql)
	{
		DataSet ds = new DataSet();
		using (NpgsqlConnection connection = new NpgsqlConnection(m_strConnectString))
		{
			using NpgsqlDataAdapter sqldap = new NpgsqlDataAdapter(i_strSql, connection);
			try
			{
				connection.Open();
				int i = sqldap.Fill(ds);
			}
			catch (NpgsqlException ex)
			{
				throw new Exception("Execute sql error...Details:'" + ex.Message + "'");
			}
			finally
			{
				connection.Close();
			}
		}
		return ds;
	}

	public static void Sub_TestAccess()
	{
	}
}
