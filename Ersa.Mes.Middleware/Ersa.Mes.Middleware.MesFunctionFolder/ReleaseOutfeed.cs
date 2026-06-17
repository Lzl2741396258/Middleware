using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.Common.Extensions;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.FileSystem.Model.Protocol;
using Ersa.Mes.FileSystem.Model.RequestResponse.Request;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Definition;
using Ersa.Mes.Middleware.Factory;

namespace Ersa.Mes.Middleware.MesFunctionFolder;

public class ReleaseOutfeed : Edc_MesFunction
{
	public Request_ReleaseOutfeed m_Request;

	public Edc_ProtocolSelectiveZevi m_ProtocolZvei = new Edc_ProtocolSelectiveZevi();

	protected override string m_strFunctionName => "ReleaseOutfeed";

	public override bool m_blnResponse => false;

	public ReleaseOutfeed(List<Edc_ConfigMesFunction> i_lstMesFunction, Inf_Logger i_edcLogger)
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
		return base.m_strRequest.IndexOf(Enum_MesFunction.AuslaufFreigeben_Anfo_EL.Fun_strGetDescription(), StringComparison.OrdinalIgnoreCase) > 0;
	}

	public override void Sub_GetRequest()
	{
		m_Request = base.m_strRequest.Fun_DeserializeContent<Request_ReleaseOutfeed>();
	}

	public override Task<string> Fun_strExecute()
	{
		try
		{
			Sub_GetRequest();
			if (base.m_blnActivePlatform)
			{
				base.m_edcLogger.Debug("Begin " + MethodBase.GetCurrentMethod().DeclaringType.Name + ".Fun_blnConnectMesPlatform()...", null, "Fun_strExecute", 86);
				Task.Run((Func<Task>)Fun_blnConnectMesPlatform);
			}
			Task.Run((Action)Sub_AddToDatabase);
			OnShowMessage(Enum_LogType.Info, "Ersasoft -> 'Release Outfeed'...");
			base.m_i32ActCount++;
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "Ersasoft -> 'Release Outfeed' Error...Details:'" + ex.Message + "'");
		}
		return Task.FromResult(string.Empty);
	}

	public virtual void Sub_CreateProtocol(string i_strPath)
	{
	}
}
