using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.nozzleoperatingactualvalues")]
public class nozzleoperatingactualvalue
{
	[Key]
	[StringLength(36)]
	public string nozzleguid { get; set; }

	public long? geomertyid { get; set; }

	public long? machineid { get; set; }

	public long? solderpot { get; set; }

	public DateTime? usestartdate { get; set; }

	public DateTime? useenddate { get; set; }

	public long? waveontime { get; set; }

	public long? waveofftime { get; set; }

	public long? totaltime { get; set; }

	public long? dispencenumber { get; set; }
}
