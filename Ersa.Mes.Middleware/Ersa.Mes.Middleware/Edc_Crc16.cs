using System;
using System.Windows.Forms;

namespace Ersa.Mes.Middleware;

public class Edc_Crc16
{
	private const ushort C_int16Polynomial = 40961;

	private ushort[] ma_uint16Table;

	public Edc_Crc16()
	{
		ma_uint16Table = new ushort[257];
		Sub_Init();
	}

	public ushort Fun_uint16ComputeChecksum(byte[] ia_bytDaten)
	{
		return Fun_uint16ComputeChecksum(ia_bytDaten, 0, ia_bytDaten.Length);
	}

	public ushort Fun_uint16ComputeChecksum(byte[] ia_bytDaten, int i_int32Offset, int i_int32Length)
	{
		ushort num = 0;
		checked
		{
			try
			{
				int num2 = i_int32Length - 1;
				int num3 = 0;
				while (true)
				{
					int num4 = num3;
					int num5 = num2;
					if (num4 > num5)
					{
						break;
					}
					byte b = (byte)((num ^ ia_bytDaten[i_int32Offset + num3]) & 0x1000FF);
					num = unchecked((ushort)((ushort)((uint)num >> 8) ^ ma_uint16Table[b]));
					num3++;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			return num;
		}
	}

	private void Sub_Init()
	{
		try
		{
			ushort num = checked((ushort)(ma_uint16Table.Length - 1));
			ushort num2 = 0;
			while (true)
			{
				ushort num3 = num2;
				ushort num4 = num;
				if ((uint)num3 > (uint)num4)
				{
					break;
				}
				ushort num5 = 0;
				ushort num6 = num2;
				byte b = 0;
				byte num7;
				byte b2;
				do
				{
					num5 = (ushort)((((num5 ^ num6) & 1) == 0) ? ((ushort)((uint)num5 >> 1)) : ((ushort)((uint)num5 >> 1) ^ 0xA001u));
					num6 = (ushort)((uint)num6 >> 1);
					checked
					{
						b = (byte)unchecked((uint)(b + 1));
						num7 = b;
						b2 = 7;
					}
				}
				while ((uint)num7 <= (uint)b2);
				ma_uint16Table[num2] = num5;
				checked
				{
					num2 = (ushort)unchecked((uint)(num2 + 1));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}
}
