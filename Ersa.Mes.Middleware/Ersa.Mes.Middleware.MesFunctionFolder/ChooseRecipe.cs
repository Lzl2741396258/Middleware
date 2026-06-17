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

public class ChooseRecipe : Edc_MesFunction
{
	public Request_ChooseRecipe m_Request;

	public Response_ChooseRecipe m_ResponseChooseRecipe;

	protected override string m_strFunctionName => "ChooseRecipe";

	public override bool m_blnResponse => true;

	protected string m_strCode { get; set; } = string.Empty;


	protected Edc_Result r_edcResult { get; set; } = new Edc_Result();


	protected string r_strCurrentRecipe { get; set; } = string.Empty;


	public ChooseRecipe(List<Edc_ConfigMesFunction> i_lstMesFunction, Inf_Logger i_edcLogger)
		: base(i_edcLogger)
	{
		Edc_ConfigMesFunction function = i_lstMesFunction.Where((Edc_ConfigMesFunction s) => s.Pro_strName.Equals(m_strFunctionName)).FirstOrDefault();
		if (function != null)
		{
			base.m_blnActivePlatform = function.Pro_blnActivePlatform;
			base.m_blnActiveDatabase = function.Pro_blnActiveDatabase;
			base.m_blnActiveLocalFile = function.Pro_blnActiveLocalFile;
		}
		i_edcLogger.Debug($"Assmebly MES Function {m_strFunctionName}   Platform:{base.m_blnActivePlatform}  Database:{base.m_blnActiveDatabase}  LocalFile:{base.m_blnActiveLocalFile}", null, ".ctor", 73);

	}

	public override bool Fun_blnContains()
	{
		return base.m_strRequest.IndexOf(Enum_MesFunction.RezeptAuswaehlen_Anfo_EL.Fun_strGetDescription(), StringComparison.OrdinalIgnoreCase) > 0;
	}

	public override void Sub_GetRequest()
	{
		m_Request = base.m_strRequest.Fun_DeserializeContent<Request_ChooseRecipe>();
		if (m_Request != null)
		{
			m_strCode = Fun_strGetCode();
		}
	}

	protected virtual string Fun_strGetResponse()
	{
		m_ResponseChooseRecipe = new Response_ChooseRecipe
		{
			Header = new Struct_HeaderWithTrack
			{
				m_dtmTimestamp = m_Request.m_sttHeader.m_dtmTimestamp,
				m_strProcessName = m_Request.m_sttHeader.m_strProcessName,
				m_strLineName = m_Request.m_sttHeader.m_strLineName,
				m_strStationName = m_Request.m_sttHeader.m_strStationName,
				m_strMessageId = m_Request.m_sttHeader.m_strMessageId,
				m_strVersion = m_Request.m_sttHeader.m_strVersion
			},
			Result = r_edcResult,
			RecipeCode = r_strCurrentRecipe
		};
		return SerializerHelper.Fun_strSerializerModel<Response_ChooseRecipe>(m_ResponseChooseRecipe);
	}

	public override Task<string> Fun_strExecute()
	{
		string a_strResponse = string.Empty;
		try
		{
			Sub_GetRequest();
			if (base.m_blnActivePlatform)
			{
				Fun_blnConnectMesPlatform();
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
		}
		catch (Exception ex)
		{
			a_strResponse = string.Empty;
			OnShowMessage(Enum_LogType.Error, "'ChooseRecipe' Error...Details:'" + ex.Message + "'");
		}
		base.m_edcLogger.Debug("End  " + m_strFunctionName, null, "Fun_strExecute", 159);
		return Task.FromResult(a_strResponse);
	}

	protected string Fun_strGetCode()
	{
		string a_strCode = string.Empty;
		if (m_Request.m_sttIdentifier.m_enmCodeMeaning == Enum_CodeMeaning.Code)
		{
			a_strCode = m_Request.m_sttIdentifier.m_strValue;
		}
		return a_strCode;
	}

	protected byte Fun_u32GetTrack()
	{
		return m_Request.m_sttHeader.m_bytTrackNumber;
	}
}
