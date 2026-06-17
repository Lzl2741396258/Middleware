using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.popupdialog")]
public class PopupDialog
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	public DateTime TheTime { get; set; }

	public string Request { get; set; }

	public string Response { get; set; }

	public string TerminalInformation { get; set; }

	public string ShowPopup { get; set; }

	public string MessageId { get; set; }

	public int Status { get; set; }
}
