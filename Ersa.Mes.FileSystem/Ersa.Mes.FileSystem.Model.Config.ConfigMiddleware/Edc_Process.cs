using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;

public class Edc_Process
{
	[XmlAttribute("Name")]
	public string m_strName { get; set; } = string.Empty;


	[XmlAttribute("Input")]
	public string m_strInput { get; set; }

	[XmlAttribute("Output")]
	public string m_strOutput { get; set; }

	[XmlAttribute("InputDataFormat")]
	public string m_strInputDataFormat { get; set; }

	[XmlAttribute("InputDataRepeat")]
	public bool m_blnInputDataRepeat { get; set; }

	[XmlAttribute("OutputDataFormat")]
	public string m_strOutputDataFormat { get; set; }

	[XmlAttribute("OutputDataRepeat")]
	public bool m_blnOutputDataRepeat { get; set; }
}
