using System;

namespace Ersa.Mes.Common.Description;

public class DescriptionChineseAttribute : Attribute
{
	public string m_strIdentifier { get; private set; }

	public DescriptionChineseAttribute(string i_strIdentifier)
	{
		m_strIdentifier = i_strIdentifier;
	}
}
