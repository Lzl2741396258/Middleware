using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Model.Data;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Factory;

namespace Ersa.Mes.Middleware.MesTaskFolder;

public abstract class MT_GetEditData : Edc_MesTask
{
	private readonly string mC_PartFilename1 = "HotflowData";

	private string Pro_strFilename1;

	private readonly string mC_PartFilename2 = "Hotflow";

	private string Pro_strFilename2;

	public int m_i32LastLine = 0;

	public int m_i32LastLine2 = 0;

	public List<Edc_DataRecord> m_lstEditRecord;

	public Edc_DataRecord m_CurrectEditRecord;

	private FileSystemWatcher _fileWatcher;

	private bool m_blnInitialize = true;

	public string m_strPathData { get; set; }

	public bool m_blnActive1 { get; set; } = true;


	public bool m_blnActive2 { get; set; } = true;


	public FileSystemWatcher m_edcFileWatcher
	{
		get
		{
			if (_fileWatcher == null)
			{
				OnChanged(null, null);
			}
			return _fileWatcher;
		}
		set
		{
			_fileWatcher = value;
		}
	}

	public MT_GetEditData(string i_stirPathData, Inf_MesTaskAttributes i_edcMesTask, Inf_Logger i_edcLogger)
		: base(i_edcMesTask, i_edcLogger)
	{
		m_strPathData = i_stirPathData;
		if (string.IsNullOrEmpty(m_strPathData))
		{
			OnShowMessage(Enum_LogType.Error, "Please check the ersa config -- path of data...");
			return;
		}
		Pro_strFilename1 = Path.Combine(m_strPathData, mC_PartFilename1 + DateTime.Now.ToString("yyyyMM") + ".mdb");
		if (!File.Exists(Pro_strFilename1))
		{
			m_blnActive1 = false;
			OnShowMessage(Enum_LogType.Warn, "'Middleware -> Data Edit File '" + Pro_strFilename1 + "' is not exsit...");
		}
		Pro_strFilename2 = Path.Combine(m_strPathData, mC_PartFilename2 + DateTime.Now.ToString("yyyyMM") + ".mdb");
		if (!File.Exists(Pro_strFilename2))
		{
			m_blnActive2 = false;
			OnShowMessage(Enum_LogType.Warn, "'Middleware -> Data Edit File '" + Pro_strFilename2 + "' is not exsit...");
		}
	}

	public override Task Sub_Act()
	{
		if (m_blnInitialize)
		{
			m_edcFileWatcher = new FileSystemWatcher();
			m_edcFileWatcher.Path = m_strPathData;
			m_edcFileWatcher.Filter = "*.mdb";
			m_edcFileWatcher.EnableRaisingEvents = true;
			m_edcFileWatcher.IncludeSubdirectories = false;
			m_edcFileWatcher.NotifyFilter = NotifyFilters.LastWrite;
			m_edcFileWatcher.Changed += OnChanged;
			m_blnInitialize = false;
		}
		return Task.CompletedTask;
	}

	public override void Sub_End()
	{
		if (m_edcFileWatcher != null)
		{
			m_edcFileWatcher.EnableRaisingEvents = false;
		}
		base.Sub_End();
	}

	public abstract Task Sub_ConnectMesPlatform();

	protected void OnChanged(object sender, FileSystemEventArgs e)
	{
		try
		{
			Thread.Sleep(1000);
			if (sender != null || e != null)
			{
				m_lstEditRecord = new List<Edc_DataRecord>();
				Sub_GetDataEditRecord();
				if (m_lstEditRecord != null && m_lstEditRecord.Count > 0)
				{
					Sub_ConnectMesPlatform();
				}
			}
		}
		catch (Exception ex)
		{
			base.m_edcLogger.Error("Middleware -> Get Hotflow Data Error...Details:'" + ex.Message + "'", null, "OnChanged", 182);
		}
	}

	public void Sub_GetDataEditRecord()
	{
		DataTable dt = null;
		try
		{
			m_i32LastLine = Edc_CsvHelper.Fun_i32GetMaxRow(Pro_strFilename1);
			m_i32LastLine2 = Edc_CsvHelper.Fun_i32GetMaxRow(Pro_strFilename2);
			if (m_blnActive1)
			{
				dt = Edc_CsvHelper.Fun_dtOpenCSV(Pro_strFilename1, m_i32LastLine, i_blnHead: false);
				Sub_AddEditRecordToList(dt);
			}
			if (m_blnActive2)
			{
				dt = Edc_CsvHelper.Fun_dtOpenCSV(Pro_strFilename2, m_i32LastLine2, i_blnHead: false);
				Sub_AddEditRecordToList(dt);
			}
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "Middleware -> Get Hotflow Data error...Sub_GetDataEditRecord()...Detail:'" + ex.Message + "'");
		}
	}

	private void Sub_AddEditRecordToList(DataTable dt)
	{
		foreach (DataRow dr in dt.Rows)
		{
			Edc_DataRecord model = new Edc_DataRecord();
			model.m_strEditTime = dr[0].ToString();
			model.m_strUser = dr[1].ToString();
			model.m_strPlace = dr[2].ToString();
			model.m_strPLCAddress = dr[3].ToString();
			model.m_strOldValue = dr[4].ToString();
			model.n_strNewValue = dr[5].ToString();
			m_lstEditRecord.Add(model);
		}
	}
}
