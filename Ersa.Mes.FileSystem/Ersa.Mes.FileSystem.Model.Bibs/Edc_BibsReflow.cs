using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Bibs;

[Serializable]
[XmlRoot("SOLDER_PRG")]
public class Edc_BibsReflow
{
	[XmlIgnore]
	public bool m_blnActive = false;

	[XmlElement("Version")]
	public string m_strVersion { get; set; } = string.Empty;


	[XmlElement("PrgName")]
	public m_edcProgram m_edcProgram { get; set; } = new m_edcProgram();


	[XmlElement("Bib")]
	public Bib m_edcBib { get; set; } = new Bib();


	[XmlElement("Info")]
	public Info m_edcInfo { get; set; } = new Info();


	[XmlElement("Benutzer")]
	public Bibs_User m_edcUser { get; set; } = new Bibs_User();


	[XmlElement("History")]
	public History m_edcHistory { get; set; } = new History();


	[XmlElement("STRUCT_Heizungen")]
	public Struct_Heaters m_edcHeaters { get; set; } = new Struct_Heaters();


	[XmlElement("STRUCT_Pyrolyse")]
	public Struct_Pyrolyse m_edcPyrolyse { get; set; } = new Struct_Pyrolyse();


	[XmlElement("STRUCT_Pyrometer")]
	public Struct_Pyrometer m_edcPyrometer { get; set; } = new Struct_Pyrometer();


	[XmlElement("STRUCT_Breiten")]
	public Struct_ConveryWidth m_edcConveryWidth { get; set; } = new Struct_ConveryWidth();


	[XmlElement("STRUCT_Transporte")]
	public Struct_Convery m_edcConverySpeed { get; set; } = new Struct_Convery();


	[XmlElement("STRUCT_N2")]
	public Struct_O2N2 m_edcO2N2 { get; set; } = new Struct_O2N2();


	[XmlElement("STRUCT_Schnell")]
	public Struct_Fast m_edcFast { get; set; } = new Struct_Fast();

}
