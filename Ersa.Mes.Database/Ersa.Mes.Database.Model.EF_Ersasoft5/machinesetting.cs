using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

[Table("public.machinesettings")]
public class machinesetting
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long machineid { get; set; }

	[Key]
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int settingtype { get; set; }

	[Key]
	[Column(Order = 2)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int settingindex { get; set; }

	public long? longvalue { get; set; }

	[StringLength(500)]
	public string textvalue { get; set; }

	public float? realvalue { get; set; }

	[MaxLength(int.MaxValue)]
	public byte[] arrayvalue { get; set; }

	public string memovalue { get; set; }
}
