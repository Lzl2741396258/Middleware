using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.nozzlegeometries")]
public class nozzlegeometry
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long geomertyid { get; set; }

	[Key]
	[Column(Order = 1)]
	public float innerdiameter { get; set; }

	[Key]
	[Column(Order = 2)]
	public float outerdiameter { get; set; }

	[Key]
	[Column(Order = 3)]
	public float height { get; set; }

	[Key]
	[Column(Order = 4)]
	public bool visible { get; set; }

	[Key]
	[Column(Order = 5)]
	public bool special { get; set; }

	public int? specialtype { get; set; }
}
