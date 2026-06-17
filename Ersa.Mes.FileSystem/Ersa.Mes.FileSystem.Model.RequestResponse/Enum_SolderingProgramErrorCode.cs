using System.ComponentModel;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public enum Enum_SolderingProgramErrorCode : byte
{
	[Description("Popup")]
	NichtDefiniert,
	[Description("OK")]
	Ok,
	[Description("Error")]
	Fehler,
	[Description("ProgramError")]
	ProgrammFehler,
	[Description("BibPathError")]
	BibliothekFehler,
	[Description("WrongVersion")]
	FalscheVersion
}
