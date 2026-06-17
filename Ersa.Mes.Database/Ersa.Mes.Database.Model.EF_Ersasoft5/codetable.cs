using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.codetables")]
public class codetable
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long codetableid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long groupid { get; set; }

	[StringLength(100)]
	public string name { get; set; }

	public DateTime? creationdate { get; set; }

	public long? creationuser { get; set; }

	public DateTime? changedate { get; set; }

	public long? changeuser { get; set; }
}
