using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.languageentries")]
public class languageentry
{
	[Key]
	[Column(Order = 0)]
	[StringLength(10)]
	public string language { get; set; }

	[Key]
	[Column(Order = 1)]
	[StringLength(10)]
	public string textkey { get; set; }

	[Key]
	[Column(Order = 2)]
	[StringLength(400)]
	public string text { get; set; }
}
