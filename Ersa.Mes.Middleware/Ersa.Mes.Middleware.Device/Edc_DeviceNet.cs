using System;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace Ersa.Mes.Middleware.Device;

public class Edc_DeviceNet : Inf_DeviceNet, Inf_Device
{
	public int Pro_i32Interval { get; set; } = 10;


	public bool Pro_blnFlagRecieveEvent { get; set; }

	public string m_strRemark { get; set; } = "";


	public Edc_Socket Pro_edcSocketSystem { get; set; }

	public string Pro_strIP
	{
		get
		{
			return Pro_edcSocketSystem.Pro_strIP;
		}
		set
		{
			Pro_edcSocketSystem.Pro_strIP = value;
		}
	}

	public int Pro_i32Port
	{
		get
		{
			return Pro_edcSocketSystem.Pro_i32Port;
		}
		set
		{
			Pro_edcSocketSystem.Pro_i32Port = value;
		}
	}

	public Encoding Pro_edcEncoding
	{
		get
		{
			return Pro_edcSocketSystem.Pro_edcEncoding;
		}
		set
		{
			Pro_edcSocketSystem.Pro_edcEncoding = value;
		}
	}

	[Browsable(false)]
	public int Pro_i32ReadTimeout
	{
		get
		{
			return Pro_edcSocketSystem.Pro_i32ReadTimeout;
		}
		set
		{
			Pro_edcSocketSystem.Pro_i32ReadTimeout = value;
		}
	}

	[Browsable(false)]
	public int Pro_i32WriteTimeout
	{
		get
		{
			return Pro_edcSocketSystem.Pro_i32WriteTimeout;
		}
		set
		{
			Pro_edcSocketSystem.Pro_i32WriteTimeout = value;
		}
	}

	private event Evt_SocketDataReceivedEventHandle m_evtReceived;

	public event Evt_SocketDataReceivedEventHandle Evt_Received
	{
		add
		{
			m_evtReceived += value;
		}
		remove
		{
			m_evtReceived -= value;
		}
	}

	private event Evt_SocketErrorDataReceivedEventHandle m_evtError;

	public event Evt_SocketErrorDataReceivedEventHandle Error
	{
		add
		{
			m_evtError += value;
		}
		remove
		{
			m_evtError -= value;
		}
	}

	public Edc_DeviceNet(bool i_blnServer)
	{
		Pro_edcSocketSystem = new Edc_Socket(i_blnServer);
		Pro_edcSocketSystem.m_evtDataReceived += SystemSocket_DataReceived;
		Pro_edcSocketSystem.m_evtErrorReceived += SystemSocket_ErrorReceived;
	}

	private void SystemSocket_DataReceived(string i_strData)
	{
		try
		{
			if (!Pro_blnFlagRecieveEvent)
			{
				byte[] data = Encoding.ASCII.GetBytes(i_strData);
				this.m_evtReceived?.Invoke(Pro_edcSocketSystem, new Evt_PortDataEventHandlerArgs(Fun_strGetName(), data));
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void SystemSocket_ErrorReceived(string i_strData)
	{
		if (i_strData != null)
		{
			byte[] data = Encoding.ASCII.GetBytes(i_strData);
			this.m_evtError?.Invoke(Pro_edcSocketSystem, new Evt_PortDataEventHandlerArgs(Fun_strGetName(), data));
		}
	}

	public bool Fun_blnOpen()
	{
		try
		{
			lock (this)
			{
				if (!Pro_edcSocketSystem.Pro_blnConnect)
				{
					return Pro_edcSocketSystem.Fun_blnOpen();
				}
				return false;
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public virtual bool Fun_blnClose()
	{
		try
		{
			lock (this)
			{
				if (Pro_edcSocketSystem.Pro_blnConnect)
				{
					Pro_edcSocketSystem.Sub_Close();
					return true;
				}
				return false;
			}
		}
		catch
		{
			return false;
		}
	}

	public bool Fun_IsOpen()
	{
		return Pro_edcSocketSystem.Pro_blnConnect;
	}

	public virtual bool Fun_blnSendData(byte[] a_bytes)
	{
		try
		{
			lock (this)
			{
				if (Fun_IsOpen())
				{
					Thread.Sleep(Pro_i32Interval);
					int result = Pro_edcSocketSystem.Fun_i32Send(Encoding.ASCII.GetString(a_bytes));
					return true;
				}
			}
		}
		catch (Exception)
		{
			return false;
		}
		return false;
	}

	public virtual bool Fun_blnSendData(byte[] i_bytes, int i_i32Offset, int i_i32Count)
	{
		try
		{
			lock (this)
			{
				if (Pro_edcSocketSystem.Pro_blnConnect)
				{
					byte[] a_bytData = new byte[i_i32Count];
					i_bytes.CopyTo(a_bytData, i_i32Offset);
					Thread.Sleep(Pro_i32Interval);
					Pro_edcSocketSystem.Fun_i32Send(Encoding.ASCII.GetString(a_bytData));
					return true;
				}
			}
		}
		catch (Exception)
		{
			return false;
		}
		return false;
	}

	public bool Fun_blnSend(byte[] a_bytes)
	{
		return Fun_blnSendData(a_bytes);
	}

	public bool Fun_blnSend(string i_strData)
	{
		return Fun_blnSend(Encoding.Default.GetBytes(i_strData));
	}

	public string Fun_strGetName()
	{
		return $"{Pro_edcSocketSystem.Pro_strIP} {Pro_edcSocketSystem.Pro_i32Port}";
	}
}
