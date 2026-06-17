using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.protocolvariables")]
public class protocolvariable
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long machineid { get; set; }

	[Key]
	[Column(Order = 1)]
	[StringLength(200)]
	public string variable { get; set; }

	[StringLength(160)]
	public string namekeyarray { get; set; }

	[StringLength(10)]
	public string unitkey { get; set; }

	[Key]
	[Column(Order = 2)]
	[StringLength(4)]
	public string columnname { get; set; }

	[Key]
	[Column(Order = 3)]
	[StringLength(15)]
	public string datatype { get; set; }
}
