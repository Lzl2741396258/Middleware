using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.machineoperatingdatavalue")]
public class machineoperatingdatavalue
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long operatingdataid { get; set; }

	[Key]
	[Column(Order = 1)]
	[StringLength(100)]
	public string dataid { get; set; }

	[StringLength(10)]
	public string namekey { get; set; }

	public long? timespan { get; set; }

	public long? percentage { get; set; }

	public long? value { get; set; }

	public float? realvalue { get; set; }

	public int? track { get; set; }
}
