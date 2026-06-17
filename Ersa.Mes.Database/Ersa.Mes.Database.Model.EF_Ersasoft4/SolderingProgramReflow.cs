using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.solderingprogramreflow")]
public class SolderingProgramReflow
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[StringLength(10)]
	public string Version { get; set; }

	[StringLength(50)]
	public string LibraryName { get; set; }

	[StringLength(50)]
	public string ProgramName { get; set; }

	public DateTime? CreateTime { get; set; }

	public DateTime? LastEditTime { get; set; }

	[StringLength(50)]
	public string Author { get; set; }

	public string Content { get; set; }

	public bool IsVariable { get; set; }

	public int HistoryItems { get; set; }
}
