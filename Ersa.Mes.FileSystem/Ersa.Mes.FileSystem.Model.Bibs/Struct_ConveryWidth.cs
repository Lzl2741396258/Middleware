using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Bibs;

public class Struct_ConveryWidth
{
	[XmlElement("a_sttBr_0_.bytActive")]
	public int? SpreadActive0 { get; set; } = 0;


	[XmlElement("a_sttBr_0_.lngSollwert")]
	public double? LaneWidth0 { get; set; } = 0.0;


	[XmlElement("a_sttBr_1_.bytActive")]
	public int? SpreadActive1 { get; set; } = 0;


	[XmlElement("a_sttBr_1_.lngSollwert")]
	public double? LaneWidth1 { get; set; } = 0.0;


	[XmlIgnore]
	public int m_i32ZuordnungTr { get; set; }

	[XmlElement("a_sttBr_1_.intZuordnungTr")]
	public string m_strZuordnungTr
	{
		get
		{
			return m_i32ZuordnungTr.ToString();
		}
		set
		{
			int.TryParse(value.ToString(), out var num1);
			m_i32ZuordnungTr = num1;
		}
	}

	[XmlElement("a_sttBr_2_.bytActive")]
	public byte SpreadActive2 { get; set; }

	[XmlElement("a_sttBr_2_.lngSollwert")]
	public long LaneWidth2 { get; set; }

	[XmlElement("a_sttBr_3_.bytActive")]
	public int? SpreadActive3 { get; set; } = 0;


	[XmlElement("a_sttBr_3_.lngSollwert")]
	public double? LaneWidth3 { get; set; } = 0.0;


	[XmlElement("a_sttBr_4_.bytActive")]
	public int? SpreadActive4 { get; set; } = 0;


	[XmlElement("a_sttBr_4_.lngSollwert")]
	public double? LaneWidth4 { get; set; } = 0.0;


	[XmlElement("a_sttBr_5_.bytActive")]
	public int? SpreadActive5 { get; set; } = 0;


	[XmlElement("a_sttBr_5_.lngSollwert")]
	public double? LaneWidth5 { get; set; } = 0.0;


	[XmlElement("a_sttBr_6_.bytActive")]
	public int? SpreadActive6 { get; set; } = 0;


	[XmlElement("a_sttBr_6_.lngSollwert")]
	public double? LaneWidth6 { get; set; } = 0.0;


	[XmlElement("a_sttBr_7_.bytActive")]
	public int? SpreadActive7 { get; set; } = 0;


	[XmlElement("a_sttBr_7_.lngSollwert")]
	public double? LaneWidth7 { get; set; } = 0.0;


	[XmlElement("a_sttBr_8_.bytActive")]
	public int? SpreadActive8 { get; set; } = 0;


	[XmlElement("a_sttBr_8_.lngSollwert")]
	public double? LaneWidth8 { get; set; } = 0.0;


	[XmlElement("a_sttBr_9_.bytActive")]
	public int? SpreadActive9 { get; set; } = 0;


	[XmlElement("a_sttBr_9_.lngSollwert")]
	public double? LaneWidth9 { get; set; } = 0.0;


	[XmlElement("a_sttBr_10_.bytActive")]
	public int? SpreadActive10 { get; set; } = 0;


	[XmlElement("a_sttBr_10_.lngSollwert")]
	public double? LaneWidth10 { get; set; } = 0.0;


	[XmlElement("a_sttBr_11_.bytActive")]
	public int? SpreadActive11 { get; set; } = 0;


	[XmlElement("a_sttBr_11_.lngSollwert")]
	public double? LaneWidth11 { get; set; } = 0.0;

}
