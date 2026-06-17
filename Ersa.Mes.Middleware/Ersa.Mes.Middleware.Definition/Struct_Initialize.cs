using System;
using System.Xml.Serialization;
using Ersa.Mes.FileSystem.Model.RequestResponse;

namespace Ersa.Mes.Middleware.Definition;

[Serializable]
[XmlRoot("STRUCT_ParameterGruppeNameEventIntervallWiederholungen")]
public class Struct_Initialize
{
	[XmlAttribute("Name")]
	public string m_strName;

	[XmlAttribute("AnzahlWiederholungen")]
	public int m_i32RepetationNumber;

	[XmlAttribute("Intervall")]
	public int m_i32Interval;

	[XmlAttribute("Event")]
	public string m_strEvent;

	[XmlElement("Parameter")]
	public Struct_ParameterWithoutValue[] m_sttParameterWithoutValue;
}
