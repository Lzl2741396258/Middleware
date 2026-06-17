using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.ztxt")]
public class Ztxt
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	public DateTime? TheTime { get; set; }

	[StringLength(8000)]
	public string TimeDataFrom { get; set; }

	public int TimeManual { get; set; }

	public int TimeProduction { get; set; }

	public int TimeAuto { get; set; }

	public int TimeTotal { get; set; }

	public int TimeCongestion { get; set; }

	public int ConsumedPower { get; set; }

	public int ProducedPCBs1 { get; set; }

	public int DefectivePCBs1 { get; set; }

	public int TotalPCBs1 { get; set; }

	public int ProducedPCBs2 { get; set; }

	public int DefectivePCBs2 { get; set; }

	public int TotalPCBs2 { get; set; }

	public int ProducedPCBs3 { get; set; }

	public int DefectivePCBs3 { get; set; }

	public int TotalPCBs3 { get; set; }

	public int ProducedPCBs4 { get; set; }

	public int DefectivePCBs4 { get; set; }

	public int TotalPCBs4 { get; set; }
}
