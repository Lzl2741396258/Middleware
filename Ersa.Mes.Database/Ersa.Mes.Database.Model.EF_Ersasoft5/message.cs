using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.messages")]
public class message
{
	[Key]
	[Column(Order = 0)]
	[StringLength(36)]
	public string messageid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long machineid { get; set; }

	[Key]
	[Column(Order = 2)]
	public DateTime occurred { get; set; }

	public DateTime? acknowledged { get; set; }

	[Key]
	[Column(Order = 3)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long userid { get; set; }

	[Key]
	[Column(Order = 4)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int operationmode { get; set; }

	[Key]
	[Column(Order = 5)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int messagetype { get; set; }

	[Key]
	[Column(Order = 6)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int messagekey { get; set; }

	[Key]
	[Column(Order = 7)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int facility1key { get; set; }

	[Key]
	[Column(Order = 8)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int facility2key { get; set; }

	[Key]
	[Column(Order = 9)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int facility3key { get; set; }

	public int? system { get; set; }

	public int? code { get; set; }

	public DateTime? messagereset { get; set; }

	[StringLength(30)]
	public string possibleactions { get; set; }

	[StringLength(30)]
	public string requestedactions { get; set; }
}
