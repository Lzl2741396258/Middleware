using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.Common.Extensions;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.FileSystem.Model.Protocol;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.FileSystem.Model.RequestResponse.Request;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Definition;
using Ersa.Mes.Middleware.Factory;

namespace Ersa.Mes.Middleware.MesFunctionFolder;

public class PCBOutfeed : Edc_MesFunction
{
	public Request_PCBOutfeed m_Request;

	public Edc_ProtocolSelectiveZevi m_ProtocolZvei = new Edc_ProtocolSelectiveZevi();

	protected override string m_strFunctionName => "PCBOutfeed";

	public override bool m_blnResponse => false;

	public byte m_bytTrack { get; set; } = 1;


	public string m_strCodes { get; set; } = string.Empty;


	public DateTime m_dtmInfeed { get; set; }

	public DateTime m_dtmOutfeed { get; set; }

	public string m_strPathCreateProtocol { get; set; }

	public string m_strPathRootDirectory { get; set; }

	public string m_strProtocolFilename { get; set; }

	public PCBOutfeed(List<Edc_ConfigMesFunction> i_lstMesFunction, Inf_Logger i_edcLogger)
		: base(i_edcLogger)
	{
		Edc_ConfigMesFunction function = i_lstMesFunction.Where((Edc_ConfigMesFunction s) => s.Pro_strName.Equals(m_strFunctionName)).FirstOrDefault();
		if (function != null)
		{
			base.m_blnActivePlatform = function.Pro_blnActivePlatform;
			base.m_blnActiveDatabase = function.Pro_blnActiveDatabase;
			base.m_blnActiveLocalFile = function.Pro_blnActiveLocalFile;
		}
		m_strPathRootDirectory = AppDomain.CurrentDomain.BaseDirectory;
		i_edcLogger.Debug($"Assmebly MES Function {m_strFunctionName}   Platform:{base.m_blnActivePlatform}  Database:{base.m_blnActiveDatabase}  LocalFile:{base.m_blnActiveLocalFile}", null, ".ctor", 88);
	}

	public override bool Fun_blnContains()
	{
		return base.m_strRequest.IndexOf(Enum_MesFunction.PcbAusgelaufen_Anfo_EL.Fun_strGetDescription(), StringComparison.OrdinalIgnoreCase) > 0;
	}

	public override void Sub_GetRequest()
	{
		m_Request = base.m_strRequest.Fun_DeserializeContent<Request_PCBOutfeed>();
		m_dtmOutfeed = m_Request.m_sttHeader.m_dtmTimestamp.ToLocalTime();
		m_bytTrack = m_Request.m_sttHeader.m_bytTrackNumber;
		m_strCodes = Fun_strGetCode();
	}

	public override Task<string> Fun_strExecute()
	{
		try
		{
			base.m_edcLogger.Debug("Execute " + m_strFunctionName, null, "Fun_strExecute", 120);
			Sub_GetRequest();
			if (base.m_blnActivePlatform)
			{
				base.m_edcLogger.Debug("Begin " + MethodBase.GetCurrentMethod().DeclaringType.Name + ".Fun_blnConnectMesPlatform()...", null, "Fun_strExecute", 128);
				Task.Run((Func<Task>)Fun_blnConnectMesPlatform);
			}
			if (base.m_blnActiveDatabase)
			{
				Task.Run((Action)Sub_AddToDatabase);
			}
			if (base.m_blnActiveLocalFile)
			{
				Task.Run(delegate
				{
					Sub_WriteToLocalFile(MethodBase.GetCurrentMethod().DeclaringType.Name);
				});
			}
			base.m_i32ActCount++;
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "Ersasoft -> 'PCB Outfeed' Error...Details:'" + ex.Message + "'");
		}
		base.m_edcLogger.Debug("End Execute " + m_strFunctionName, null, "Fun_strExecute", 149);
		return Task.FromResult(string.Empty);
	}

	protected virtual void Sub_CreateProtocol()
	{
		if (!string.IsNullOrEmpty(m_strPathRootDirectory))
		{
			m_strPathCreateProtocol = Path.Combine(m_strPathRootDirectory, "Protocol");
			m_strProtocolFilename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".protocol";
			if (!Directory.Exists(m_strPathCreateProtocol))
			{
				Directory.CreateDirectory(m_strPathCreateProtocol);
			}
		}
	}

	public string Fun_strGetCode()
	{
		try
		{
			Edc_Identifier[] a_sttIdentifier = m_Request.ma_sttIdentifier;
			if (a_sttIdentifier.Length == 0)
			{
				return "No_Code";
			}
			string a_strCodes = string.Empty;
			Edc_Identifier[] array = a_sttIdentifier;
			foreach (Edc_Identifier item in array)
			{
				a_strCodes = a_strCodes + item.m_strValue + ",";
			}
			return a_strCodes.TrimEnd(',');
		}
		catch
		{
		}
		return string.Empty;
	}
}
