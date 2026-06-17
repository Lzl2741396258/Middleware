using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.paneldata")]
public class paneldata
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long paneldataid { get; set; }

	[Key]
	[Column(Order = 1)]
	[StringLength(32)]
	public string hash { get; set; }

	[Key]
	[Column(Order = 2)]
	[StringLength(400)]
	public string panelcode { get; set; }

	[Key]
	[Column(Order = 3)]
	[StringLength(400)]
	public string pcbcode { get; set; }

	[Key]
	[Column(Order = 4)]
	public string pcbdata { get; set; }
}
