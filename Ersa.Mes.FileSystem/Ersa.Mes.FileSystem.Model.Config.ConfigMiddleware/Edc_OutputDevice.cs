using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;

public class Edc_OutputDevice
{
	[XmlArray("SerialPorts")]
	[XmlArrayItem("SerialPort")]
	public List<Edc_ConfigSerialPort> m_lstSerialPorts { get; set; }

	[XmlArray("Sockets")]
	[XmlArrayItem("Socket")]
	public List<Edc_Socket> m_lstSockets { get; set; }
}
