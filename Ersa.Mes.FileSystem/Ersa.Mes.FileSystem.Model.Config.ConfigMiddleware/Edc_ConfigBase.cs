using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;

[Serializable]
[XmlRoot("root")]
public class Edc_ConfigBase
{
	[XmlElement("InterfaceAddress")]
	public Edc_InterfaceAddress m_edcInterfaceAddress { get; set; }

	[XmlElement("PLC")]
	public Edc_PLC m_sttPLC { get; set; }

	[XmlArray("InterfaceAddresses")]
	[XmlArrayItem("InterfaceAddress")]
	public Edc_InterfaceAddress[] m_edcInterfaceAddresses { get; set; }

	[XmlElement("FilesPath")]
	public Edc_FilesPath m_edcFilesPath { get; set; } = new Edc_FilesPath();


	[XmlArray("MesFunctions")]
	[XmlArrayItem("MesFunction")]
	public List<Edc_ConfigMesFunction> m_lstMesFunction { get; set; }

	[XmlArray("MesTasks")]
	[XmlArrayItem("MesTask")]
	public Edc_MesTaskAttributes[] ma_MesTask { get; set; }

	[XmlElement("Device")]
	public Edc_Device m_clsDevice { get; set; } = new Edc_Device();


	[XmlElement("BasicSettings")]
	public Edc_BasicSettings m_clsBasicSettings { get; set; }

	[XmlElement("Database")]
	public Edc_Database m_clsDatabase { get; set; }

	[XmlElement("FTP")]
	public Edc_FTP m_clsFTP { get; set; }

	public Edc_ConfigBase(string i_strPathErsasoft = "")
	{
		m_edcFilesPath.m_strPathErsasoft = i_strPathErsasoft;
	}
}
