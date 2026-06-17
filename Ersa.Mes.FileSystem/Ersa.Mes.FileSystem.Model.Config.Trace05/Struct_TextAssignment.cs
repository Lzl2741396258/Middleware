using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.Trace05;

[Serializable]
public struct Struct_TextAssignment
{
	[XmlElement("Fehlernummer")]
	public string ErrorNumber { get; set; }

	[XmlElement("Fehlertext")]
	public Struct_ErrorText ErrorText { get; set; }
}
