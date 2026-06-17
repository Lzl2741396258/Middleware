using System;
using System.Runtime.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config;

[DataContract(Name = "test1", Namespace = "http://test2")]
public struct Struct_ConfigJson
{
	private string m_PathConfig;

	[IgnoreDataMember]
	public bool m_blnRead { get; set; }

	[DataMember(Name = "ClientIP")]
	public string m_strClientIP { get; set; }

	[DataMember(Name = "ClientPort")]
	public string m_strClientPort { get; set; }

	[DataMember(Name = "PathConfig")]
	public string m_strPathConfig
	{
		get
		{
			return AppDomain.CurrentDomain.BaseDirectory + "Configuration\\Config.json";
		}
		set
		{
			m_PathConfig = value;
		}
	}

	[DataMember(Name = "PathConfigTrace05")]
	public string m_strPathConfigTrace05 { get; set; }

	[DataMember(Name = "PathProtocol")]
	public string m_strPathProtocol { get; set; }

	[DataMember(Name = "PathTrend")]
	public string m_strPathTrend { get; set; }

	public void SUB_Init()
	{
		m_blnRead = true;
		m_strClientIP = string.Empty;
		m_strClientPort = string.Empty;
		m_strPathConfig = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\Config.json";
		m_strPathConfigTrace05 = string.Empty;
		m_strPathProtocol = string.Empty;
		m_strPathTrend = string.Empty;
	}
}
