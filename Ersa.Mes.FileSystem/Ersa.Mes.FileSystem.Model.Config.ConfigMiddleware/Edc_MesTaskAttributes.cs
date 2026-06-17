using System.ComponentModel;
using System.Xml.Serialization;
using Ersa.Mes.FileSystem.Interface;

namespace Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;

public class Edc_MesTaskAttributes : Inf_MesTaskAttributes
{
	[Description("Name")]
	[XmlAttribute("Name")]
	public string Pro_strName { get; set; } = string.Empty;


	[Description("Active")]
	[XmlAttribute("Active")]
	public bool Pro_blnActive { get; set; } = false;


	[Description("Interval")]
	[XmlAttribute("Interval")]
	public int Pro_i32Interval { get; set; } = 1000;


	[Description("DelayTime")]
	[XmlAttribute("DelayTime")]
	public int Pro_i32DelayTime { get; set; } = 1000;


	[Description("Repetition")]
	[XmlAttribute("Repetition")]
	public int Pro_i32Repetition { get; set; } = 1;

}
