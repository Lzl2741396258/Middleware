using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.linkcache")]
public class linkcache
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long linkid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long machineid { get; set; }

	[Key]
	[Column(Order = 2)]
	[StringLength(32)]
	public string hash { get; set; }

	[Key]
	[Column(Order = 3)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int usage { get; set; }

	[Key]
	[Column(Order = 4)]
	[StringLength(1000)]
	public string link { get; set; }

	[Key]
	[Column(Order = 5)]
	public DateTime creationdate { get; set; }
}
