using System;
using System.Security.Cryptography;
using System.Text;

namespace Ersa.Mes.Common.Helper;

public static class MD5Helper
{
	public static string Fun_strGetMd5(string i_strConvertString, bool i_blnToLower)
	{
		MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
		string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(i_strConvertString)), 4, 8);
		t2 = t2.Replace("-", "");
		if (i_blnToLower)
		{
			t2 = t2.ToLower();
		}
		return t2;
	}

	private static string Fun_strUserMd5(string i_strInput)
	{
		string pwd = "";
		MD5 md5 = MD5.Create();
		byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(i_strInput));
		for (int i = 0; i < s.Length; i++)
		{
			pwd += s[i].ToString("X");
		}
		return pwd;
	}
}
