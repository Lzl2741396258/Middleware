using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.productioncontrol")]
public class productioncontrol
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long productioncontrolid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long machineid { get; set; }

	[StringLength(250)]
	public string description { get; set; }

	public string adjustments { get; set; }

	public bool? isactive { get; set; }

	public DateTime? creationdate { get; set; }

	public long? creationuser { get; set; }

	public DateTime? changedate { get; set; }

	public long? changeuser { get; set; }
}
