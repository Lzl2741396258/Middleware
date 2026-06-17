using System;

namespace Ersa.Mes.Common.Extensions;

[AttributeUsage(AttributeTargets.All)]
public class Edc_MesFunctionEnglishAttribute : Attribute
{
	public string m_strIdentifier { get; private set; }

	public Edc_MesFunctionEnglishAttribute(string i_strIdentifiers)
	{
		m_strIdentifier = i_strIdentifiers;
	}
}
