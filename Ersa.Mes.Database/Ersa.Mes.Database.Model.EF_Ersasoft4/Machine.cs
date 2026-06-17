using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.machine")]
public class Machine
{
	[Key]
	[Column("machineid", Order = 1)]
	public int MachineId { get; set; } = 1;


	[Column("machinetype", Order = 2)]
	[StringLength(40)]
	public string MachineType { get; set; }

	[Column("machinenumber", Order = 3)]
	[StringLength(30)]
	public string MachineNumber { get; set; }

	[Column("creationdate", Order = 4)]
	public DateTime CreationDate { get; set; }

	[Column("codetableid", Order = 5)]
	public int CodeTableId { get; set; }

	[Column("defaultlibrary", Order = 6)]
	public int DefaultLibrary { get; set; }

	[Column("defaultdprogram", Order = 7)]
	public int DefaultProgram { get; set; }

	[Column("protocolversion", Order = 8)]
	public int ProtocolVersion { get; set; }
}
