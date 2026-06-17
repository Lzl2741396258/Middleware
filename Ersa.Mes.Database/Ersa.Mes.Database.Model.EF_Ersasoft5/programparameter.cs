using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.programparameter")]
public class programparameter
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long historyid { get; set; }

	[Key]
	[Column(Order = 1)]
	[StringLength(100)]
	public string variable { get; set; }

	[StringLength(10)]
	public string datatype { get; set; }

	[Key]
	[Column(Order = 2)]
	[StringLength(50)]
	public string value { get; set; }
}
