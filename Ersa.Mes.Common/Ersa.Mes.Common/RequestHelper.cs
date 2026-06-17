using System;
using System.Text;

namespace Ersa.Mes.Common;

public static class RequestHelper
{
	public static int Fun_i32GetSize(byte[] a_TcpIpMessage, bool i_blnCRC)
	{
		if (!i_blnCRC)
		{
			return 0;
		}
		byte[] a_bytMessageSize = new byte[4];
		Array.Copy(a_TcpIpMessage, 1, a_bytMessageSize, 0, 4);
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse(a_bytMessageSize);
		}
		return BitConverter.ToInt32(a_bytMessageSize, 0);
	}

	public static string Fun_strExtractRequestMessage(byte[] a_TcpIpMessage)
	{
		int a_i32Length = Fun_i32GetSize(a_TcpIpMessage, i_blnCRC: true);
		byte[] a_RequestMessage = new byte[a_i32Length];
		Array.Copy(a_TcpIpMessage, 5, a_RequestMessage, 0, a_i32Length);
		return Encoding.UTF8.GetString(a_RequestMessage);
	}

	public static string Fun_strExtractRequestMessage(byte[] a_TcpIpMessage, int i_i32MessageSize)
	{
		byte[] a_RequestMessage = new byte[i_i32MessageSize];
		Array.Copy(a_TcpIpMessage, 5, a_RequestMessage, 0, i_i32MessageSize);
		return Encoding.UTF8.GetString(a_RequestMessage);
	}
}
