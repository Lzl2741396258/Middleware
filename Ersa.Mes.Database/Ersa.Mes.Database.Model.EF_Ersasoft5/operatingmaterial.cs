using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.operatingmaterial")]
public class operatingmaterial
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long materialid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int type { get; set; }

	[Key]
	[Column(Order = 2)]
	[StringLength(100)]
	public string name { get; set; }

	[StringLength(200)]
	public string specification { get; set; }

	public bool? deleted { get; set; }
}
