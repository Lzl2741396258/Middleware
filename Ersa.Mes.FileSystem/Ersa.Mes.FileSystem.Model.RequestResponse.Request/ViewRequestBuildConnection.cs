using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

[Serializable]
[XmlRoot("STRUCT_VerbindungAufbauen_Anfo_EL")]
public class ViewRequestBuildConnection
{
	[XmlAttribute("Zeitstempel")]
	public DateTime DateTime { get; set; }

	[XmlAttribute("Prozessname")]
	public string ProcessName { get; set; }

	[XmlAttribute("Linienname")]
	public string LineName { get; set; }

	[XmlAttribute("Stationsname")]
	public string StationName { get; set; }

	[XmlAttribute("MessageID")]
	public Guid MessageId { get; set; }

	[XmlAttribute("Spurnummer")]
	public string TrackNumber { get; set; }

	[XmlAttribute("Version")]
	public string Version { get; set; }

	[XmlElement("Maschinentyp")]
	public string MachineType { get; set; }
}
