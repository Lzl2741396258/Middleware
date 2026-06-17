using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.Common.Extensions;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.FileSystem.Model.RequestResponse.Request;
using Ersa.Mes.FileSystem.Model.RequestResponse.Response;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Definition;
using Ersa.Mes.Middleware.Factory;

namespace Ersa.Mes.Middleware.MesFunctionFolder;

public class ReleaseInfeed : Edc_MesFunction
{
	protected override string m_strFunctionName => "ReleaseInfeed";

	public override bool m_blnResponse => true;

	protected Request_ReleaseInfeed m_Request { get; set; }

	protected Response_ReleaseInfeed m_Response { get; set; }

	public Enum_MachineType m_enuMachineType { get; set; } = Enum_MachineType.Reflow;


	protected Edc_Result r_edcResult { get; set; } = new Edc_Result();


	protected Enum_ComingRelease r_enuComingRelease { get; set; } = Enum_ComingRelease.NichtDefiniert;


	public string m_strCameraNoRead { get; set; } = string.Empty;


	public string m_strCodes { get; set; } = string.Empty;


	public List<Struct_Identifier> m_lstIdentifier { get; set; } = new List<Struct_Identifier>();


	public ReleaseInfeed(List<Edc_ConfigMesFunction> i_lstMesFunction, Inf_Logger i_edcLogger, Enum_MachineType i_enuMachineType, string i_strCameraNoRead)
		: base(i_edcLogger)
	{
		Edc_ConfigMesFunction function = i_lstMesFunction.Where((Edc_ConfigMesFunction s) => s.Pro_strName.Equals(m_strFunctionName)).FirstOrDefault();
		if (function != null)
		{
			base.m_blnActivePlatform = function.Pro_blnActivePlatform;
			base.m_blnActiveDatabase = function.Pro_blnActiveDatabase;
			base.m_blnActiveLocalFile = function.Pro_blnActiveLocalFile;
		}
		m_strCameraNoRead = i_strCameraNoRead;
		m_enuMachineType = i_enuMachineType;
		i_edcLogger.Debug($"Assmebly MES Function {m_strFunctionName}   Platform:{base.m_blnActivePlatform}  Database:{base.m_blnActiveDatabase}  LocalFile:{base.m_blnActiveLocalFile}", null, ".ctor", 87);
	}

	public override bool Fun_blnContains()
	{
		return base.m_strRequest.IndexOf(Enum_MesFunction.EinlaufFreigeben_Anfo_EL.Fun_strGetDescription(), StringComparison.OrdinalIgnoreCase) > 0;
	}

	public override void Sub_GetRequest()
	{
		
		m_Request = base.m_strRequest.Fun_DeserializeContent<Request_ReleaseInfeed>();
		if (m_Request != null && m_Request.ma_edcIdentifier != null)
		{
			m_lstIdentifier = Fun_lstGetIdentifier();
		}
	}

	public virtual string Fun_strGetResponse()
	{
		m_Response = new Response_ReleaseInfeed
		{
			m_sttHeader = new Struct_HeaderWithTrack
			{
				m_dtmTimestamp = DateTime.Now,
				m_strProcessName = m_Request.m_sttHeader.m_strProcessName,
				m_strLineName = m_Request.m_sttHeader.m_strLineName,
				m_strStationName = m_Request.m_sttHeader.m_strStationName,
				m_strMessageId = m_Request.m_sttHeader.m_strMessageId,
				m_bytTrackNumber = m_Request.m_sttHeader.m_bytTrackNumber,
				m_strVersion = m_Request.m_sttHeader.m_strVersion
			},
			ma_sttIdentifier = m_Request.ma_edcIdentifier,
			m_enuMachineType = m_enuMachineType,
			m_sttResult = r_edcResult,
			m_enuComingRelease = r_enuComingRelease
		};
		return SerializerHelper.Fun_strSerializerModel<Response_ReleaseInfeed>(m_Response);
	}

	public override async Task<string> Fun_strExecute()
	{
		string a_strResponse = string.Empty;
		try
		{
			// Get Request Info
			Sub_GetRequest();
			base.m_edcLogger.Debug("Begin " + m_strFunctionName + ".Fun_blnConnectMesPlatform()", null, "Fun_strExecute", 141);
			await Task.Run((Func<Task>)Fun_blnConnectMesPlatform);
			base.m_edcLogger.Debug("End " + m_strFunctionName + ".Fun_blnConnectMesPlatform()", null, "Fun_strExecute", 143);
			if (base.m_blnActiveDatabase)
			{
				Task.Run((Action)Sub_AddToDatabase);
			}
			a_strResponse = Fun_strGetResponse();
			OnShowProcess(++base.m_i32ActCount);
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			OnShowMessage(Enum_LogType.Error, "Middleware -> 'Release Infeed' Sub_Execute() Error...Details:'" + ex.Message + "'");
		}
		base.m_edcLogger.Info("End " + m_strFunctionName);
		return a_strResponse;
	}

	protected List<Struct_Identifier> Fun_lstGetIdentifier()
	{
		string a_strCode = string.Empty;
		Struct_Identifier[] ma_edcIdentifier = m_Request.ma_edcIdentifier;
		for (int i = 0; i < ma_edcIdentifier.Length; i++)
		{
			Struct_Identifier item = ma_edcIdentifier[i];
			if (item.m_enmCodeMeaning == Enum_CodeMeaning.Code)
			{
				a_strCode = a_strCode + item.m_strValue + ",";
			}
		}
		m_strCodes = a_strCode.TrimEnd(',');
		return m_Request.ma_edcIdentifier.ToList();
	}

	protected byte Fun_u32GetTrack()
	{
		return m_Request.m_sttHeader.m_bytTrackNumber;
	}
}
