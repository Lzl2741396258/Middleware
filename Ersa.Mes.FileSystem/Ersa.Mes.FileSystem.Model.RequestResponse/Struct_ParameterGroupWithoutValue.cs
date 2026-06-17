using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

[Serializable]
[XmlRoot("STRUCT_ParameterGruppeNameEventIntervallWiederholungen")]
public struct Struct_ParameterGroupWithoutValue
{
	[XmlAttribute("Name")]
	public string m_strGroupName;

	[XmlAttribute("AnzahlWiederholungen")]
	public int m_i32NumberofRepetitions;

	[XmlAttribute("Intervall")]
	public int m_intInterval;

	[XmlAttribute("Event")]
	public string m_strEvent;

	[XmlElement("Parameter")]
	public Struct_ParameterWithoutValue[] ma_sttParameterWithoutValue;
}
