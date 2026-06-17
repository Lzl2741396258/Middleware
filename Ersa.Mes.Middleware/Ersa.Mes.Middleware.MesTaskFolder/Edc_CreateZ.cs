using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.Database;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Factory;
using Ersa.Mes.Middleware.Global;
using Ersa.Mes.Middleware.Interfaces;

namespace Ersa.Mes.Middleware.MesTaskFolder;

public class Edc_CreateZ : Edc_MesTask, Inf_MesTask
{
	public const string m_strSection1 = "Comment";

	public const string m_strSection2 = "Times";

	public const string m_strSection3 = "Power values";

	public const string m_strSection4 = "Conveyor 1";

	public const string m_strSection5 = "Conveyor 2";

	public const string m_strSection6 = "Conveyor 3";

	public const string m_strSection7 = "Conveyor 4";

	private Dictionary<string, int> m_lstData;

	public Dictionary<string, int> m_dicData
	{
		get
		{
			if (m_lstData == null)
			{
				m_lstData = new Dictionary<string, int>();
			}
			return m_lstData;
		}
		set
		{
			m_lstData = value;
		}
	}

	public string m_strPathCreateZ { get; set; }

	public int m_i32MachineID { get; set; }

	public Edc_CreateZ(string i_strPathCreateZ, int i_i32MachineID, Edc_ConfigBase i_Config, Inf_MesTaskAttributes i_edcMesTaskBase, Inf_Logger i_edcLogger)
		: base(i_edcMesTaskBase, i_edcLogger)
	{
		m_i32MachineID = i_i32MachineID;
		m_strPathCreateZ = i_strPathCreateZ;
	}

	public override async Task Sub_Act()
	{
		m_dicData = new Dictionary<string, int>();
		Fun_blnGetDataFromPostgresql(m_dicData);
		Sub_CreateZtxt();
	}

	public bool Fun_blnGetDataFromPostgresql(Dictionary<string, int> i_dic)
	{
		i_dic = new Dictionary<string, int>();
		try
		{
			string a_strSql = Fun_strGetSql_OperationDataValue();
			DataSet ds = Edc_PostgresqlHelper.ExecuteQuery(a_strSql);
			if (ds.Tables.Count == 0)
			{
				OnShowMessage(Enum_LogType.Warn, "DataSet result is null...");
				return false;
			}
			base.m_edcLogger.Debug($"Get {ds.Tables.Count} rows data...", null, "Fun_blnGetDataFromPostgresql", 105);
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				string dataid = dr["dataid"].ToString();
				string[] str1 = dataid.Split('|');
				if (str1.Length > 1)
				{
					dataid = str1[str1.Length - 1];
				}
				if (m_dicData.ContainsKey(dataid))
				{
					m_dicData.Remove(dataid);
				}
				int value = (dr["timespan"].ToString().Equals("0") ? int.Parse(dr["value"].ToString()) : int.Parse(dr["timespan"].ToString()));
				m_dicData.Add(dataid, value);
			}
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "Get data from database error...Details:'" + ex.Message + "'");
			return false;
		}
		return true;
	}

	public void Sub_CreateZtxt()
	{
		try
		{
			if (!File.Exists(m_strPathCreateZ))
			{
				File.Create(m_strPathCreateZ).Dispose();
				OnShowMessage(Enum_LogType.Info, "Create z.txt File Success...");
			}
			if (m_dicData.Count == 0)
			{
				base.m_edcLogger.Debug("m_dicData count = 0... ", null, "Sub_CreateZtxt", 154);
			}
			Edc_IniHelper.Fun_i32Write("Comment", "Timedata from", Edc_Global.Pro_dtmSoftBegin.ToString("dddd, dd. MM yyyy hh:mm:ss"), m_strPathCreateZ);
			int v2 = int.Parse(m_dicData["enmSttBetriebEinrichten"].ToString());
			Edc_IniHelper.Fun_i32Write("Times", "Waiting time MANUAL", Fun_strFormatDataTime(v2), m_strPathCreateZ);
			int v3 = int.Parse(m_dicData["enmSttBetriebProduktion"].ToString());
			Edc_IniHelper.Fun_i32Write("Times", "Working time production", Fun_strFormatDataTime(v3), m_strPathCreateZ);
			int v4 = int.Parse(m_dicData["enmSttBetriebWarten"].ToString());
			Edc_IniHelper.Fun_i32Write("Times", "Waiting time AUTO", Fun_strFormatDataTime(v4), m_strPathCreateZ);
			int v5 = int.Parse(m_dicData["enmSttBetriebGesamt"].ToString());
			Edc_IniHelper.Fun_i32Write("Times", "Working time total", Fun_strFormatDataTime(v5), m_strPathCreateZ);
			int v6 = int.Parse(m_dicData["enmSttBetriebStauNachf"].ToString());
			Edc_IniHelper.Fun_i32Write("Times", "Waiting time congestion", Fun_strFormatDataTime(v6), m_strPathCreateZ);
			Edc_IniHelper.Fun_i32Write("Power values", "Consumed power", m_dicData["enmLoetgutChargenZaehler"].ToString(), m_strPathCreateZ);
			Edc_IniHelper.Fun_i32Write("Conveyor 1", "Produced PCBs", m_dicData["enmAnzahlLoetgutProduziert"].ToString(), m_strPathCreateZ);
			Edc_IniHelper.Fun_i32Write("Conveyor 1", "Defective PCBs", m_dicData["enmAnzahlLoetgutFehlerhaft"].ToString(), m_strPathCreateZ);
			Edc_IniHelper.Fun_i32Write("Conveyor 1", "Total PCBs", m_dicData["enmAnzahlLoetgutEingelaufen"].ToString(), m_strPathCreateZ);
			Edc_IniHelper.Fun_i32Write("Conveyor 2", "Produced PCBs", "0", m_strPathCreateZ);
			Edc_IniHelper.Fun_i32Write("Conveyor 2", "Defective PCBs", "0", m_strPathCreateZ);
			Edc_IniHelper.Fun_i32Write("Conveyor 2", "Total PCBs", "0", m_strPathCreateZ);
			Edc_IniHelper.Fun_i32Write("Conveyor 3", "Produced PCBs", "0", m_strPathCreateZ);
			Edc_IniHelper.Fun_i32Write("Conveyor 3", "Defective PCBs", "0", m_strPathCreateZ);
			Edc_IniHelper.Fun_i32Write("Conveyor 3", "Total PCBs", "0", m_strPathCreateZ);
			Edc_IniHelper.Fun_i32Write("Conveyor 4", "Produced PCBs", "0", m_strPathCreateZ);
			Edc_IniHelper.Fun_i32Write("Conveyor 4", "Defective PCBs", "0", m_strPathCreateZ);
			Edc_IniHelper.Fun_i32Write("Conveyor 4", "Total PCBs", "0", m_strPathCreateZ);
			OnShowMessage(Enum_LogType.Info, "Input Data to " + m_strPathCreateZ + " Success...");
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "Create z.txt Fail... Details:'" + ex.Message + "'");
		}
	}

	public string Fun_strFormatDataTime(int i_i32Second)
	{
		TimeSpan ts = new TimeSpan(0, 0, i_i32Second);
		return ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0');
	}

	public string Fun_strGetSql_OperationDataID()
	{
		StringBuilder strSql = new StringBuilder();
		strSql.Append(" SELECT operatingdataid");
		strSql.Append(" FROM public.machineoperatingdatahead");
		strSql.AppendFormat(" where machineid = {0}", m_i32MachineID);
		strSql.Append(" order by creationdate desc ");
		strSql.Append(" limit 3");
		return strSql.ToString();
	}

	public string Fun_strGetSql_OperationDataValue()
	{
		StringBuilder strSql = new StringBuilder();
		strSql.Append(" SELECT operatingdataid, dataid, namekey, timespan, percentage, value, realvalue");
		strSql.Append(" FROM public.machineoperatingdatavalue");
		strSql.AppendFormat(" where operatingdataid in ({0})", Fun_strGetSql_OperationDataID());
		return strSql.ToString();
	}
}
