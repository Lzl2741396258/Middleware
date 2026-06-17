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
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Definition;
using Ersa.Mes.Middleware.Factory;

namespace Ersa.Mes.Middleware.MesFunctionFolder;

public class ErsasoftTrigger : Edc_MesFunction
{
	public Request_ErsasoftTrigger m_Request;

	protected override string m_strFunctionName => "ErsasoftTrigger";

	public override bool m_blnResponse => true;

	public ErsasoftTrigger(List<Edc_ConfigMesFunction> i_lstMesFunction, Inf_Logger i_edcLogger)
		: base(i_edcLogger)
	{
		Edc_ConfigMesFunction function = i_lstMesFunction.Where((Edc_ConfigMesFunction s) => s.Pro_strName.Equals(m_strFunctionName)).FirstOrDefault();
		if (function != null)
		{
			base.m_blnActivePlatform = function.Pro_blnActivePlatform;
			base.m_blnActiveDatabase = function.Pro_blnActiveDatabase;
			base.m_blnActiveLocalFile = function.Pro_blnActiveLocalFile;
		}
		i_edcLogger.Debug($"Assmebly MES Function {m_strFunctionName}   Platform:{base.m_blnActivePlatform}  Database:{base.m_blnActiveDatabase}  LocalFile:{base.m_blnActiveLocalFile}", null, ".ctor", 54);
	}

	public override bool Fun_blnContains()
	{
		return base.m_strRequest.IndexOf(Enum_MesFunction.ErsasoftTriggern_Anfo_LE.Fun_strGetDescription(), StringComparison.OrdinalIgnoreCase) > 0;
	}

	public override void Sub_GetRequest()
	{
		m_Request = base.m_strRequest.Fun_DeserializeContent<Request_ErsasoftTrigger>();
	}

	public string Fun_strGetResponse()
	{
		Request_ErsasoftTrigger model = new Request_ErsasoftTrigger
		{
			m_sttHeader = new Struct_HeaderWithTrack
			{
				m_dtmTimestamp = DateTime.Now,
				m_strProcessName = "Reflow Soldering",
				m_strLineName = "01",
				m_strStationName = "01",
				m_strMessageId = Guid.NewGuid().ToString(),
				m_strVersion = "0.1"
			},
			m_enmMessageRequest = Enum_MessageRequest.MaschinenzustandMelden
		};
		return SerializerHelper.Fun_strSerializerModel<Request_ErsasoftTrigger>(model);
	}

	public override Task<string> Fun_strExecute()
	{
		string a_strResponse = string.Empty;
		try
		{
			Sub_GetRequest();
			if (base.m_blnActivePlatform)
			{
				base.m_edcLogger.Debug("Begin " + MethodBase.GetCurrentMethod().DeclaringType.Name + ".Fun_blnConnectMesPlatform()...", null, "Fun_strExecute", 106);
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
			OnShowMessage(Enum_LogType.Info, "Ersasoft -> Connected with Ersasoft");
			base.m_i32ActCount++;
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "MES Function 'Build Connect' Error...Details:'" + ex.Message + "'");
		}
		return Task.FromResult(a_strResponse);
	}
}
