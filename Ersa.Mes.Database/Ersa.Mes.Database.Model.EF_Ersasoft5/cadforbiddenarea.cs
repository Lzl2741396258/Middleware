using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.cadforbiddenareas")]
public class cadforbiddenarea
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long areaid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long historyid { get; set; }

	public float? startpointx { get; set; }

	public float? startpointy { get; set; }

	public float? endpointx { get; set; }

	public float? endpointy { get; set; }

	public float? height { get; set; }

	public bool? crossingallowed { get; set; }
}
