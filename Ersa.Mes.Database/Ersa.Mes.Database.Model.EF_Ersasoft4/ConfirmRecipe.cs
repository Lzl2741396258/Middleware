using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.confirmrecipe")]
public class ConfirmRecipe
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column(Order = 0)]
	public int Id { get; set; }

	[Column(Order = 1)]
	public DateTime TheTime { get; set; }

	[StringLength(50)]
	[Column(Order = 2)]
	public string CallIdentifier { get; set; }

	[StringLength(8000)]
	[Column(Order = 3)]
	public string Identifier { get; set; }

	[StringLength(50)]
	[Column(Order = 4)]
	public string Library111 { get; set; }

	[StringLength(50)]
	[Column(Order = 5)]
	public string Program { get; set; }

	[StringLength(20)]
	[Column(Order = 6)]
	public string SolderingProgramStartStatus { get; set; }

	[Column(Order = 7)]
	public string SolderingProgramTargetValue { get; set; }
}
