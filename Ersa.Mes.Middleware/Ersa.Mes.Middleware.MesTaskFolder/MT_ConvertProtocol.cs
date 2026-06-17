using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Model.Protocol;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Factory;
using Ersa.Mes.Middleware.Interfaces;

namespace Ersa.Mes.Middleware.MesTaskFolder;

public class MT_ConvertProtocol : Edc_MesTask, Inf_MesTask
{
	public List<FileInfo> m_FileInfoList = new List<FileInfo>();

	public string m_strPathProtocol { get; set; }

	public string m_strPathProtocolBak { get; set; }

	public string m_strPathVF3Protocol { get; set; }

	public MT_ConvertProtocol(string i_strPathProtocol, string i_strPathBakProtocol, string i_strPathVF3Protocol, Inf_MesTaskAttributes i_edcMesTask, Inf_Logger i_edcLogger)
		: base(i_edcMesTask, i_edcLogger)
	{
		m_strPathProtocol = i_strPathProtocol;
		m_strPathProtocolBak = i_strPathBakProtocol;
		m_strPathVF3Protocol = i_strPathVF3Protocol;
	}

	public override Task Sub_Act()
	{
		throw new NotImplementedException();
	}

	private void Sub_ConvertProtocol()
	{
	}

	public Edc_ParameterMeasuring GetMeasuring(string i_strBeginTime, string i_strChannelName, string i_strChannelUnit, string i_strSampleValue, string i_strLimitHH = "-1", string i_strLimitLL = "-1")
	{
		Edc_MeasuringSample sample = new Edc_MeasuringSample
		{
			m_strValue = i_strSampleValue,
			m_strTime = i_strBeginTime
		};
		Edc_MeasuringNominalValue nominalValue = new Edc_MeasuringNominalValue
		{
			m_strValue = i_strSampleValue
		};
		Edc_MeasuringLimit_HH hh = new Edc_MeasuringLimit_HH
		{
			m_strValue = i_strLimitHH,
			m_strRelative = "true"
		};
		Edc_MeasuringLimit_LL ll = new Edc_MeasuringLimit_LL
		{
			m_strValue = i_strLimitLL,
			m_strRelative = "true"
		};
		Edc_MeasuringChannel channel = new Edc_MeasuringChannel
		{
			m_strName = i_strChannelName,
			m_strUnitOfMeasure = i_strChannelUnit,
			m_strSample = sample,
			m_strNominalValue = nominalValue,
			m_sttLimitHH = hh,
			m_sttLimitLL = ll
		};
		return new Edc_ParameterMeasuring
		{
			m_strEquipment = i_strChannelName,
			m_strChannel = channel
		};
	}

	private void SUB_MovetoBak(string i_strPathBak, FileInfo i_FileInfo)
	{
		string a_strNewFullName = Path.Combine(i_strPathBak, i_FileInfo.Name);
		if (!Directory.Exists(i_strPathBak))
		{
			Directory.CreateDirectory(i_strPathBak);
			OnShowMessage(Enum_LogType.Info, "Create directory:'" + i_strPathBak + "'");
		}
		try
		{
			if (!File.Exists(a_strNewFullName))
			{
				File.Move(i_FileInfo.FullName, a_strNewFullName);
				OnShowMessage(Enum_LogType.Info, "Protocol move to '" + a_strNewFullName + "'");
				return;
			}
			FileInfo fileinfo = new FileInfo(a_strNewFullName);
			File.Move(i_FileInfo.FullName, Path.Combine(i_strPathBak + DateTime.Now.ToString("yyyyMMddHHmmssfff") + i_FileInfo.Extension));
			OnShowMessage(Enum_LogType.Info, "Protocol Rename and move to '" + a_strNewFullName + "'");
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "Error message'" + ex.Message + "'");
		}
	}

	public List<FileInfo> GetAllFiles(DirectoryInfo i_strDirctoryPath)
	{
		FileInfo[] allFile = i_strDirctoryPath.GetFiles();
		FileInfo[] array = allFile;
		foreach (FileInfo fi in array)
		{
			m_FileInfoList.Add(fi);
		}
		DirectoryInfo[] allDir = i_strDirctoryPath.GetDirectories();
		DirectoryInfo[] array2 = allDir;
		foreach (DirectoryInfo d in array2)
		{
			GetAllFiles(d);
		}
		return m_FileInfoList;
	}
}
