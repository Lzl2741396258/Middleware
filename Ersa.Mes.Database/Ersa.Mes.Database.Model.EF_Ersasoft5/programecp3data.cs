using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.programecp3data")]
public class programecp3data
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long historyid { get; set; }

	[Key]
	[Column(Order = 1)]
	[StringLength(20)]
	public string version { get; set; }

	[StringLength(250)]
	public string description { get; set; }

	public string modobject { get; set; }

	public string boundingcubes { get; set; }
}
