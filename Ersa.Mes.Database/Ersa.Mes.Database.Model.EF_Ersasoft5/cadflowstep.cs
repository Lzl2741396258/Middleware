using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.cadflowsteps")]
public class cadflowstep
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long flowstepid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long historyid { get; set; }

	public float? startpointx { get; set; }

	public float? startpointy { get; set; }

	public float? endpointx { get; set; }

	public float? endpointy { get; set; }

	public int? steptype { get; set; }

	public long? geomertyid { get; set; }

	public int? rank { get; set; }

	public int? motiongroupid { get; set; }

	public int? mode { get; set; }
}
