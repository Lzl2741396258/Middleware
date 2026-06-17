using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.transferuserinput")]
public class TransferUserInput
{
	public int Id { get; set; }

	[StringLength(100)]
	public string CallIdentifier { get; set; }

	[StringLength(50)]
	public string User { get; set; }

	public DateTime? LoginTime { get; set; }

	public DateTime? LogOutnTime { get; set; }

	[StringLength(50)]
	public string OeeCoding { get; set; }

	[StringLength(50)]
	public string ChangeStatus { get; set; }

	[StringLength(50)]
	public string OeeCodeTest { get; set; }

	[StringLength(100)]
	public string ChangedPlcParameter { get; set; }

	[StringLength(100)]
	public string ChangedTextParameter { get; set; }

	[StringLength(50)]
	public string ValueBeforeChange { get; set; }

	[StringLength(50)]
	public string ValueAfterChange { get; set; }
}
