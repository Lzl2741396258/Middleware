using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Protocol;

[Serializable]
[XmlRoot("unitData")]
public class Edc_ProtocolSelectiveZeviVF4 : Edc_ProtocolSelectiveZevi
{
	[XmlAttribute("Operator")]
	public string m_strOperator { get; set; }

	[XmlArray("productionResources")]
	[XmlArrayItem("resource")]
	public Edc_Resource[] ma_strResource { get; set; }
}
