using System;
using System.Collections.Generic;
using System.Threading;
using Ersa.Mes.FileSystem.Model.Config;
using Ersa.Mes.FileSystem.Model.Config.Trace05;
using Ersa.Mes.FileSystem.Model.RequestResponse;

namespace Ersa.Mes.Middleware.Global;

public static class Edc_Global
{
	public static readonly int Pro_i32MaxRepetitions;

	public static List<string[]> m_lstSpray;

	public static DateTime Pro_dtmSoftBegin;

	public static string WebUrl;

	public static Mutex Pro_Mutex;

	public static Struct_ConfigJson m_sttConfigJson { get; set; }

	public static Struct_ConfigTrace05 m_sttConfigTrace05 { get; set; }

	public static Enum_OEECode m_enuOeeCode { get; set; }

	public static string Pro_strCompanyName { get; set; }

	public static string Pro_strTypeName { get; set; }

	public static string Pro_strVersion { get; set; }

	static Edc_Global()
	{
		Pro_i32MaxRepetitions = 999999999;
		m_lstSpray = new List<string[]>();
		Pro_dtmSoftBegin = DateTime.Now;
		WebUrl = string.Empty;
	}

	public static bool Fun_blnAppRunning(string ProductName)
	{
		Pro_Mutex = new Mutex(initiallyOwned: true, ProductName, out var i_blnNotRunning);
		if (!i_blnNotRunning)
		{
			return true;
		}
		return false;
	}
}
