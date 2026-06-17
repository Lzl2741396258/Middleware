using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Model.Bibs;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Factory;

namespace Ersa.Mes.Middleware.MesTaskFolder;

public class MT_GetBibs : Edc_MesTask
{
	private string _strPathAndFilename;

	public const string mC_strFileName = "Satzdaten.csv";

	public const string mC_strColumnHead1 = "Set0";

	public const string mC_strColumnHead7 = "Mode6";

	public const string mC_strColumnHead8 = "Spray amount [%]7";

	public const string mC_strColumnHead9 = "Spray time [s]8";

	public string m_strPath { get; set; }

	public string m_strBibName { get; set; }

	public string m_strRecipeName { get; set; }

	public string m_strPathAndFilename
	{
		get
		{
			if (string.IsNullOrEmpty(_strPathAndFilename))
			{
				return Path.Combine(m_strPath, m_strBibName, m_strRecipeName);
			}
			return _strPathAndFilename;
		}
		set
		{
			_strPathAndFilename = value;
		}
	}

	public MT_GetBibs(string i_strPath, string i_strBibName, string i_strRecipeName, Inf_MesTaskAttributes i_edcMesTaskAttributes, Inf_Logger i_edcLogger)
		: base(i_edcMesTaskAttributes, i_edcLogger)
	{
		m_strPath = i_strPath;
		m_strBibName = i_strBibName;
		m_strRecipeName = i_strRecipeName;
	}

	public override Task Sub_Act()
	{
		return Task.CompletedTask;
	}

	public DataTable Fun_dtGetBibsFromCSV()
	{
		DataTable dt = null;
		try
		{
			string i_strPathAndFilename = Path.Combine(m_strPath, m_strBibName, "Satzdaten.csv");
			dt = Edc_CsvHelper.Fun_dtOpenCSV(i_strPathAndFilename, 1, i_blnHead: false);
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "Get Bibs error...Detail:'" + ex.Message + "'");
			return null;
		}
		return dt;
	}

	public List<Edc_BibsReflow> Fun_lstGetBibsFromXml(string i_strPathBib, string i_strBibName)
	{
		List<Edc_BibsReflow> a_lstBibs = new List<Edc_BibsReflow>();
		try
		{
			if (string.IsNullOrEmpty(i_strBibName))
			{
				List<FileInfo> a_lstDirectory = new DirectoryInfo(i_strPathBib).GetFiles(i_strPathBib, SearchOption.AllDirectories).ToList();
				foreach (FileInfo item in a_lstDirectory)
				{
					try
					{
						Edc_BibsReflow a_bib = item.FullName.Fun_edcDeserializeByFilePath<Edc_BibsReflow>();
						a_lstBibs.Add(a_bib);
					}
					catch
					{
					}
				}
			}
			OnShowMessage(Enum_LogType.Info, $"Middleware -> Get {a_lstBibs.Count} Bib File...");
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "Get Bibs error...Detail:'" + ex.Message + "'");
			return null;
		}
		return a_lstBibs;
	}

	public virtual void Sub_WriteToDatabaseXml(List<Edc_BibsReflow> i_lstBibsReflow)
	{
	}

	public List<string[]> Fun_lstGetSpray()
	{
		DataTable dt = Fun_dtGetBibsFromCSV();
		List<string[]> list = new List<string[]>();
		foreach (DataRow dr in dt.Rows)
		{
			if (!string.IsNullOrEmpty(dr["Mode6"].ToString()) || !string.IsNullOrEmpty(dr["Spray amount [%]7"].ToString()) || !string.IsNullOrEmpty(dr["Spray time [s]8"].ToString()))
			{
				list.Add(new string[4]
				{
					dr["Set0"].ToString(),
					dr["Mode6"].ToString(),
					dr["Spray amount [%]7"].ToString(),
					dr["Spray time [s]8"].ToString()
				});
			}
		}
		if (list.Count != 0)
		{
			OnShowMessage(Enum_LogType.Info, $"Get Spray Data Successed...{list.Count} rows in total");
		}
		return list;
	}
}
