using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.aoistepdata")]
public class aoistepdata
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long programid { get; set; }

	[Key]
	[Column(Order = 1)]
	[StringLength(36)]
	public string stepguid { get; set; }

	[Key]
	[Column(Order = 2)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int panel { get; set; }

	[MaxLength(int.MaxValue)]
	public byte[] binaries { get; set; }
}
