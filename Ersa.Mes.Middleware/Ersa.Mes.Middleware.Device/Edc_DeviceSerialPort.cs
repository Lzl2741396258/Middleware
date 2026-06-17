using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using Ersa.Mes.Middleware.MesTaskFolder;

namespace Ersa.Mes.Middleware.Device;

public class Edc_DeviceSerialPort : Inf_DeviceSerialPort, Inf_Device
{
	public List<byte> m_lstCache = new List<byte>();

	public byte[] Pro_bytBegin = new byte[1] { 2 };

	public byte[] Pro_bytEnd = new byte[1] { 3 };

	public readonly byte[] Pro_bytEnter = new byte[1] { 13 };

	public readonly byte[] Pro_bytWrap = new byte[1] { 10 };

	public int m_i32IntervalTime { get; set; }

	public bool m_blnReceiveEventFlag { get; set; } = true;


	public SerialPort Pro_SystemSerialPort { get; set; } = new SerialPort();


	public bool m_blnCacheActive { get; set; } = false;


	public string m_strRemark { get; set; } = "";


	public string Pro_strCom
	{
		get
		{
			return Pro_SystemSerialPort.PortName;
		}
		set
		{
			Pro_SystemSerialPort.PortName = value;
		}
	}

	public int Pro_i32BaudRate
	{
		get
		{
			return Pro_SystemSerialPort.BaudRate;
		}
		set
		{
			Pro_SystemSerialPort.BaudRate = value;
		}
	}

	public Parity Pro_edcParity
	{
		get
		{
			return Pro_SystemSerialPort.Parity;
		}
		set
		{
			Pro_SystemSerialPort.Parity = value;
		}
	}

	public int Pro_i32DataBits
	{
		get
		{
			return Pro_SystemSerialPort.DataBits;
		}
		set
		{
			Pro_SystemSerialPort.DataBits = value;
		}
	}

	public StopBits Pro_edcStopBits
	{
		get
		{
			return Pro_SystemSerialPort.StopBits;
		}
		set
		{
			Pro_SystemSerialPort.StopBits = value;
		}
	}

	public Encoding Pro_edcEncoding
	{
		get
		{
			return Pro_SystemSerialPort.Encoding;
		}
		set
		{
			Pro_SystemSerialPort.Encoding = value;
		}
	}

	public bool Pro_blnDtrEnable
	{
		get
		{
			return Pro_SystemSerialPort.DtrEnable;
		}
		set
		{
			Pro_SystemSerialPort.DtrEnable = value;
		}
	}

	public bool Pro_blnRtsEnable
	{
		get
		{
			return Pro_SystemSerialPort.RtsEnable;
		}
		set
		{
			Pro_SystemSerialPort.RtsEnable = value;
		}
	}

	public int Pro_i32ReadTimeout
	{
		get
		{
			return Pro_SystemSerialPort.ReadTimeout;
		}
		set
		{
			Pro_SystemSerialPort.ReadTimeout = value;
		}
	}

	public int Pro_i32WriteTimeout
	{
		get
		{
			return Pro_SystemSerialPort.WriteTimeout;
		}
		set
		{
			Pro_SystemSerialPort.WriteTimeout = value;
		}
	}

	private event SerialDataReceivedEventHandle _evtReceived;

	public event SerialDataReceivedEventHandle m_evtReceived
	{
		add
		{
			_evtReceived += value;
		}
		remove
		{
			_evtReceived -= value;
		}
	}

	private event SerialErrorReceivedEventHandler _evtError;

	public event SerialErrorReceivedEventHandler m_evtError
	{
		add
		{
			_evtError += value;
		}
		remove
		{
			_evtError -= value;
		}
	}

	private event SerialPinChangedEventHandler _evtPinChanged;

	public event SerialPinChangedEventHandler m_evtPinChanged
	{
		add
		{
			_evtPinChanged += value;
		}
		remove
		{
			_evtPinChanged -= value;
		}
	}

	public Edc_DeviceSerialPort()
	{
		Pro_SystemSerialPort.ReceivedBytesThreshold = 1;
		Pro_SystemSerialPort.PinChanged += Sub_PinChanged;
		Pro_SystemSerialPort.DataReceived += Sub_DataReceived;
		Pro_SystemSerialPort.ErrorReceived += Sub_ErrorEvent;
	}

	private void Sub_PinChanged(object sender, SerialPinChangedEventArgs e)
	{
		this._evtPinChanged?.Invoke(sender, e);
	}

	private void Sub_DataReceived(object a_Sender, SerialDataReceivedEventArgs e)
	{
		try
		{
			if (m_blnReceiveEventFlag)
			{
				Thread.Sleep(100);
				byte[] a_bytDatas = new byte[Pro_SystemSerialPort.BytesToRead];
				Pro_SystemSerialPort.Read(a_bytDatas, 0, a_bytDatas.Length);
				if (m_blnCacheActive)
				{
					m_lstCache.AddRange(a_bytDatas);
				}
				this._evtReceived?.Invoke(a_Sender, new Evt_PortDataEventHandlerArgs(Fun_strGetName(), a_bytDatas));
			}
		}
		catch (Exception)
		{
		}
	}

	private void Sub_ErrorEvent(object a_Sender, SerialErrorReceivedEventArgs e)
	{
		this._evtError?.Invoke(a_Sender, e);
	}

	public bool Fun_blnCheckDataFormat(Enum_DataFormat i_enuDataCheck, ref byte[] r_bytOut)
	{
		switch (i_enuDataCheck)
		{
		case Enum_DataFormat.Default:
			return true;
		case Enum_DataFormat.STXETX:
		{
			Pro_bytBegin = new byte[1] { 2 };
			Pro_bytEnd = new byte[1] { 3 };
			int i_i32BeginIndex = m_lstCache.IndexOf(Pro_bytBegin[0]);
			if (i_i32BeginIndex < 0)
			{
				return false;
			}
			int i_i32EndIndex = m_lstCache.FindIndex(i_i32BeginIndex, (byte s) => s == Pro_bytEnd[0]);
			if (i_i32BeginIndex >= 0 && i_i32EndIndex >= 0)
			{
				r_bytOut = new byte[i_i32EndIndex - i_i32BeginIndex - 1];
				m_lstCache.CopyTo(i_i32BeginIndex + 1, r_bytOut, 0, r_bytOut.Length);
				m_lstCache.RemoveRange(i_i32BeginIndex, i_i32EndIndex - i_i32BeginIndex + 1);
				return true;
			}
			break;
		}
		case Enum_DataFormat.OnlyEnd0D:
		{
			Pro_bytEnd = new byte[1] { 13 };
			int i_i32EndIndex2 = m_lstCache.FindIndex(0, (byte s) => s == Pro_bytEnd[0]);
			if (i_i32EndIndex2 >= 0)
			{
				r_bytOut = new byte[i_i32EndIndex2 + 1];
				m_lstCache.CopyTo(0, r_bytOut, 0, r_bytOut.Length);
				m_lstCache.RemoveRange(0, i_i32EndIndex2 + 1);
				return true;
			}
			break;
		}
		case Enum_DataFormat.STXEnterWrap:
		{
			int i_i32BeginIndex2 = m_lstCache.IndexOf(Pro_bytBegin[0]);
			if (i_i32BeginIndex2 < 0)
			{
				return false;
			}
			int i_i32EndIndex3 = m_lstCache.FindIndex(i_i32BeginIndex2, (byte s) => s == Pro_bytWrap[0]);
			if (i_i32BeginIndex2 >= 0 && i_i32EndIndex3 >= 0)
			{
				r_bytOut = new byte[i_i32EndIndex3 - i_i32BeginIndex2 - 2];
				m_lstCache.CopyTo(i_i32BeginIndex2 + 1, r_bytOut, 0, r_bytOut.Length);
				m_lstCache.RemoveRange(i_i32BeginIndex2, i_i32EndIndex3 - i_i32BeginIndex2 + 1);
				return true;
			}
			break;
		}
		case Enum_DataFormat.EnterWrap:
			if (m_lstCache.IndexOf(Pro_bytEnter[0]) > 0 && m_lstCache.IndexOf(Pro_bytWrap[0]) > 0)
			{
				r_bytOut = new byte[m_lstCache.IndexOf(16) - 1];
				m_lstCache.CopyTo(0, r_bytOut, 0, r_bytOut.Length);
				m_lstCache.RemoveRange(0, m_lstCache.IndexOf(16) + 1);
				return true;
			}
			break;
		}
		return false;
	}

	public bool Fun_blnSendData(byte[] a_bytData)
	{
		try
		{
			lock (this)
			{
				if (Pro_SystemSerialPort.IsOpen)
				{
					Thread.Sleep(m_i32IntervalTime);
					Pro_SystemSerialPort.Write(a_bytData, 0, a_bytData.Length);
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

	public bool Fun_blnSendData(string a_strData)
	{
		return Fun_blnSendData(Encoding.Default.GetBytes(a_strData));
	}

	public bool Fun_blnSendData(byte[] a_bytData, int a_i32Offset, int a_i32Count)
	{
		try
		{
			lock (this)
			{
				if (Pro_SystemSerialPort.IsOpen)
				{
					Thread.Sleep(m_i32IntervalTime);
					Pro_SystemSerialPort.Write(a_bytData, a_i32Offset, a_i32Count);
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

	public bool Fun_blnSend(byte[] a_bytData)
	{
		return Fun_blnSendData(a_bytData);
	}

	public bool Fun_blnSend(string i_strData)
	{
		return Fun_blnSend(Encoding.Default.GetBytes(i_strData));
	}

	public bool Fun_blnOpen()
	{
		try
		{
			lock (this)
			{
				if (!Pro_SystemSerialPort.IsOpen)
				{
					Pro_SystemSerialPort.Open();
					return true;
				}
				return false;
			}
		}
		catch (Exception)
		{
			return false;
		}
	}

	public virtual bool Fun_blnClose()
	{
		try
		{
			lock (this)
			{
				if (Pro_SystemSerialPort.IsOpen)
				{
					Pro_SystemSerialPort.Close();
					return true;
				}
				Pro_SystemSerialPort.Dispose();
				return false;
			}
		}
		catch (Exception)
		{
			return false;
		}
	}

	public virtual bool Fun_IsOpen()
	{
		try
		{
			lock (this)
			{
				return Pro_SystemSerialPort.IsOpen;
			}
		}
		catch (Exception)
		{
			return false;
		}
	}

	public virtual string[] Fun_GetComNames()
	{
		try
		{
			lock (this)
			{
				return SerialPort.GetPortNames();
			}
		}
		catch
		{
			throw new Exception("Get Com Names error");
		}
	}

	public string Fun_strGetName()
	{
		return Pro_strCom;
	}
}
