using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.messagecontext")]
public class messagecontext
{
	[Key]
	[StringLength(36)]
	public string messageid { get; set; }

	public string details { get; set; }

	public string context { get; set; }
}
