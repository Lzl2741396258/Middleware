using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.pcbinfeed")]
public class PcbInfeed
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	public DateTime TheTime { get; set; }

	[StringLength(50)]
	public string CallIdentifier { get; set; }

	[StringLength(50)]
	public string Charge { get; set; }

	[StringLength(8000)]
	public string Identifier { get; set; }

	public int TrackNo { get; set; }

	[StringLength(8000)]
	public string Produktname { get; set; }

	[StringLength(8000)]
	public string ParameterGroup { get; set; }
}
