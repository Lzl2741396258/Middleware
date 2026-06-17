using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.users")]
public class user
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long userid { get; set; }

	[Key]
	[Column(Order = 1)]
	[StringLength(50)]
	public string username { get; set; }

	[Key]
	[Column(Order = 2)]
	[StringLength(20)]
	public string passwordsalt { get; set; }

	[Key]
	[Column(Order = 3)]
	[StringLength(28)]
	public string passwordhash { get; set; }

	public DateTime? createdat { get; set; }

	public bool? isdefault { get; set; }

	public bool? isextern { get; set; }

	[StringLength(200)]
	public string code { get; set; }

	public bool? isactive { get; set; }

	public bool? isactiveafterautologout { get; set; }

	public int? permissions { get; set; }

	[Key]
	[Column(Order = 4)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long machineid { get; set; }
}
