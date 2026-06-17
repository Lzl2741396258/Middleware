using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.parameter")]
public class parameter
{
	[Key]
	[Column("parameter")]
	[StringLength(40)]
	public string parameter1 { get; set; }

	[StringLength(250)]
	public string content { get; set; }

	public long? value { get; set; }
}
