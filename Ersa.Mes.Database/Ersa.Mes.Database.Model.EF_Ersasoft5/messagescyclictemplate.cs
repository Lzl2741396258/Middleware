using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.messagescyclictemplates")]
public class messagescyclictemplate
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long templateid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long cycliclibid { get; set; }

	[Key]
	[Column(Order = 2)]
	public bool activ { get; set; }

	[Key]
	[Column(Order = 3)]
	[StringLength(200)]
	public string message { get; set; }

	[StringLength(100)]
	public string facility1 { get; set; }

	[StringLength(100)]
	public string facility2 { get; set; }

	[StringLength(100)]
	public string facility3 { get; set; }

	public int? interval { get; set; }

	public int? time1 { get; set; }

	public int? time2 { get; set; }

	public bool? blockinfeedactive { get; set; }

	public int? blockinfeedafter { get; set; }

	public int? numberofresets { get; set; }

	public int? periodofresets { get; set; }
}
