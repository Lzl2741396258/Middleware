using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.initialize")]
public class Initialize
{
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	public DateTime TheTime { get; set; }

	[StringLength(50)]
	public string GroupName { get; set; }

	public int? NumberofRepetitions { get; set; }

	public int? Interval { get; set; }

	[StringLength(50)]
	public string Event { get; set; }

	public string sttParameterWithoutValue { get; set; }
}
