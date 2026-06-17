using System;
using System.ComponentModel;

namespace Ersa.Mes.FileSystem.Model.Message;

public class Edc_Message
{
	[Description("ID")]
	public int m_i32ID { get; set; }

	[Description("Typ")]
	public string m_strType { get; set; } = "NotDefined";


	[Description("Nummer")]
	public string m_strNumber { get; set; }

	[Description("Ort1")]
	public string m_strFacility1 { get; set; }

	[Description("Ort2")]
	public string m_strFacility2 { get; set; }

	[Description("Ort3")]
	public string m_strFacility3 { get; set; }

	[Description("Meldungstext")]
	public string m_strMessageText { get; set; }

	[Description("Betriebsart")]
	public string m_strMachineMode { get; set; }

	[Description("Tag_aufgetreten")]
	public DateTime m_dtmOccurredDate { get; set; }

	[Description("Zeit_aufgetreten")]
	public DateTime m_dtmOccurredTime { get; set; }

	[Description("Tag_quittiert")]
	public DateTime m_dtmAcknowledgedDate { get; set; }

	[Description("Zeit_quittiert")]
	public DateTime m_dtmAcknowledgedTime { get; set; }

	[Description("Bediener")]
	public string m_strOperator { get; set; }
}
