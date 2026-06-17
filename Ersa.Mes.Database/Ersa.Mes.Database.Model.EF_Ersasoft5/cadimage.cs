using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.cadimages")]
public class cadimage
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

	public float? width { get; set; }

	public float? height { get; set; }

	public int? viewside { get; set; }

	public float? pixelpermmx { get; set; }

	public float? pixelpermmy { get; set; }

	public float? zerooffsetx { get; set; }

	public float? zerooffsety { get; set; }

	public float? matrix00 { get; set; }

	public float? matrix01 { get; set; }

	public float? matrix10 { get; set; }

	public float? matrix11 { get; set; }
}
