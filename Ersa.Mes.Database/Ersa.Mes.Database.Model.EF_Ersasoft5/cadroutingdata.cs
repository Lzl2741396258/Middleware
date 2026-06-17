using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.cadroutingdata")]
public class cadroutingdata
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long routingid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long historyid { get; set; }

	public int? steptype { get; set; }

	public int? mode { get; set; }

	public int? machinemodule { get; set; }

	public int? modulenumber { get; set; }

	public int? toolnumber { get; set; }

	public int? syncmode { get; set; }

	public int? syncid { get; set; }

	public float? startpointx { get; set; }

	public float? startpointy { get; set; }

	public float? endpointx { get; set; }

	public float? endpointy { get; set; }

	public int? syncpos { get; set; }
}
