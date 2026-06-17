using System;

namespace Ersa.Mes.Common.Extensions;

public static class TypeExtend
{
	public static int ToInt(this int? i_i32Value)
	{
		return i_i32Value.GetValueOrDefault();
	}

	public static int? Fun_fdcToIntOrNull(this string i_strValue)
	{
		int i = -1;
		if (int.TryParse(i_strValue, out i))
		{
			return i;
		}
		return null;
	}

	public static bool? Fun_fdcToBoolOrNull(this string i_strValue)
	{
		bool i = false;
		if (bool.TryParse(i_strValue, out i))
		{
			return i;
		}
		return null;
	}

	public static DateTime? Fun_fdcToDateTimeOrNull(this string i_strValue)
	{
		DateTime i = DateTime.Parse("1900-1-1");
		if (DateTime.TryParse(i_strValue, out i))
		{
			return i;
		}
		return null;
	}

	public static int Fun_fdcToInt(this string i_strValue)
	{
		int i = -1;
		if (int.TryParse(i_strValue, out i))
		{
			return i;
		}
		return -1;
	}
}
