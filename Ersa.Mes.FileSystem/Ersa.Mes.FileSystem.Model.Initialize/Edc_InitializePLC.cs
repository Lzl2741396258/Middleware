using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Initialize;

[Serializable]
[XmlRoot("Struct_Plc")]
public class Edc_InitializePLC
{
	[XmlAttribute("Name")]
	[Description("Name")]
	public string m_strName { get; set; }

	[XmlAttribute("Repetitions")]
	[Description("Repetitions")]
	public int m_i32Repetitions { get; set; }

	[XmlAttribute("Interval")]
	[Description("Interval")]
	public int m_i32Interval { get; set; }

	[XmlAttribute("Event")]
	[Description("Event")]
	public string m_strEvent { get; set; }

	[XmlElement("Parameter")]
	[Description("Parameter")]
	public List<Edc_ParameterPLC> m_lstParameterPLC { get; set; }
}
