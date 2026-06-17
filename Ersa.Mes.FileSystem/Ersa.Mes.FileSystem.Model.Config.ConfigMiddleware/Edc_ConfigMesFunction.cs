using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;

public class Edc_ConfigMesFunction
{
	[XmlAttribute("Name")]
	public string Pro_strName { get; set; } = string.Empty;


	[XmlAttribute("PlatformActive")]
	public bool Pro_blnActivePlatform { get; set; } = false;


	[XmlAttribute("DatabaseActive")]
	public bool Pro_blnActiveDatabase { get; set; } = false;


	[XmlAttribute("LocalFileActive")]
	public bool Pro_blnActiveLocalFile { get; set; } = false;

}
