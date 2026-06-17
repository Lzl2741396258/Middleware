using Ersa.Mes.Common.Description;

namespace Ersa.Mes.Middleware.Definition;

public enum Enum_ProtocolType : short
{
	[DescriptionGerman("Keines")]
	None = 0,
	[DescriptionGerman("ZVEI")]
	ZVEI = 1,
	[DescriptionGerman("NichtDefiniert")]
	NotDefined = 255
}
