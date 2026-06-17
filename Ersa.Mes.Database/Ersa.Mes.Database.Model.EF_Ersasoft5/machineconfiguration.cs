using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.machineconfigurations")]
public class machineconfiguration
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long configurationid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long machineid { get; set; }

	[StringLength(250)]
	public string description { get; set; }

	[Key]
	[Column(Order = 2)]
	public string filename { get; set; }

	[Key]
	[Column(Order = 3)]
	public string configuration { get; set; }

	[Key]
	[Column(Order = 4)]
	public DateTime creationdate { get; set; }

	[Key]
	[Column(Order = 5)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long creationuser { get; set; }
}
