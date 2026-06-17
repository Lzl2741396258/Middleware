using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.transferparameterBase")]
public class TransferParameterBase
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column(Order = 1)]
	public int Id { get; set; }

	[Column(Order = 2)]
	public int TransferParameterId { get; set; }

	public string blnWarteAufPcb { get; set; }

	public string blnStauNachfolger { get; set; }

	public string stfDate { get; set; }

	public string dwdPcbCounter { get; set; }

	public string dwdProcessTime { get; set; }

	public string dwdLengthPCB { get; set; }

	public string dwdLengthPCBSoll { get; set; }

	public string dwdPCBHoleIgnore { get; set; }

	public string stfProdukt { get; set; }

	public string stfProg { get; set; }

	public string stfBib { get; set; }

	public string stfZeitstempel { get; set; }

	public string stfBarcode { get; set; }

	public string stfUserErsasoft { get; set; }

	public string sntMode { get; set; }

	public string sntError { get; set; }

	public string sntConveyor { get; set; }
}
