using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Protocol;

[Serializable]
[XmlRoot("ERSA")]
public class Edc_ProtocolElementSelectiveDefault
{
	[XmlElement("LoetProtokoll")]
	public Edc_ProtocolLoet m_edcProtocolLoet { get; set; }
}
