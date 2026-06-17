using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Ersa.Mes.Common.Description;
using Ersa.Mes.Common.Extensions;

namespace Ersa.Mes.Middleware.Helper;

public static class EnumHelper
{
	public static string Fun_strGetEnumValueDescription<T>(T i_objEnumValue) where T : Enum, IConvertible
	{
		Type a_Type = typeof(T);
		return GetEnumDescription(a_Type, i_objEnumValue);
	}

	public static string Fun_strGetChineseDescription<T>(T i_objEnumValue)
	{
		Type type = typeof(T);
		FieldInfo a_FieldInfo = type.GetField(i_objEnumValue.ToString());
		return (a_FieldInfo.GetCustomAttributes(typeof(DescriptionChineseAttribute), inherit: false).FirstOrDefault() is DescriptionChineseAttribute ma) ? ma.m_strIdentifier : null;
	}

	public static string Fun_strGetEnglishDescription<T>(T i_objEnumValue)
	{
		Type type = typeof(T);
		string a_strParameterName = i_objEnumValue.ToString();
		FieldInfo a_FieldInfo = type.GetField(a_strParameterName);
		return (a_FieldInfo.GetCustomAttributes(typeof(DescriptionEnglishAttribute), inherit: false).FirstOrDefault() is DescriptionEnglishAttribute ma) ? ma.m_strIdentifier : null;
	}

	public static string Fun_strGetFunctionEnglishDescription(Type i_EnumType, object i_objEnumValue)
	{
		FieldInfo a_FieldInfo = i_EnumType.GetField(i_objEnumValue.ToString());
		return (a_FieldInfo.GetCustomAttributes(typeof(Edc_MesFunctionEnglishAttribute), inherit: false).FirstOrDefault() is Edc_MesFunctionEnglishAttribute ma) ? ma.m_strIdentifier : null;
	}

	public static string GetEnumDescription(Type i_EnumType, object i_objEnumValue)
	{
		FieldInfo a_FieldInfo = i_EnumType.GetField(i_objEnumValue.ToString());
		Attribute attribute = a_FieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false).FirstOrDefault() as DescriptionAttribute;
		return (attribute == null) ? string.Empty : ((DescriptionAttribute)attribute).Description;
	}
}
