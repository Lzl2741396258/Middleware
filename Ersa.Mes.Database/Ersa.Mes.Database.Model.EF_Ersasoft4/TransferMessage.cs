using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.transfermessage")]
public class TransferMessage
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	public string MessageType { get; set; }

	[StringLength(4)]
	public string Facility1 { get; set; }

	[StringLength(4)]
	public string Facility2 { get; set; }

	[StringLength(4)]
	public string Facility3 { get; set; }

	[StringLength(4)]
	public string FacilityText { get; set; }

	[StringLength(50)]
	public string Text1 { get; set; }

	[StringLength(50)]
	public string Text2 { get; set; }

	[StringLength(50)]
	public string Text3 { get; set; }

	[StringLength(50)]
	public string MessageText { get; set; }

	[StringLength(50)]
	public string Status { get; set; }

	[StringLength(20)]
	public string OperatingMode { get; set; }

	public DateTime? OccurredDate { get; set; }

	public DateTime? AcknowledgedDate { get; set; }

	[StringLength(20)]
	public string Operator { get; set; }
}
