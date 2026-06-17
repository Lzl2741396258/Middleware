using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Message;

[Serializable]
[XmlRoot("root")]
public class Edc_lstMessage
{
	[XmlArray("ErrorMessages")]
	[XmlArrayItem("ErrorMessage")]
	public Edc_ErrorMessage[] m_edcMessage { get; set; }
}
