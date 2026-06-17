using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.packagemacros")]
public class packagemacro
{
	[Key]
	[Column(Order = 0)]
	[StringLength(66)]
	public string packageid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long geomertyid { get; set; }

	[Key]
	[Column(Order = 2)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long category { get; set; }

	public string macro { get; set; }

	public int? soldertemp { get; set; }

	public int? envtemp { get; set; }

	public string pcbdata { get; set; }
}
