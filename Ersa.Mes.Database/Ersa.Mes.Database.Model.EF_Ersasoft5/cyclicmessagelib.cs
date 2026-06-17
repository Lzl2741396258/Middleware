using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.cyclicmessagelib")]
public class cyclicmessagelib
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long cycliclibid { get; set; }

	[Key]
	[Column(Order = 1)]
	[StringLength(100)]
	public string name { get; set; }

	public DateTime? creationdate { get; set; }

	public long? creationuser { get; set; }

	public DateTime? changedate { get; set; }

	public long? changeuser { get; set; }
}
