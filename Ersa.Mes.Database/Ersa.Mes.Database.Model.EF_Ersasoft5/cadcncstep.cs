using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.cadcncsteps")]
public class cadcncstep
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long flowstepid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long historyid { get; set; }

	[Key]
	[Column(Order = 2)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int cncrang { get; set; }

	public int? cnctype { get; set; }

	public float? parameter1 { get; set; }

	public float? parameter2 { get; set; }

	public float? parameter3 { get; set; }

	public float? parameter4 { get; set; }
}
