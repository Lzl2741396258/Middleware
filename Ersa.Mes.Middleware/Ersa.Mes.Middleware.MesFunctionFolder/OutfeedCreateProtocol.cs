using System;
using System.Collections.Generic;
using System.IO;
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

public class OutfeedCreateProtocol : Edc_MesFunction
{
	protected override string m_strFunctionName => "OutfeedCreateProtocol";

	public override bool m_blnResponse => true;

	public Request_OutfeedCreateProtocol m_Request { get; set; }

	public Response_OutfeedCreateProtocol m_Response { get; set; }

	public string m_strCodes { get; set; } = string.Empty;


	public byte m_bytTack { get; set; } = 1;


	protected Edc_Result r_edcResult { get; set; } = new Edc_Result
	{
		m_enuResultCode = Enum_ResponseCode.ok
	};


	public OutfeedCreateProtocol(List<Edc_ConfigMesFunction> i_lstMesFunction, Inf_Logger i_edcLogger)
		: base(i_edcLogger)
	{
		Edc_ConfigMesFunction function = i_lstMesFunction.Where((Edc_ConfigMesFunction s) => s.Pro_strName.Equals(m_strFunctionName)).FirstOrDefault();
		if (function != null)
		{
			base.m_blnActivePlatform = function.Pro_blnActivePlatform;
			base.m_blnActiveDatabase = function.Pro_blnActiveDatabase;
			base.m_blnActiveLocalFile = function.Pro_blnActiveLocalFile;
		}
		i_edcLogger.Debug($"Assmebly MES Function {m_strFunctionName}   Platform:{base.m_blnActivePlatform}  Database:{base.m_blnActiveDatabase}  LocalFile:{base.m_blnActiveLocalFile}", null, ".ctor", 64);
	}

	public override bool Fun_blnContains()
	{
		return base.m_strRequest.IndexOf(Enum_MesFunction.AuslaufProtokollErstellen_Anfo_EL.Fun_strGetDescription(), StringComparison.OrdinalIgnoreCase) > 0;
	}

	public override void Sub_GetRequest()
	{
		m_Request = base.m_strRequest.Fun_DeserializeContent<Request_OutfeedCreateProtocol>();
		m_strCodes = Fun_strGetCode();
		m_bytTack = m_Request.m_sttHeader.m_bytTrackNumber;
	}

	public string Fun_strGetResponse()
	{
		m_Response = new Response_OutfeedCreateProtocol
		{
			m_sttHeader = new Struct_HeaderWithTrack
			{
				m_dtmTimestamp = DateTime.Now,
				m_strProcessName = m_Request.m_sttHeader.m_strProcessName,
				m_strLineName = m_Request.m_sttHeader.m_strLineName,
				m_strStationName = m_Request.m_sttHeader.m_strStationName,
				m_strMessageId = m_Request.m_sttHeader.m_strMessageId,
				m_bytTrackNumber = m_bytTack,
				m_strVersion = m_Request.m_sttHeader.m_strVersion
			},
			m_sttResult = r_edcResult
		};
		return SerializerHelper.Fun_strSerializerModel<Response_OutfeedCreateProtocol>(m_Response);
	}

	public override Task<string> Fun_strExecute()
	{
		string a_strResponse = string.Empty;
		try
		{
			Sub_GetRequest();
			if (base.m_blnActivePlatform)
			{
				base.m_edcLogger.Debug("Begin " + m_strFunctionName + ".Fun_blnConnectMesPlatform()", null, "Fun_strExecute", 115);
				Fun_blnConnectMesPlatform();
				base.m_edcLogger.Debug("End " + m_strFunctionName + ".Fun_blnConnectMesPlatform()", null, "Fun_strExecute", 117);
			}
			a_strResponse = Fun_strGetResponse();
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "MES Function 'Outfeed Create Protocol' Error...Details:'" + ex.Message + "'");
		}
		return Task.FromResult(a_strResponse);
	}

	public string Fun_strGetProtocolFullname()
	{
		try
		{
			return Path.Combine(m_Request.m_strSolderingProtocolPath, m_Request.m_strSolderingProtocolFilename);
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "MES Function 'Outfeed Create Protocol' Error...Details:'" + ex.Message + "'");
		}
		return string.Empty;
	}

	public string Fun_strGetCode()
	{
		string result = string.Empty;
		Edc_Identifier[] ma_sttIdentifier = m_Request.ma_sttIdentifier;
		foreach (Edc_Identifier item in ma_sttIdentifier)
		{
			if (item.m_enmCodeMeaning == Enum_CodeMeaning.Code)
			{
				result = result + item.m_strValue + ",";
			}
		}
		return result.TrimEnd(',');
	}
}
