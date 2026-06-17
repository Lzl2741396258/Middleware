using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Bibs;

public class Struct_MuHeight
{
	[XmlElement("a_sttBr_0_.bytActive")]
	public int? BrMuHeightActive0 { get; set; } = 0;


	[XmlElement("a_sttBr_0_.lngSollwert")]
	public double? BrMuHeightSetValue0 { get; set; } = 0.0;


	[XmlElement("a_sttBr_1_.bytActive")]
	public int? BrMuHeightActive1 { get; set; } = 0;


	[XmlElement("a_sttBr_1_.lngSollwert")]
	public double? BrMuHeightSetValue1 { get; set; } = 0.0;


	[XmlElement("a_sttBr_2_.bytActive")]
	public int? BrMuHeightActive2 { get; set; } = 0;


	[XmlElement("a_sttBr_2_.lngSollwert")]
	public double? BrMuHeightSetValue2 { get; set; } = 0.0;


	[XmlElement("a_sttBr_3_.bytActive")]
	public int? BrMuHeightActive3 { get; set; } = 0;


	[XmlElement("a_sttBr_3_.lngSollwert")]
	public double? BrMuHeightSetValue3 { get; set; } = 0.0;

}
