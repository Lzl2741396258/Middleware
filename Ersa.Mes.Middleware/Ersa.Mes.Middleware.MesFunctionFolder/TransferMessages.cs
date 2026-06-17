using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.Common.Extensions;
using Ersa.Mes.Database.Model.EF_Ersasoft4;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.FileSystem.Model.Message;
using Ersa.Mes.FileSystem.Model.RequestResponse.Request;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Definition;
using Ersa.Mes.Middleware.Factory;

namespace Ersa.Mes.Middleware.MesFunctionFolder;

public class TransferMessages : Edc_MesFunction
{
	protected override string m_strFunctionName => "TransferMessages";

	public override bool m_blnResponse => false;

	public Request_TransferMessages m_Request { get; set; }

	public Enum_MessageState m_enuMessageState { get; set; } = Enum_MessageState.NotDefined;


	public Enum_MessageType m_enuMessageType { get; set; } = Enum_MessageType.NichtDefiniert;


	public string m_strAlarmCode { get; set; }

	public string m_strAlarmText { get; set; }

	public string m_strAlarmUser { get; set; }

	public string Pro_strAlarmNumber
	{
		get
		{
			MessageInTransferMessage a_sttMessage = m_Request.m_sttMessage;
			return a_sttMessage.m_strNumberMessageText.PadLeft(4, '0') + "." + a_sttMessage.m_strNumberFacility1.PadLeft(4, '0') + "." + a_sttMessage.m_strNumberFacility2.PadLeft(4, '0') + "." + a_sttMessage.m_strNumberFacility3.PadLeft(4, '0');
		}
	}

	public string Pro_strAlarmText
	{
		get
		{
			MessageInTransferMessage a_sttMessage = m_Request.m_sttMessage;
			return a_sttMessage.m_strMessageText + "_" + a_sttMessage.m_strTextFacility1 + "_" + a_sttMessage.m_strTextFacility2 + "_" + a_sttMessage.m_strTextFacility3;
		}
	}

	public TransferMessages(List<Edc_ConfigMesFunction> i_lstMesFunction, Inf_Logger i_edcLogger)
		: base(i_edcLogger)
	{
		Edc_ConfigMesFunction function = i_lstMesFunction.Where((Edc_ConfigMesFunction s) => s.Pro_strName.Equals(m_strFunctionName)).FirstOrDefault();
		if (function != null)
		{
			base.m_blnActivePlatform = function.Pro_blnActivePlatform;
			base.m_blnActiveDatabase = function.Pro_blnActiveDatabase;
			base.m_blnActiveLocalFile = function.Pro_blnActiveLocalFile;
		}
		i_edcLogger.Debug($"Assmebly MES Function {m_strFunctionName}   Platform:{base.m_blnActivePlatform}  Database:{base.m_blnActiveDatabase}  LocalFile:{base.m_blnActiveLocalFile}", null, ".ctor", 68);
	}

	public override bool Fun_blnContains()
	{
		return base.m_strRequest.IndexOf(Enum_MesFunction.MeldungenUebertragen_Anfo_EL.Fun_strGetDescription(), StringComparison.OrdinalIgnoreCase) > 0;
	}

	public override void Sub_GetRequest()
	{
		string a_strRequest = base.m_strRequest.Replace("&#x0;", "-");
		m_Request = a_strRequest.Fun_DeserializeContent<Request_TransferMessages>();
		try
		{
			m_enuMessageState = m_Request.m_sttMessage.m_enuState;
			m_enuMessageType = m_Request.m_sttMessage.m_enuType;
			m_strAlarmCode = Pro_strAlarmNumber;
			m_strAlarmText = Pro_strAlarmText;
			m_strAlarmUser = Pro_strAlarmUser();
		}
		catch
		{
			throw new Exception("Transfer Message StateReport is Error...");
		}
	}

	public override Task<string> Fun_strExecute()
	{
		try
		{
			Sub_GetRequest();
			if (base.m_blnActivePlatform)
			{
				base.m_edcLogger.Debug("Begin " + MethodBase.GetCurrentMethod().DeclaringType.Name + ".Fun_blnConnectMesPlatform()...", null, "Fun_strExecute", 111);
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
			OnShowMessage(Enum_LogType.Error, "Middleware -> 'Transfer Messages' Error...Details:'" + ex.Message + "'");
		}
		return Task.FromResult(string.Empty);
	}

	protected override void Sub_AddToDatabase()
	{
		try
		{
			using (new Ersasoft4())
			{
			}
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, ex.Message);
		}
	}

	public string Pro_strNumberAndText()
	{
		try
		{
			MessageInTransferMessage a_sttMessage = m_Request.m_sttMessage;
			return a_sttMessage.m_strNumberMessageText.PadLeft(4, '0') + "." + a_sttMessage.m_strNumberFacility1.PadLeft(4, '0') + "." + a_sttMessage.m_strNumberFacility2.PadLeft(4, '0') + "." + a_sttMessage.m_strNumberFacility3.PadLeft(4, '0') + " " + a_sttMessage.m_strMessageText + "_" + a_sttMessage.m_strTextFacility1 + "_" + a_sttMessage.m_strTextFacility2 + "_" + a_sttMessage.m_strTextFacility3;
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "MES Function 'Transfer Messages GetNumberAndText' Error...Details:'" + ex.Message + "'");
		}
		return string.Empty;
	}

	public virtual string Pro_strAlarmUser()
	{
		return m_Request.m_sttMessage.m_strUser;
	}
}
