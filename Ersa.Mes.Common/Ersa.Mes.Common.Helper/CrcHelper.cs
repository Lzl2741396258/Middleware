namespace Ersa.Mes.Common.Helper;

public static class CrcHelper
{
	private static readonly ushort[] m_crcTable;

	static CrcHelper()
	{
		m_crcTable = new ushort[256];
		for (ushort num = 0; num < m_crcTable.Length; num = (ushort)(num + 1))
		{
			ushort num2 = 0;
			ushort num3 = num;
			for (byte b = 0; b < 8; b = (byte)(b + 1))
			{
				num2 = ((((num2 ^ num3) & 1) == 0) ? ((ushort)(num2 >> 1)) : ((ushort)((uint)(num2 >> 1) ^ 0xA001u)));
				num3 = (ushort)(num3 >> 1);
			}
			m_crcTable[num] = num2;
		}
	}

	public static ushort Fun_u32CalculateCRC16(byte[] buffer, int i_i32Offset = 0)
	{
		ushort num = 0;
		for (int i = 0; i < buffer.Length; i++)
		{
			byte b = (byte)(num ^ buffer[i_i32Offset + i]);
			num = (ushort)((num >> 8) ^ m_crcTable[b]);
		}
		return num;
	}
}
