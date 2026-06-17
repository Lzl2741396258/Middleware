using System.ComponentModel;

namespace Ersa.Mes.FileSystem.Model.Data;

public class Edc_DataRecord
{
	[Description("Column1")]
	public string m_strEditTime { get; set; }

	[Description("Column2")]
	public string m_strUser { get; set; }

	[Description("Column3")]
	public string m_strPlace { get; set; }

	[Description("Column4")]
	public string m_strPLCAddress { get; set; }

	[Description("Column5")]
	public string m_strOldValue { get; set; }

	[Description("Column6")]
	public string n_strNewValue { get; set; }
}
