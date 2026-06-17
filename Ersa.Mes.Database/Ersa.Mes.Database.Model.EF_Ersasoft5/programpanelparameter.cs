using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.programpanelparameter")]
public class programpanelparameter
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long historyid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long panelid { get; set; }

	[StringLength(512)]
	public string panelcode { get; set; }

	public bool? sm1active { get; set; }

	public bool? fm1active { get; set; }

	public bool? fm2active { get; set; }

	public bool? lm1active { get; set; }

	public bool? lm2active { get; set; }

	public bool? lm3active { get; set; }

	public float? offsetx { get; set; }

	public float? offsety { get; set; }

	public float? offsetz { get; set; }

	public float? rotationdeg { get; set; }
}
