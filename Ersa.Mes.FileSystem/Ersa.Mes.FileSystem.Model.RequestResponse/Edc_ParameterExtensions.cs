using System.Collections.Generic;
using System.Linq;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public static class Edc_ParameterExtensions
{
	public static Struct_ProcessParameter Fun_edcGetData(this List<Struct_ProcessParameter> list, string i_strPara, int i_i32TrackNumber = 1, string i_strDefault = "")
	{
		Struct_ProcessParameter item = list.Where((Struct_ProcessParameter s) => s.m_strName.Equals(i_strPara) && s.m_bytTrackNumber == i_i32TrackNumber).FirstOrDefault();
		if (item == null)
		{
			return null;
		}
		return item;
	}

	public static string Fun_strGetData(this List<Struct_ProcessParameter> list, string i_strPara, int i_i32TrackNumber = 1, string i_strDefault = "")
	{
		Struct_ProcessParameter item = list.Where((Struct_ProcessParameter s) => s.m_strName.Equals(i_strPara) && s.m_bytTrackNumber == i_i32TrackNumber).FirstOrDefault();
		if (item == null)
		{
			return i_strDefault;
		}
		return item.m_strValue.Replace(',', '.');
	}

	public static bool Fun_blnGetData(this List<Struct_ProcessParameter> list, string i_strPara, int i_i32TrackNumber = 1, string i_strDefault = "")
	{
		Struct_ProcessParameter item = list.Where((Struct_ProcessParameter s) => s.m_strName.Equals(i_strPara) && s.m_bytTrackNumber == i_i32TrackNumber).FirstOrDefault();
		if (item == null)
		{
			return false;
		}
		bool i_blnDefault = false;
		bool.TryParse(item.m_strValue, out i_blnDefault);
		return i_blnDefault;
	}

	public static int Fun_i32GetData(this List<Struct_ProcessParameter> list, string i_strPara, int i_i32TrackNumber = 1, int i_i32Default = -1)
	{
		Struct_ProcessParameter item = list.Where((Struct_ProcessParameter s) => s.m_strName.Equals(i_strPara) && s.m_bytTrackNumber == i_i32TrackNumber).FirstOrDefault();
		if (item == null)
		{
			return i_i32Default;
		}
		string a_strValue = item.m_strValue;
		int.TryParse(a_strValue, out i_i32Default);
		return i_i32Default;
	}
}
