using System;

namespace Ersa.Mes.Common.Description;

public class DescriptionEnglishAttribute : Attribute
{
	public string m_strIdentifier { get; private set; }

	public DescriptionEnglishAttribute(string i_strIdentifier)
	{
		m_strIdentifier = i_strIdentifier;
	}
}
