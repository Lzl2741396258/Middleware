using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.Trace05;

[Serializable]
public struct Struct_Anfo
{
	[XmlElement("Anfo_EL")]
	public string m_strAnfoEL { get; set; }
}
