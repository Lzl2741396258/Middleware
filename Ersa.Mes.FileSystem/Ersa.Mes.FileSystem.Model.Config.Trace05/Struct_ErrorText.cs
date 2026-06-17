using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.Trace05;

[Serializable]
public struct Struct_ErrorText
{
	[XmlElement("Deutsch")]
	public string German { get; set; }

	[XmlElement("Englisch")]
	public string English { get; set; }
}
