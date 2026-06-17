using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.codeconfigurationtrackings")]
public class codeconfigurationtracking
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long trackid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long arrayindex { get; set; }

	[Key]
	[Column(Order = 2)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long machineid { get; set; }

	[Key]
	[Column(Order = 3)]
	public bool isconfigured { get; set; }

	public bool? isactive { get; set; }

	public long? location { get; set; }

	public long? track { get; set; }

	public long? codefunction { get; set; }

	public bool? usealb { get; set; }

	public bool? useelb { get; set; }

	public long? timeout { get; set; }

	public DateTime? creationdate { get; set; }

	public long? creationuser { get; set; }
}
