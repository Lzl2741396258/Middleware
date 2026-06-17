using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.usertrackings")]
public class usertracking
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long trackid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long userid { get; set; }

	[Key]
	[Column(Order = 2)]
	public DateTime time { get; set; }

	[StringLength(8)]
	public string activity { get; set; }

	[Key]
	[Column(Order = 3)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long machineid { get; set; }

	[StringLength(200)]
	public string parameter { get; set; }

	[StringLength(200)]
	public string oldvalue { get; set; }

	[StringLength(200)]
	public string newvalue { get; set; }
}
