using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.nozzleoperatingchanges")]
public class nozzleoperatingchanx
{
	[Key]
	[Column(Order = 0)]
	[StringLength(36)]
	public string nozzleguid { get; set; }

	[Key]
	[Column(Order = 1)]
	public DateTime changedate { get; set; }

	public long? userid { get; set; }

	public long? machineid { get; set; }

	public long? solderpot { get; set; }
}
