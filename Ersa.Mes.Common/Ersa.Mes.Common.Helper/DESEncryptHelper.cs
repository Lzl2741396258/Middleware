using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ersa.Mes.Common.Helper;

public static class DESEncryptHelper
{
	private const string mC_strKey = "KURTZERSA";

	public static string Fun_strEncrypt(string i_strText)
	{
		return Fun_strEncrypt(i_strText, "KURTZERSA");
	}

	private static string Fun_strEncrypt(string a_strText, string i_strKey)
	{
		DESCryptoServiceProvider des = new DESCryptoServiceProvider();
		byte[] inputByteArray = Encoding.Default.GetBytes(a_strText);
		using (MD5 md5 = MD5.Create())
		{
			des.Key = Encoding.ASCII.GetBytes(BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(i_strKey + "md5"))).Replace("-", null).Substring(0, 8));
			des.IV = Encoding.ASCII.GetBytes(BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(i_strKey + "md5"))).Replace("-", null).Substring(0, 8));
		}
		using MemoryStream ms = new MemoryStream();
		CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
		cs.Write(inputByteArray, 0, inputByteArray.Length);
		cs.FlushFinalBlock();
		StringBuilder ret = new StringBuilder();
		byte[] array = ms.ToArray();
		foreach (byte b in array)
		{
			ret.AppendFormat("{0:X2}", b);
		}
		return ret.ToString();
	}

	public static string Fun_strDecrypt(string i_strText)
	{
		if (!string.IsNullOrEmpty(i_strText))
		{
			return Fun_strDecrypt(i_strText, "KURTZERSA");
		}
		return string.Empty;
	}

	private static string Fun_strDecrypt(string i_strText, string i_strKey)
	{
		DESCryptoServiceProvider des = new DESCryptoServiceProvider();
		int len = i_strText.Length / 2;
		byte[] inputByteArray = new byte[len];
		for (int x = 0; x < len; x++)
		{
			int i = Convert.ToInt32(i_strText.Substring(x * 2, 2), 16);
			inputByteArray[x] = (byte)i;
		}
		using (MD5 md5 = MD5.Create())
		{
			des.Key = Encoding.ASCII.GetBytes(BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(i_strKey + "md5"))).Replace("-", null).Substring(0, 8));
			des.IV = Encoding.ASCII.GetBytes(BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(i_strKey + "md5"))).Replace("-", null).Substring(0, 8));
		}
		using MemoryStream ms = new MemoryStream();
		CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
		cs.Write(inputByteArray, 0, inputByteArray.Length);
		cs.FlushFinalBlock();
		return Encoding.Default.GetString(ms.ToArray());
	}
}
