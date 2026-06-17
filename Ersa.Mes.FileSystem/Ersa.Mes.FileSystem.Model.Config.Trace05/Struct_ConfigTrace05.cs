using System;
using System.Xml.Serialization;
using Ersa.Mes.FileSystem.Model.RequestResponse;

namespace Ersa.Mes.FileSystem.Model.Config.Trace05;

[Serializable]
[XmlRoot("STRUCT_Configuration")]
public struct Struct_ConfigTrace05
{
	public bool m_blnRead { get; set; }

	[XmlElement("Version")]
	public string m_strVersion { get; set; }

	[XmlElement("MaschinenTyp")]
	public Enum_MachineType m_enuMachineType { get; set; }

	[XmlElement("Prozessname")]
	public string m_strProcessName { get; set; }

	[XmlElement("Linienname")]
	public string m_strLineName { get; set; }

	[XmlElement("Stationsname")]
	public string m_strStationsName { get; set; }

	[XmlElement("Aenderungsname")]
	public string m_strChangeName { get; set; }

	[XmlElement("ValidierungDurchfuehren")]
	public string m_strExecuteValidation { get; set; }

	[XmlElement("Kommunikationsweg")]
	public Enum_Communication m_enuCommunication { get; set; }

	[XmlElement("NameUndPfadSchema")]
	public string m_strSchemaPath { get; set; }

	[XmlElement("TimeoutPollen")]
	public int m_i32Timeout { get; set; }

	[XmlElement("ZyklusPollen")]
	public int m_i32CycleTime { get; set; }

	[XmlElement("EinstellungenTcpip")]
	public Struct_TcpipSettings m_sttSettingsTcpip { get; set; }

	[XmlElement("Verzeichnis")]
	public Struct_Diretory m_sttDiretory { get; set; }

	public void SUB_Init()
	{
		m_blnRead = true;
		m_strVersion = string.Empty;
		m_enuMachineType = Enum_MachineType.Reflow;
		m_strProcessName = string.Empty;
		m_strLineName = string.Empty;
		m_strStationsName = string.Empty;
		m_strChangeName = string.Empty;
		m_strExecuteValidation = string.Empty;
		m_enuCommunication = Enum_Communication.NichtDefiniert;
		m_strSchemaPath = string.Empty;
		m_i32Timeout = 0;
		m_i32CycleTime = 0;
		m_sttSettingsTcpip.SUB_Init();
	}
}
