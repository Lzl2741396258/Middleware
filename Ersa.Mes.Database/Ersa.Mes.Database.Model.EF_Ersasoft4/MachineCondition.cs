using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.machinecondition")]
public class MachineCondition
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	public DateTime TheTime { get; set; }

	[StringLength(50)]
	public string MessageID { get; set; }

	[StringLength(20)]
	public string MachineType { get; set; }

	[StringLength(4)]
	public string MDECode { get; set; }

	public string CodeChangeStatus { get; set; }

	[StringLength(30)]
	public string MDECodeText { get; set; }

	public int ProductsInMachine { get; set; }

	[StringLength(50)]
	public string ProductsInMachineChangeStatus { get; set; }

	public string SolderingProgram { get; set; }

	public int ElapsedSecond { get; set; }
}
