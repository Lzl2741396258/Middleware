using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.solderinglibraries")]
public class solderinglibrary
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long libraryid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long groupid { get; set; }

	[Key]
	[Column(Order = 2)]
	[StringLength(100)]
	public string name { get; set; }

	[StringLength(250)]
	public string description { get; set; }

	public bool? deleted { get; set; }

	public DateTime? creationdate { get; set; }

	public long? creationuser { get; set; }

	public DateTime? changedate { get; set; }

	public long? changeuser { get; set; }
}
