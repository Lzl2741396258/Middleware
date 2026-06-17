using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.aoiresults")]
public class aoiresult
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long resultid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int aoitype { get; set; }

	[Key]
	[Column(Order = 2)]
	public string panelcode { get; set; }

	[Key]
	[Column(Order = 3)]
	[StringLength(36)]
	public string stepguid { get; set; }

	[Key]
	[Column(Order = 4)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long machineid { get; set; }

	[Key]
	[Column(Order = 5)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long programid { get; set; }

	[Key]
	[Column(Order = 6)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int panel { get; set; }

	[StringLength(32)]
	public string hash { get; set; }

	public int? result { get; set; }

	public bool? manual { get; set; }

	public string data { get; set; }

	[Key]
	[Column(Order = 7)]
	public DateTime creationdate { get; set; }

	[MaxLength(int.MaxValue)]
	public byte[] binaries { get; set; }
}
