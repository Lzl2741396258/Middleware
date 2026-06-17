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
using Ersa.Mes.Middleware.Model;

namespace Ersa.Mes.Middleware.MesFunctionFolder;

public class TransferProcessParameter : Edc_MesFunction
{
	public Request_TransferProcessParameter m_Request;

	protected override string m_strFunctionName => "TransferProcessParameter";

	public override bool m_blnResponse => false;

	private Enum_GetParameterType m_enuGetParameter { get; set; } = Enum_GetParameterType.Test;


	public List<Struct_ProcessParameter> m_lstParameters { get; set; }

	public TransferProcessParameter(Enum_GetParameterType i_enuGetParameter, List<Edc_ConfigMesFunction> i_lstMesFunction, Inf_Logger i_edcLogger)
		: base(i_edcLogger)
	{
		Edc_ConfigMesFunction function = i_lstMesFunction.Where((Edc_ConfigMesFunction s) => s.Pro_strName.Equals(m_strFunctionName)).FirstOrDefault();
		if (function != null)
		{
			base.m_blnActivePlatform = function.Pro_blnActivePlatform;
			base.m_blnActiveDatabase = function.Pro_blnActiveDatabase;
			base.m_blnActiveLocalFile = function.Pro_blnActiveLocalFile;
		}
		m_enuGetParameter = i_enuGetParameter;
		i_edcLogger.Debug($"Assmebly MES Function {m_strFunctionName}   Platform:{base.m_blnActivePlatform}  Database:{base.m_blnActiveDatabase}  LocalFile:{base.m_blnActiveLocalFile}", null, ".ctor", 57);
	}

	public override bool Fun_blnContains()
	{
		return base.m_strRequest.IndexOf(Enum_MesFunction.ParameterUebertragen_Anfo_EL.Fun_strGetDescription(), StringComparison.OrdinalIgnoreCase) > 0;
	}

	public override void Sub_GetRequest()
	{
		m_Request = base.m_strRequest.Fun_DeserializeContent<Request_TransferProcessParameter>();
		m_lstParameters = m_Request.m_sttParameterGroup.ma_sttParameter.ToList();
	}

	public override Task<string> Fun_strExecute()
	{
		try
		{
			Sub_GetRequest();
			if (base.m_blnActivePlatform)
			{
				base.m_edcLogger.Debug("Begin " + MethodBase.GetCurrentMethod().DeclaringType.Name + ".Fun_blnConnectMesPlatform()...", null, "Fun_strExecute", 91);
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
			OnShowProcess(++base.m_i32ActCount);
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "Middleware -> 'Transfer Process Parameter' Error...Details:'" + ex.Message + "'");
		}
		return Task.FromResult(string.Empty);
	}

	protected override Task Fun_blnConnectMesPlatform()
	{
		return Task.FromResult(result: true);
	}

	private List<Struct_ProcessParameter> Fun_lstGetParameters(Enum_GetParameterType i_enuGetParameter)
	{
		switch (i_enuGetParameter)
		{
		case Enum_GetParameterType.Protocol:
			m_lstParameters = Fun_lstGetParametersProtocol();
			break;
		case Enum_GetParameterType.ALL:
			m_lstParameters = Fun_lstGetParametersAll();
			break;
		}
		return m_lstParameters;
	}

	private List<Struct_ProcessParameter> Fun_lstGetParametersAll()
	{
		return m_Request.m_sttParameterGroup.ma_sttParameter.ToList();
	}

	private List<Struct_ProcessParameter> Fun_lstGetParametersProtocol()
	{
		List<Struct_ProcessParameter> a_lstParameters = new List<Struct_ProcessParameter>();
		List<Struct_ProcessParameter> list = Fun_lstGetParametersAll();
		ProcessParameter p1 = new ProcessParameter();
		Type type = p1.GetType();
		BindingFlags flag = BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;
		FieldInfo[] fields = type.GetFields(flag);
		foreach (FieldInfo item in fields)
		{
			string i_strName = item.Name;
			string i_strValue = item.GetValue(item).ToString();
			switch (i_strName)
			{
			case "m_strTemperatureSet":
			case "m_strTemperatureActual":
			case "m_strRate":
			{
				int i;
				for (i = 0; i < 50; i++)
				{
					m_lstParameters.Add(m_lstParameters.Find((Struct_ProcessParameter s) => s.m_strName.Equals($"{i_strValue}[{i}]") && s.m_bytTrackNumber == 1));
				}
				break;
			}
			case "m_strBlower":
			{
				int j;
				for (j = 0; j < 7; j++)
				{
					m_lstParameters.Add(m_lstParameters.Find((Struct_ProcessParameter s) => s.m_strName.Equals($"{i_strValue}[{j}]") && s.m_bytTrackNumber == 1));
				}
				break;
			}
			default:
				m_lstParameters.Add(m_lstParameters.Find((Struct_ProcessParameter s) => s.m_strName.Equals(i_strValue) && s.m_bytTrackNumber == 1));
				break;
			}
		}
		return m_lstParameters;
	}
}
