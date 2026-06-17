using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.placerprograms")]
public class placerprogram
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long programid { get; set; }

	public string setting { get; set; }

	public string data { get; set; }

	public string pipeline { get; set; }
}
