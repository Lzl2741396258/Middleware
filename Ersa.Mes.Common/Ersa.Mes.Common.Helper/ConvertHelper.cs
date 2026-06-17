using System;
using System.Text;

namespace Ersa.Mes.Common.Helper;

public static class ConvertHelper
{
	public static string Fun_strConvert16(byte[] i_bytes, string a_strSeparate)
	{
		string a_strResult = string.Empty;
		foreach (byte item in i_bytes)
		{
			a_strResult = a_strResult + Convert.ToString(item, 16).PadLeft(2, '0') + a_strSeparate;
		}
		if (!string.IsNullOrEmpty(a_strResult))
		{
			a_strResult = a_strResult.Substring(0, a_strResult.Length - 1);
		}
		return a_strResult;
	}

	public static byte[] Fun_bytConvert16(byte[] i_bytes)
	{
		byte[] a_bytOut = new byte[i_bytes.Length];
		for (int i = 0; i < i_bytes.Length; i++)
		{
			a_bytOut[i] = byte.Parse(Convert.ToString(i_bytes[i], 16));
		}
		return a_bytOut;
	}

	public static double Fun_dblOneTenth(string i_strData)
	{
		if (double.TryParse(i_strData, out var value))
		{
			if (value > 0.0)
			{
				return value / 10.0;
			}
			return value;
		}
		return 0.0;
	}

	public static double Fun_dblOneTenth(int i_i32Data)
	{
		return Fun_dblOneTenth(i_i32Data.ToString());
	}

	public static double Fun_dblConvertData(string i_strData, string i_strUnit)
	{
		bool a_blnResult = i_strUnit.Contains("1/10 ");
		if (a_blnResult)
		{
			i_strUnit = i_strUnit.Replace("1/10 ", "");
		}
		if (double.TryParse(i_strData, out var result))
		{
			if (a_blnResult && result > 0.0)
			{
				return result / 10.0;
			}
			return result;
		}
		return 0.0;
	}

	public static int Fun_i32ConvertToInt32(string i_strData)
	{
		int result = 0;
		if (string.IsNullOrEmpty(i_strData))
		{
			return result;
		}
		int.TryParse(i_strData, out result);
		return result;
	}

	public static bool Fun_i32Pickup(string i_strData, char i_chrBegin, char i_chrEnd, out int result)
	{
		result = -1;
		if (i_strData.IndexOf(i_chrEnd) > i_strData.IndexOf(i_chrBegin) && i_strData.IndexOf(i_chrBegin) >= 0)
		{
			string str = i_strData.Substring(i_strData.IndexOf(i_chrBegin) + 1, i_strData.IndexOf(i_chrEnd) - i_strData.IndexOf(i_chrBegin) - 1);
			return int.TryParse(str, out result);
		}
		return false;
	}

	public static string Fun_strToBinary(string i_strData)
	{
		byte[] bytes = Encoding.Unicode.GetBytes(i_strData);
		StringBuilder sb = new StringBuilder(bytes.Length * 8);
		byte[] array = bytes;
		foreach (byte item in array)
		{
			sb.Append(Convert.ToString(item, 2).PadLeft(8, '0'));
		}
		return sb.ToString();
	}
}
