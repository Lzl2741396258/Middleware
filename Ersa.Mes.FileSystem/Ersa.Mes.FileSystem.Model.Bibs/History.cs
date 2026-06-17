using System.Collections;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Bibs;

public class History
{
	[XmlElement("Items")]
	public int m_i32Items { get; set; } = 0;


	[XmlIgnore]
	public ArrayList ma_strHistory { get; set; } = new ArrayList();

}
