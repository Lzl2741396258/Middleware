using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.codetablemembers")]
public class codetablemember
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long codememberid { get; set; }

	public long? codetableid { get; set; }

	public long? libraryid { get; set; }

	public long? programid { get; set; }

	[StringLength(500)]
	public string code { get; set; }
}
