using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.programimages")]
public class programimage
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long programid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int purpose { get; set; }

	[Key]
	[Column(Order = 2)]
	[MaxLength(int.MaxValue)]
	public byte[] image { get; set; }

	[StringLength(250)]
	public string filename { get; set; }

	[StringLength(50)]
	public string metainfo1 { get; set; }

	[StringLength(50)]
	public string metainfo2 { get; set; }

	[StringLength(50)]
	public string metainfo3 { get; set; }

	[StringLength(50)]
	public string metainfo4 { get; set; }

	[StringLength(50)]
	public string metainfo5 { get; set; }

	[StringLength(50)]
	public string metainfo6 { get; set; }
}
