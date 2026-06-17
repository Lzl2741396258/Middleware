using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.programcontentchangerecord")]
public class ProgramContentChangeRecord
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	public DateTime TheTime { get; set; }

	public string History { get; set; }

	public string Library { get; set; }

	public string Program { get; set; }
}
