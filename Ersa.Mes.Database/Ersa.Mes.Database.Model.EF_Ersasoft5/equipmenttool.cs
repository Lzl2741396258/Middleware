using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.equipmenttools")]
public class equipmenttool
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long equipmenttoolid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long equipmentid { get; set; }

	[Key]
	[Column(Order = 2)]
	[StringLength(200)]
	public string identification { get; set; }

	public bool? deleted { get; set; }

	public string setting { get; set; }
}
