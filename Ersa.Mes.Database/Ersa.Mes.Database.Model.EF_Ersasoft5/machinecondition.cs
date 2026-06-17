using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.machineconditions")]
public class machinecondition
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long conditionid { get; set; }

	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long machineid { get; set; }

	[Key]
	[Column(Order = 2)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int oeecode { get; set; }

	[Key]
	[Column(Order = 3)]
	public DateTime time { get; set; }

	public int track { get; set; }
}
