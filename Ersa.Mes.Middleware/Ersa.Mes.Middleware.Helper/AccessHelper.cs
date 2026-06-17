using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

namespace Ersa.Mes.Middleware.Helper;

public static class AccessHelper
{
	public static OleDbConnection conn = new OleDbConnection();

	public static OleDbCommand comm = new OleDbCommand();

	private static string _strConnectString;

	public static string m_strConnectString
	{
		get
		{
			if (string.IsNullOrEmpty(_strConnectString))
			{
				_strConnectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ConfigurationManager.AppSettings["AccessConnect"] + ";Persist Security Info=False;";
			}
			return _strConnectString;
		}
	}

	private static void Sub_OpenConnection()
	{
		if (conn.State == ConnectionState.Closed)
		{
			conn.ConnectionString = m_strConnectString;
			comm.Connection = conn;
			try
			{
				conn.Open();
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
	}

	private static void Sub_CloseConnection()
	{
		if (conn.State == ConnectionState.Open)
		{
			conn.Close();
			conn.Dispose();
			comm.Dispose();
		}
	}

	public static void Sub_ExcuteSql(string i_strSql)
	{
		try
		{
			Sub_OpenConnection();
			comm.CommandType = CommandType.Text;
			comm.CommandText = i_strSql;
			comm.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			throw new Exception(e.Message);
		}
		finally
		{
			Sub_CloseConnection();
		}
	}

	public static OleDbDataReader Sub_DataReader(string i_strSql)
	{
		OleDbDataReader dr = null;
		try
		{
			Sub_OpenConnection();
			comm.CommandText = i_strSql;
			comm.CommandType = CommandType.Text;
			dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
		}
		catch
		{
			try
			{
				dr.Close();
				Sub_CloseConnection();
			}
			catch
			{
			}
		}
		return dr;
	}

	public static void Sub_DataReader(string i_strSql, ref OleDbDataReader dr)
	{
		try
		{
			Sub_OpenConnection();
			comm.CommandText = i_strSql;
			comm.CommandType = CommandType.Text;
			dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
		}
		catch
		{
			try
			{
				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
			}
			catch
			{
			}
			finally
			{
				Sub_CloseConnection();
			}
		}
	}

	public static DataSet Sub_DataSet(string i_strSql)
	{
		DataSet ds = new DataSet();
		OleDbDataAdapter da = new OleDbDataAdapter();
		try
		{
			Sub_OpenConnection();
			comm.CommandType = CommandType.Text;
			comm.CommandText = i_strSql;
			da.SelectCommand = comm;
			da.Fill(ds);
		}
		catch (Exception e)
		{
			throw new Exception(e.Message);
		}
		finally
		{
			Sub_CloseConnection();
		}
		return ds;
	}

	public static void Sub_DataSet(string i_strSql, ref DataSet ds)
	{
		OleDbDataAdapter da = new OleDbDataAdapter();
		try
		{
			Sub_OpenConnection();
			comm.CommandType = CommandType.Text;
			comm.CommandText = i_strSql;
			da.SelectCommand = comm;
			da.Fill(ds);
		}
		catch (Exception e)
		{
			throw new Exception(e.Message);
		}
		finally
		{
			Sub_CloseConnection();
		}
	}

	public static DataTable Sub_DataTable(string i_strSql)
	{
		DataTable dt = new DataTable();
		OleDbDataAdapter da = new OleDbDataAdapter();
		try
		{
			Sub_OpenConnection();
			comm.CommandType = CommandType.Text;
			comm.CommandText = i_strSql;
			da.SelectCommand = comm;
			da.Fill(dt);
		}
		catch (Exception e)
		{
			throw new Exception(e.Message);
		}
		finally
		{
			Sub_CloseConnection();
		}
		return dt;
	}

	public static void Sub_DataTable(string i_strSql, ref DataTable dt)
	{
		OleDbDataAdapter da = new OleDbDataAdapter();
		try
		{
			Sub_OpenConnection();
			comm.CommandType = CommandType.Text;
			comm.CommandText = i_strSql;
			da.SelectCommand = comm;
			da.Fill(dt);
		}
		catch (Exception e)
		{
			throw new Exception(e.Message);
		}
		finally
		{
			Sub_CloseConnection();
		}
	}

	public static DataView Fun_dvDataView(string i_strSql)
	{
		OleDbDataAdapter da = new OleDbDataAdapter();
		DataView dv = new DataView();
		DataSet ds = new DataSet();
		try
		{
			Sub_OpenConnection();
			comm.CommandType = CommandType.Text;
			comm.CommandText = i_strSql;
			da.SelectCommand = comm;
			da.Fill(ds);
			dv = ds.Tables[0].DefaultView;
		}
		catch (Exception e)
		{
			throw new Exception(e.Message);
		}
		finally
		{
			Sub_CloseConnection();
		}
		return dv;
	}
}
