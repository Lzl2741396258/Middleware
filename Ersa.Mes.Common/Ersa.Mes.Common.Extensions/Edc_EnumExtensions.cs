using System;
using System.ComponentModel;
using System.Reflection;
using Ersa.Mes.Common.Description;

namespace Ersa.Mes.Common.Extensions;

public static class Edc_EnumExtensions
{
	// Compare String
	public static string Fun_strGetDescription(this Enum i_enuValue)
	{
		Type type = i_enuValue.GetType();
		FieldInfo a_FieldInfo = type.GetField(i_enuValue.ToString());
		if (a_FieldInfo.IsDefined(typeof(DescriptionAttribute), inherit: true))
		{
			DescriptionAttribute attribute = (DescriptionAttribute)a_FieldInfo.GetCustomAttribute(typeof(DescriptionAttribute), inherit: true);
			return attribute.Description;
		}
		return string.Empty;
	}

	private static object Fun_edcGetExtensionDescription<T>(this Enum i_enuValue)
	{
		Type type = i_enuValue.GetType();
		FieldInfo a_FieldInfo = type.GetField(i_enuValue.ToString());
		if (a_FieldInfo.IsDefined(typeof(T), inherit: true))
		{
			return a_FieldInfo.GetCustomAttribute(typeof(T), inherit: true);
		}
		return string.Empty;
	}

	public static string Fun_strGetDescriptionChinese(this Enum i_enuValue)
	{
		object obj = i_enuValue.Fun_edcGetExtensionDescription<DescriptionChineseAttribute>();
		if (obj is DescriptionChineseAttribute attribute)
		{
			return attribute.m_strIdentifier;
		}
		return string.Empty;
	}

	public static string Fun_strGetDescriptionEnglish(this Enum i_enuValue)
	{
		object obj = i_enuValue.Fun_edcGetExtensionDescription<DescriptionEnglishAttribute>();
		if (obj is DescriptionEnglishAttribute attribute)
		{
			return attribute.m_strIdentifier;
		}
		return string.Empty;
	}

	public static string Fun_strGetMesFunction(this Enum i_enuValue)
	{
		object obj = i_enuValue.Fun_edcGetExtensionDescription<Edc_MesFunctionEnglishAttribute>();
		if (obj is Edc_MesFunctionEnglishAttribute attribute)
		{
			return attribute.m_strIdentifier;
		}
		return string.Empty;
	}
}
