using System;
using System.IO.Ports;

namespace Ersa.Mes.Common;

public static class SerialPortHelper_Vcam
{
	private static SerialPort m_SerialPort;

	public static bool Fun_btnOpenPort(string i_strPort, int i_i32Rate)
	{
		try
		{
			if (m_SerialPort != null && m_SerialPort.IsOpen)
			{
				m_SerialPort.Close();
			}
			m_SerialPort.PortName = i_strPort;
			m_SerialPort.BaudRate = i_i32Rate;
			m_SerialPort.Parity = Parity.None;
			m_SerialPort.DataBits = 8;
			m_SerialPort.StopBits = StopBits.One;
			if (string.IsNullOrEmpty(m_SerialPort.PortName))
			{
				return false;
			}
			m_SerialPort.Open();
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static void Sub_ClosePort()
	{
		if (m_SerialPort.IsOpen)
		{
			m_SerialPort.Close();
		}
	}

	public static SerialPort Fun_edcGetPort(string i_strPort, int i_i32Rate)
	{
		if (m_SerialPort == null)
		{
			m_SerialPort = new SerialPort();
			Fun_btnOpenPort(i_strPort, i_i32Rate);
		}
		return m_SerialPort;
	}

	public static string[] Fun_strGetBaudRate()
	{
		return new string[12]
		{
			"300", "600", "1200", "2400", "4800", "9600", "19200", "38400", "43000", "56000",
			"57600", "115200"
		};
	}
}
