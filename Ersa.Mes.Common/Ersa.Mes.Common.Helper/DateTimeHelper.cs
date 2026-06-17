using System;
using System.Globalization;

namespace Ersa.Mes.Common.Helper;

public static class DateTimeHelper
{
	public static DateTime Sub_ConvertToDateTimeDe(string i_strDate)
	{
		CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("de-de");
		string format = "dddd, dd. MMM yyyy HH:mm:ss";
		DateTime a_dtmResult = DateTime.MinValue;
		bool result = DateTime.TryParseExact(i_strDate, format, cultureInfo, DateTimeStyles.None, out a_dtmResult);
		return a_dtmResult;
	}

	public static string Fun_strGetDatetimeStandard(string i_strTime, string i_strJoinDate = "-", string i_strJoinTime = ":")
	{
		return Fun_dtmGetDatetime(i_strTime).ToString("yyyy" + i_strJoinDate + "MM" + i_strJoinDate + "dd HH" + i_strJoinTime + "mm" + i_strJoinTime + "ss");
	}

	public static string Fun_strGetDatetimeStandard(DateTime i_dtmDate, string i_strJoinYear = "-", string i_strJoinHour = ":")
	{
		return i_dtmDate.ToString("yyyy" + i_strJoinYear + "MM" + i_strJoinYear + "dd HH" + i_strJoinHour + "mm" + i_strJoinHour + "ss");
	}

	public static DateTime Fun_dtmGetDatetime(string i_strTime)
	{
		if (DateTime.TryParse(i_strTime, out var result))
		{
			return result.ToLocalTime();
		}
		return result;
	}

	public static int Fun_i32GetSecends(string i_strTime)
	{
		return Fun_dtmGetDatetime(i_strTime).Second;
	}
}
