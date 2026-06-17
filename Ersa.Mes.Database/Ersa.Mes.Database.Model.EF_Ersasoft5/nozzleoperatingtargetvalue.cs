using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.nozzleoperatingtargetvalues")]
public class nozzleoperatingtargetvalue
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long geomertyid { get; set; }

	public long? waveontargettime { get; set; }

	public long? waveofftargettime { get; set; }

	public long? totaltargettime { get; set; }

	public long? dispencetargetnumber { get; set; }
}
