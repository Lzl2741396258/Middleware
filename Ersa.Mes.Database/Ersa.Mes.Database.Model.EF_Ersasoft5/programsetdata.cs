using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.programsetdata")]
public class programsetdata
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long historyid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int rownumber { get; set; }

	public int? sortorder { get; set; }

	[Key]
	[Column(Order = 2)]
	[StringLength(1000)]
	public string rowcontent { get; set; }
}
