using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.transferparameterpyro")]
public class TransferParameterPyro
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	public int Active { get; set; }

	public long Sollwert { get; set; }

	public long Istwert { get; set; }

	public long TolPos { get; set; }

	public long TolNeg { get; set; }

	public long MaxUeberPcb { get; set; }

	public int ToleranceError { get; set; }

	public long PcbDiffInGrad { get; set; }

	public long PcbDiffRelativ { get; set; }
}
