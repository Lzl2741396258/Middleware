using System;
using System.IO.Ports;

namespace Ersa.Mes.Middleware.Helper;

public static class SerialportHelper
{
	public static void Sub_Open(SerialPort i_SerialPort, Action<object, SerialDataReceivedEventArgs> i_delSerialPortDAtaReceived)
	{
		if (i_SerialPort != null && i_SerialPort.IsOpen)
		{
			i_SerialPort.Close();
		}
		i_SerialPort.DataReceived += i_delSerialPortDAtaReceived.Invoke;
		i_SerialPort.ReceivedBytesThreshold = 1;
		if (!string.IsNullOrEmpty(i_SerialPort.PortName))
		{
			i_SerialPort.Open();
		}
	}

	public static string[] Fun_strGetBaudRate()
	{
		return new string[7] { "9600", "19200", "38400", "43000", "56000", "57600", "115200" };
	}

	public static byte[] Fun_bytGetByte16Hex(byte[] i_bytes)
	{
		byte[] a_bytNew = new byte[i_bytes.Length];
		for (int i = 0; i < i_bytes.Length; i++)
		{
			a_bytNew[i] = byte.Parse(Convert.ToString(i_bytes[i], 16));
		}
		return a_bytNew;
	}
}
