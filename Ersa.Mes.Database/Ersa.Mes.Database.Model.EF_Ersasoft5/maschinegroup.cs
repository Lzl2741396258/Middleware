using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.maschinegroups")]
public class maschinegroup
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long groupid { get; set; }

	[StringLength(50)]
	public string groupname { get; set; }

	[StringLength(40)]
	public string machinetype { get; set; }

	public long? programversion { get; set; }

	public long? protocolversion { get; set; }

	public long? recorderversion { get; set; }

	public long? operatingdataversion { get; set; }
}
