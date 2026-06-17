using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.transferparameterconvery")]
public class TransferParameterConvery
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	public string sntConveyor { get; set; }

	public string sntCenterSupportActive { get; set; }

	public string intSetValueSpeed { get; set; }

	public string intActualValueSpeed { get; set; }

	public string intPositionFesterHolm { get; set; }

	public string intPositionBreite { get; set; }

	public string intPositionCenter { get; set; }
}
