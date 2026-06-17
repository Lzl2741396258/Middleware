using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;

public class Edc_Device
{
	[XmlAttribute("HearderEndActive")]
	public bool m_blnHearderEndActive { get; set; } = false;


	[XmlElement("CameraNoRead")]
	public string m_strCameraNoRead { get; set; } = string.Empty;


	[XmlArray("SerialPorts")]
	[XmlArrayItem("SerialPort")]
	public List<Edc_ConfigSerialPort> m_lstSerialPorts { get; set; } = new List<Edc_ConfigSerialPort>();


	[XmlArray("Sockets")]
	[XmlArrayItem("Socket")]
	public List<Edc_Socket> m_lstSockets { get; set; } = new List<Edc_Socket>();


	[XmlArray("Processes")]
	[XmlArrayItem("Process")]
	public List<Edc_Process> m_lstProcess { get; set; } = new List<Edc_Process>();

}
