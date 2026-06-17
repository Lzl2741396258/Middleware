using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.transferparametero2n2")]
public class TransferParameterO2N2
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	public string sntN2 { get; set; }

	public string a_sntN2_0 { get; set; }

	public string a_sntN2_1 { get; set; }

	public string sntCoolSwitched { get; set; }

	public string intSetValueN2 { get; set; }

	public string a_intSetValueN2Mess2_0 { get; set; }

	public string a_intSetValueN2Mess2_1 { get; set; }

	public string intN2Druck { get; set; }

	public string intN2Durchfluss { get; set; }

	public string intSetValueO2 { get; set; }

	public string a_intSetValueO2Mess2_0 { get; set; }

	public string a_intSetValueO2Mess2_1 { get; set; }

	public string intActualValueO2 { get; set; }

	public string a_intActualValueO2Mess2_0 { get; set; }

	public string a_intActualValueO2Mess2_1 { get; set; }
}
