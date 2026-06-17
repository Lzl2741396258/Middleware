using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.selectprogram")]
public class SelectProgram
{
	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[Column(Order = 2)]
	public DateTime TheTime { get; set; }

	[StringLength(50)]
	[Column(Order = 3)]
	public string CallIdentifier { get; set; }

	public string Guid { get; set; }

	[StringLength(50)]
	public string Library { get; set; }

	[StringLength(50)]
	public string Program { get; set; }

	public string ChangeStatus { get; set; }

	public string SolderingProgram { get; set; }

	[StringLength(50)]
	public string SolderingProgramErrorCode { get; set; }

	public string SolderingProgramErrorText { get; set; }
}
