using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.protocolheads_ma55794")]
public class protocolheads_ma55794
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long protocolid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long historyid { get; set; }

	public bool? draft { get; set; }

	[Key]
	[Column(Order = 2)]
	public DateTime entrytime { get; set; }

	public DateTime? outlettime { get; set; }

	public long? userid { get; set; }

	public bool? faulty { get; set; }

	public long? serialboardnumber { get; set; }

	public long? mode { get; set; }

	public long? track { get; set; }
}
