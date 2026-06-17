using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.codecache")]
public class codecache
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long cacheid { get; set; }

	public long? machineid { get; set; }

	[Key]
	[Column(Order = 1)]
	[StringLength(32)]
	public string hash { get; set; }

	[Key]
	[Column(Order = 2)]
	[StringLength(100)]
	public string usage { get; set; }

	[StringLength(100)]
	public string meaning { get; set; }

	[Key]
	[Column(Order = 3)]
	[StringLength(400)]
	public string code { get; set; }

	[Key]
	[Column(Order = 4)]
	public DateTime creationdate { get; set; }

	public long? arrayindex { get; set; }
}
