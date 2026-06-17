using System.IO.Ports;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;

public class Edc_ConfigSerialPort
{
	[XmlAttribute("Active")]
	public bool m_blnActive { get; set; }

	[XmlAttribute("Com")]
	public string m_strPortName { get; set; }

	[XmlAttribute("Port")]
	public int m_i32Rate { get; set; } = 9600;


	[XmlAttribute("Parity")]
	public Parity m_edcParity { get; set; } = Parity.None;


	[XmlAttribute("DataBits")]
	public int m_i32DataBits { get; set; } = 8;


	[XmlAttribute("StopBits")]
	public StopBits m_strStopBits { get; set; } = StopBits.One;


	[XmlAttribute("DtrEnable")]
	public bool m_blnDtrEnable { get; set; } = false;


	[XmlAttribute("RtsEnable")]
	public bool m_blnRtsEnable { get; set; } = false;


	[XmlAttribute("TrackId")]
	public ushort m_i32TrackId { get; set; } = 1;


	[XmlAttribute("Remark")]
	public string m_strRemark { get; set; } = string.Empty;

}
