using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.packages")]
public class package
{
	[Key]
	[Column(Order = 0)]
	[StringLength(66)]
	public string packageid { get; set; }

	[Key]
	[Column(Order = 1)]
	[StringLength(40)]
	public string packagename { get; set; }

	public float? length { get; set; }

	public float? width { get; set; }

	public float? height { get; set; }

	public string textfile { get; set; }

	[MaxLength(int.MaxValue)]
	public byte[] bytearray { get; set; }

	public int? type { get; set; }

	public int? numberofpins { get; set; }

	public float? pitch { get; set; }
}
