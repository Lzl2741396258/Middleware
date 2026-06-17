using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

[Serializable]
[XmlRoot("ParameterGruppe")]
public struct Struct_ParameterGroup
{
	[XmlAttribute("Name")]
	[JsonProperty(PropertyName = "Name")]
	public string m_strName { get; set; }

	[XmlAttribute("Event")]
	[JsonProperty(PropertyName = "Event")]
	public string m_strEvent { get; set; }

	[XmlAttribute("Periodenzaehler")]
	[JsonProperty(PropertyName = "Interval")]
	public int m_i32Interval { get; set; }

	[XmlElement("Parameter")]
	[JsonProperty(PropertyName = "Parameter")]
	public Struct_ProcessParameter[] ma_sttParameter { get; set; }
}
