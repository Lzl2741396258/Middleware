using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.transferparameterheader")]
public class TransferParameterHeader
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[StringLength(50)]
	public string ProgramName { get; set; }

	[StringLength(50)]
	public string BibName { get; set; }

	[StringLength(50)]
	public string UserId { get; set; }

	[StringLength(50)]
	public string NumberOfPanel { get; set; }

	[StringLength(50)]
	public string BarNo { get; set; }

	[StringLength(50)]
	public string PanelNo { get; set; }

	[StringLength(50)]
	public string SubLotNo { get; set; }
}
