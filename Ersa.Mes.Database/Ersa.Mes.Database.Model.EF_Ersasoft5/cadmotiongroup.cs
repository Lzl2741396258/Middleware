using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.cadmotiongroups")]
public class cadmotiongroup
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int motiongroupid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long historyid { get; set; }

	public int? parentgroupid { get; set; }

	public int? rank { get; set; }

	public int? equipment { get; set; }

	public int? machinemodule { get; set; }

	public int? modulenumber { get; set; }

	public int? toolnumber { get; set; }

	public bool? allowautorouting { get; set; }
}
