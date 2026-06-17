using System;

namespace Ersa.Mes.Common.Description;

public class DescriptionGermanAttribute : Attribute
{
	public string m_strIdentifier { get; private set; }

	public DescriptionGermanAttribute(string i_strIdentifiers)
	{
		m_strIdentifier = i_strIdentifiers;
	}
}
