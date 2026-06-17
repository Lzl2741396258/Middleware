using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.programhistory")]
public class programhistory
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long historyid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long programid { get; set; }

	public string notes { get; set; }

	public DateTime? creationdate { get; set; }

	public long? creationuser { get; set; }

	public int? status { get; set; }

	public int? setnumber { get; set; }

	public DateTime? changedate { get; set; }

	public long? changeuser { get; set; }

	public int? releasestate { get; set; }

	public string releasenotes { get; set; }
}
