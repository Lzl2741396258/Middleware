using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.protocolparameter_ma54369")]
public class protocolparameter_ma54369
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long protocolid { get; set; }

	[Key]
	[Column(Order = 1)]
	[StringLength(100)]
	public string parameter { get; set; }

	[StringLength(500)]
	public string content { get; set; }

	[StringLength(1)]
	public string type { get; set; }
}
