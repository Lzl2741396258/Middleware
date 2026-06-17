using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.protocoldata_ma18")]
public class protocoldata_ma18
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long protocolid { get; set; }

	public float? c001 { get; set; }

	public bool? c002 { get; set; }

	public float? c003 { get; set; }

	public float? c004 { get; set; }

	public float? c005 { get; set; }

	public float? c006 { get; set; }

	public float? c007 { get; set; }
}
