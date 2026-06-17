using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.Common.Extensions;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.FileSystem.Model.RequestResponse.Request;
using Ersa.Mes.FileSystem.Model.RequestResponse.Response;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Definition;
using Ersa.Mes.Middleware.Factory;

namespace Ersa.Mes.Middleware.MesFunctionFolder;

public class Initialize : Edc_MesFunction
{
	protected override string m_strFunctionName => "Initialize";

	public override bool m_blnResponse => true;

	public Request_Initialize m_Request { get; set; }

	public Response_Initialize m_Response { get; set; }

	public string m_strPathInitialize { get; set; }

	public Initialize(string i_strPathInitialize, Inf_Logger i_edcLogger)
		: base(i_edcLogger)
	{
		m_strPathInitialize = i_strPathInitialize;
		i_edcLogger.Debug($"Assmebly MES Function {m_strFunctionName}   Platform:{base.m_blnActivePlatform}  Database:{base.m_blnActiveDatabase}  LocalFile:{base.m_blnActiveLocalFile}", null, ".ctor", 51);
	}

	public override bool Fun_blnContains()
	{
		return base.m_strRequest.IndexOf(Enum_MesFunction.Initialisierung_Anfo_EL.Fun_strGetDescription(), StringComparison.OrdinalIgnoreCase) > 0;
	}

	public override void Sub_GetRequest()
	{
		m_Request = base.m_strRequest.Fun_DeserializeContent<Request_Initialize>();
	}

	public override Task<string> Fun_strExecute()
	{
		string a_strResponse = string.Empty;
		try
		{
			Sub_GetRequest();
			if (base.m_blnActivePlatform)
			{
				base.m_edcLogger.Debug("Begin " + MethodBase.GetCurrentMethod().DeclaringType.Name + ".Fun_blnConnectMesPlatform()...", null, "Fun_strExecute", 85);
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
			if (!string.IsNullOrEmpty(a_strResponse) && m_Response != null)
			{
				Struct_ParameterGroupWithoutValue item = m_Response.m_sttParameterGroupWithoutValue[0];
				OnShowMessage(Enum_LogType.Info, $"Middleware -> 'Initialize' is Triggered...Name:'{item.m_strGroupName}' Parameter Count:'{item.ma_sttParameterWithoutValue.Length}'  Interval:'{item.m_intInterval}'");
			}
			base.m_i32ActCount++;
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "Middleware -> 'Initialize' Error...Details:'" + ex.Message + "'");
		}
		return Task.FromResult(a_strResponse);
	}

	public string Fun_strGetResponse()
	{
		if (!File.Exists(m_strPathInitialize))
		{
			OnShowMessage(Enum_LogType.Error, "Middleware -> Path of Initialize file is not exist...");
			return string.Empty;
		}
		List<Struct_ParameterGroupWithoutValue> list = new List<Struct_ParameterGroupWithoutValue>();
		try
		{
			Struct_ParameterGroupWithoutValue a_sttParameterGroupWithoutValue = Sub_LoadLocalFile();
			list.Add(a_sttParameterGroupWithoutValue);
			OnShowMessage(Enum_LogType.Info, "Middleware -> 'Initailize' Local File Successed...");
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "Middleware -> 'Initailize' Local File Failed...Detail:'" + ex.Message + "'");
		}
		m_Response = new Response_Initialize
		{
			Header = new Struct_HeaderWithTrack
			{
				m_dtmTimestamp = DateTime.Now,
				m_strProcessName = m_Request.m_sttHeader.m_strProcessName,
				m_strLineName = m_Request.m_sttHeader.m_strLineName,
				m_strStationName = m_Request.m_sttHeader.m_strStationName,
				m_strMessageId = m_Request.m_sttHeader.m_strMessageId,
				m_bytTrackNumber = 0,
				m_strVersion = m_Request.m_sttHeader.m_strVersion
			},
			m_sttParameterGroupWithoutValue = list.ToArray()
		};
		return SerializerHelper.Fun_strSerializerModel<Response_Initialize>(m_Response);
	}

	private List<Struct_ParameterGroupWithoutValue> Sub_CreateTestUserId()
	{
		Struct_ParameterWithoutValue a_sttParameterWithoutValue = new Struct_ParameterWithoutValue
		{
			m_strName = "UserId",
			m_bytTrackNumber = 0
		};
		List<Struct_ParameterGroupWithoutValue> list = new List<Struct_ParameterGroupWithoutValue>();
		Struct_ParameterGroupWithoutValue struct_ParameterGroupWithoutValue = default(Struct_ParameterGroupWithoutValue);
		struct_ParameterGroupWithoutValue.m_strGroupName = "Default Interval";
		struct_ParameterGroupWithoutValue.m_strEvent = string.Empty;
		struct_ParameterGroupWithoutValue.m_intInterval = 30;
		struct_ParameterGroupWithoutValue.m_i32NumberofRepetitions = 10;
		struct_ParameterGroupWithoutValue.ma_sttParameterWithoutValue = new Struct_ParameterWithoutValue[1] { a_sttParameterWithoutValue };
		Struct_ParameterGroupWithoutValue item = struct_ParameterGroupWithoutValue;
		list.Add(item);
		return list;
	}

	private Struct_ParameterGroupWithoutValue Sub_LoadLocalFile()
	{
		Struct_Initialize item = m_strPathInitialize.Fun_edcDeserializeByFilePath<Struct_Initialize>();
		Struct_ParameterGroupWithoutValue result = default(Struct_ParameterGroupWithoutValue);
		result.m_strGroupName = item.m_strName;
		result.m_intInterval = item.m_i32Interval;
		result.m_i32NumberofRepetitions = item.m_i32RepetationNumber;
		result.ma_sttParameterWithoutValue = item.m_sttParameterWithoutValue;
		result.m_strEvent = string.Empty;
		return result;
	}
}
