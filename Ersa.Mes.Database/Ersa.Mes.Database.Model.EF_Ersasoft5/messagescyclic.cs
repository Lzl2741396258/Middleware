using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.messagescyclic")]
public class messagescyclic
{
	[Key]
	[Column(Order = 0)]
	[StringLength(36)]
	public string messageid { get; set; }

	[Key]
	[Column(Order = 1)]
	[StringLength(200)]
	public string message { get; set; }

	[StringLength(100)]
	public string facility1 { get; set; }

	[StringLength(100)]
	public string facility2 { get; set; }

	[StringLength(100)]
	public string facility3 { get; set; }

	public bool? infeedblocked { get; set; }
}
