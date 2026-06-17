using System;
using System.Text;
using Ersa.Mes.Common.Helper;

namespace Ersa.Mes.Common;

public static class ResponseHelper
{
	public static byte[] CreateResponse(string a_ResponseMessage)
	{
		byte[] a_ResponseBytes = Encoding.UTF8.GetBytes(a_ResponseMessage);
		ushort crc = CrcHelper.Fun_u32CalculateCRC16(a_ResponseBytes);
		byte[] bytes = BitConverter.GetBytes(crc);
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse(bytes);
		}
		byte[] bytes2 = BitConverter.GetBytes(a_ResponseBytes.Length + bytes.Length);
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse(bytes2);
		}
		byte[] array = new byte[1 + bytes2.Length + a_ResponseBytes.Length + bytes.Length];
		array[0] = 1;
		Array.Copy(bytes2, 0, array, 1, bytes2.Length);
		Array.Copy(a_ResponseBytes, 0, array, 1 + bytes2.Length, a_ResponseBytes.Length);
		Array.Copy(bytes, 0, array, 1 + bytes2.Length + a_ResponseBytes.Length, bytes.Length);
		return array;
	}
}
