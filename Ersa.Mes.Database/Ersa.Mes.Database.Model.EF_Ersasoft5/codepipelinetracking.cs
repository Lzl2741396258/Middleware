using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.codepipelinetrackings")]
public class codepipelinetracking
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long trackid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long arrayindex { get; set; }

	public long? machineid { get; set; }

	public long? branchnr { get; set; }

	public long? pipeelement { get; set; }

	public string content { get; set; }

	public DateTime? creationdate { get; set; }

	public long? creationuser { get; set; }
}
