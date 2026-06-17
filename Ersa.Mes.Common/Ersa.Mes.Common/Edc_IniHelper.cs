using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Ersa.Mes.Common;

public static class Edc_IniHelper
{
	[DllImport("kernel32")]
	private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

	[DllImport("kernel32")]
	private static extern int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);

	public static string Fun_strRead(string i_strSection, string i_strKey, string i_strDefault, string i_strFilePath)
	{
		StringBuilder sb = new StringBuilder(1024);
		GetPrivateProfileString(i_strSection, i_strKey, i_strDefault, sb, 1024, i_strFilePath);
		return sb.ToString();
	}

	public static int Fun_i32Read(string i_strSection, string i_strKey, string i_strDefault, string i_strFilePath)
	{
		string value = Fun_strRead(i_strSection, i_strKey, i_strDefault, i_strFilePath);
		int i_i32Default = -1;
		int.TryParse(value, out i_i32Default);
		return i_i32Default;
	}

	public static int Fun_i32Write(string section, string key, string value, string filePath)
	{
		Sub_CheckPath(filePath);
		return WritePrivateProfileString(section, key, value, filePath);
	}

	public static int Fun_i32DeleteSection(string section, string filePath)
	{
		return Fun_i32Write(section, null, null, filePath);
	}

	public static int Fun_i32DeleteKey(string section, string key, string filePath)
	{
		return Fun_i32Write(section, key, null, filePath);
	}

	private static void Sub_CheckPath(string a_strFilePath)
	{
		if (string.IsNullOrWhiteSpace(a_strFilePath) || !File.Exists(a_strFilePath))
		{
			throw new ArgumentNullException("filePath");
		}
	}
}
