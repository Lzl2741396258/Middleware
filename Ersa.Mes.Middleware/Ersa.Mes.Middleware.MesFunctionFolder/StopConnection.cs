using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

public class StopConnection : Edc_MesFunction
{
	protected override string m_strFunctionName => "StopConnection";

	public override bool m_blnResponse => true;

	public Request_CloseConnection m_Request { get; set; }

	public Response_CloseConnection m_Response { get; set; }

	public StopConnection(List<Edc_ConfigMesFunction> i_lstMesFunction, Inf_Logger i_edcLogger)
		: base(i_edcLogger)
	{
		Edc_ConfigMesFunction function = i_lstMesFunction.Where((Edc_ConfigMesFunction s) => s.Pro_strName.Equals(m_strFunctionName)).FirstOrDefault();
		if (function != null)
		{
			base.m_blnActivePlatform = function.Pro_blnActivePlatform;
			base.m_blnActiveDatabase = function.Pro_blnActiveDatabase;
			base.m_blnActiveLocalFile = function.Pro_blnActiveLocalFile;
		}
		i_edcLogger.Debug($"Assmebly MES Function {m_strFunctionName}   Platform:{base.m_blnActivePlatform}  Database:{base.m_blnActiveDatabase}  LocalFile:{base.m_blnActiveLocalFile}", null, ".ctor", 52);
	}

	public override bool Fun_blnContains()
	{
		return base.m_strRequest.IndexOf(Enum_MesFunction.VerbindungBeenden_Anfo_EL.Fun_strGetDescription(), StringComparison.OrdinalIgnoreCase) > 0;
	}

	public override void Sub_GetRequest()
	{
		m_Request = base.m_strRequest.Fun_DeserializeContent<Request_CloseConnection>();
	}

	public string Fun_strGetResponse()
	{
		m_Response = new Response_CloseConnection
		{
			Header = new Struct_HeaderWithTrack
			{
				m_dtmTimestamp = DateTime.Now,
				m_strLineName = m_Request.m_sttHeader.m_strLineName,
				m_strMessageId = m_Request.m_sttHeader.m_strMessageId,
				m_strProcessName = m_Request.m_sttHeader.m_strProcessName,
				m_strStationName = m_Request.m_sttHeader.m_strStationName,
				m_strVersion = m_Request.m_sttHeader.m_strVersion
			},
			Result = new Edc_Result
			{
				m_enuResultCode = Enum_ResponseCode.ok,
				m_i32Number = 0,
				m_strText = string.Empty
			}
		};
		return SerializerHelper.Fun_strSerializerModel<Response_CloseConnection>(m_Response);
	}

	public override Task<string> Fun_strExecute()
	{
		string a_strResponse = string.Empty;
		try
		{
			Sub_GetRequest();
			if (base.m_blnActivePlatform)
			{
				base.m_edcLogger.Debug("Begin " + MethodBase.GetCurrentMethod().DeclaringType.Name + ".Fun_blnConnectMesPlatform()...", null, "Fun_strExecute", 110);
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
			a_strResponse = Fun_strGetResponse();
			OnShowMessage(Enum_LogType.Info, "Ersasoft -> Disconnect from Ersasoft");
			base.m_i32ActCount++;
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "MES Function 'Stop Connection' Error...Details:'" + ex.Message + "'");
		}
		return Task.FromResult(a_strResponse);
	}
}
