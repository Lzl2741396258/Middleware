using System;
using System.Data.SqlClient;
using Ersa.Mes.Logging;
using MesXPT.Model;

namespace MesXPT.CommunicationService;

public class Edc_XPTCommunicationService
{
	public string m_conStrReal = "";

	public XPT_Config m_Config;

	public Edc_XPTCommunicationService(XPT_Config i_Config, Inf_Logger i_edcLogger)
	{
		m_Config = i_Config;
	}

	public int InsertData(string i_strSQL)
	{
		m_conStrReal = "server=" + m_Config.m_strDatabaseSever + ";database=" + m_Config.m_strDatabase + ";uid=" + m_Config.m_strDatabaseUser + ";pwd=" + m_Config.m_strDatabasePassword;
		using SqlConnection con = new SqlConnection(m_conStrReal);
		using SqlCommand cmd = new SqlCommand(i_strSQL, con);
		con.Open();
		int r = cmd.ExecuteNonQuery();
		con.Close();
		return r;
	}

	private void DeleteData()
	{
		using SqlConnection con = new SqlConnection(m_conStrReal);
		string cmdTxt = "delete from TblClass where tClassId=29";
		using SqlCommand cmd = new SqlCommand(cmdTxt, con);
		con.Open();
		int r = cmd.ExecuteNonQuery();
		con.Close();
		Console.WriteLine("成功删除了{0}行", r);
	}

	private void UpdateData()
	{
		using SqlConnection con = new SqlConnection(m_conStrReal);
		string cmdTxt = "update TblClass set tClassName='高三十二班' where tClassId=30";
		using SqlCommand cmd = new SqlCommand(cmdTxt, con);
		con.Open();
		int r = cmd.ExecuteNonQuery();
		con.Close();
	}

	public string[] ScalarSelect(string i_strSQL, bool i_SelectAll = false)
	{
		string[] a_value = new string[1] { "" };
		try
		{
			m_conStrReal = "server=" + m_Config.m_strDatabaseSever + ";database=" + m_Config.m_strDatabase + ";uid=" + m_Config.m_strDatabaseUser + ";pwd=" + m_Config.m_strDatabasePassword;
			if (!i_SelectAll)
			{
				using (SqlConnection con = new SqlConnection(m_conStrReal))
				{
					using SqlCommand cmd = new SqlCommand(i_strSQL, con);
					con.Open();
					SqlDataReader reader2 = cmd.ExecuteReader();
					if (reader2.Read())
					{
						a_value[0] = reader2[0].ToString();
						con.Close();
						return a_value;
					}
					con.Close();
					return a_value;
				}
			}
			using SqlConnection sqlConnection = new SqlConnection(m_conStrReal);
			using SqlCommand sqlCommand = new SqlCommand(i_strSQL, sqlConnection);
			sqlConnection.Open();
			SqlDataReader reader = sqlCommand.ExecuteReader();
			string[] a_value2 = new string[20];
			if (reader.Read())
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
					a_value2[i] = reader[i].ToString();
				}
				sqlConnection.Close();
				return a_value2;
			}
			sqlConnection.Close();
			return a_value2;
		}
		catch (Exception ex)
		{
			a_value[0] = ex.ToString();
			return a_value;
		}
	}
}
