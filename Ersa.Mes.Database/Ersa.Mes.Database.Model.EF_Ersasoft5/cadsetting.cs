using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.cadsettings")]
public class cadsetting
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long historyid { get; set; }

	public string setting { get; set; }

	public string data { get; set; }
}
