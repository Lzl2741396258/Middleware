using System;

namespace Ersa.Mes.Middleware.Device;

public class Evt_PortDataEventHandlerArgs : EventArgs
{
	public string m_strDeviceName { get; set; }

	public byte[] m_bytData { get; set; }

	public Evt_PortDataEventHandlerArgs()
	{
		m_strDeviceName = string.Empty;
		m_bytData = null;
	}

	public Evt_PortDataEventHandlerArgs(string i_strDeviceName, byte[] a_Data)
	{
		m_strDeviceName = i_strDeviceName;
		m_bytData = a_Data;
	}
}
