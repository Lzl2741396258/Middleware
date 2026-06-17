using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.machines")]
public class machine
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long machineid { get; set; }

	[Key]
	[Column(Order = 1)]
	[StringLength(40)]
	public string machinetype { get; set; }

	[Key]
	[Column(Order = 2)]
	[StringLength(30)]
	public string machinenumber { get; set; }

	public string comment { get; set; }

	[StringLength(100)]
	public string location { get; set; }

	[StringLength(100)]
	public string productionline { get; set; }

	public DateTime? creationdate { get; set; }

	public long? codetableid { get; set; }

	public long? defaultprogramid { get; set; }

	public long? protocolversion { get; set; }

	public long? recorderversion { get; set; }

	public long? operatingdataversion { get; set; }
}
