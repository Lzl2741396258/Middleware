using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.transferparameterconvection")]
public class TransferParameterConvection
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	public string a_intBlower_0 { get; set; }

	public string a_intBlower_1 { get; set; }

	public string a_intBlower_2 { get; set; }

	public string a_intBlower_3 { get; set; }

	public string a_intBlower_4 { get; set; }

	public string a_intBlower_5 { get; set; }

	public string a_intBlower_6 { get; set; }
}
