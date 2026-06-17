using System.Collections.Generic;
using System.Linq;
using Ersa.Mes.FileSystem.Model.RequestResponse;

namespace Ersa.Mes.FileSystem.Extensions;

public static class Edc_MesExtensions
{
	public static int Fun_i32GetActualValue(this List<Struct_ProtocolElement> i_lstElement, string i_strElementName, int i_i32Default = 0)
	{
		string result = i_lstElement.Where((Struct_ProtocolElement s) => s.m_strName.Trim().Equals(i_strElementName.Trim())).FirstOrDefault()?.m_strActualValue ?? string.Empty;
		int.TryParse(result, out i_i32Default);
		return i_i32Default;
	}

	public static int Fun_i32GetValuePlus(this List<Struct_ProtocolElement> i_lstElement, string i_strElementName, string i_strDefault = "")
	{
		string value = i_lstElement.Where((Struct_ProtocolElement s) => s.m_strName.Trim().Equals(i_strElementName.Trim())).FirstOrDefault()?.m_strOffsetPlus ?? i_strDefault;
		int.TryParse(value, out var result);
		return result;
	}

	public static int Fun_i32GetValueMinus(this List<Struct_ProtocolElement> i_lstElement, string i_strElementName, string i_strDefault = "")
	{
		string value = i_lstElement.Where((Struct_ProtocolElement s) => s.m_strName.Trim().Equals(i_strElementName.Trim())).FirstOrDefault()?.m_strOffsetMinus ?? i_strDefault;
		int.TryParse(value, out var result);
		return result;
	}

	public static int Fun_i32GetSetValue(this List<Struct_ProtocolElement> i_lstElement, string i_strElementName, int i_i32Default = 0)
	{
		string result = i_lstElement.Where((Struct_ProtocolElement s) => s.m_strName.Trim().Equals(i_strElementName.Trim())).FirstOrDefault()?.m_strSetValue ?? string.Empty;
		int.TryParse(result, out i_i32Default);
		return i_i32Default;
	}

	public static string Fun_strGetSetValue(this List<Struct_ProtocolElement> i_lstElement, string i_strElementName, string i_strDefault = "")
	{
		if (string.IsNullOrEmpty(i_lstElement.Where((Struct_ProtocolElement s) => s.m_strName.Trim().Equals(i_strElementName.Trim())).FirstOrDefault()?.m_strSetValue))
		{
			return i_strDefault;
		}
		return i_lstElement.Where((Struct_ProtocolElement s) => s.m_strName.Trim().Equals(i_strElementName.Trim())).FirstOrDefault()?.m_strSetValue;
	}

	public static string Fun_strGetActualValue(this List<Struct_ProtocolElement> i_lstElement, string i_strElementName, string i_strDefault = "")
	{
		return i_lstElement.Where((Struct_ProtocolElement s) => s.m_strName.Trim().Equals(i_strElementName.Trim())).FirstOrDefault()?.m_strActualValue ?? i_strDefault;
	}
}
