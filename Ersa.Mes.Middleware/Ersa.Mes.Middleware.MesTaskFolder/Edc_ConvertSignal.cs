using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ersa.Mes.Common.Helper;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Device;
using Ersa.Mes.Middleware.Factory;

namespace Ersa.Mes.Middleware.MesTaskFolder;

public class Edc_ConvertSignal : Edc_MesTask
{
	private Edc_ConfigBase m_Config;

	protected List<Inf_Device> m_lstDevice = new List<Inf_Device>();

	public Dictionary<string, List<byte>> m_dicCache = new Dictionary<string, List<byte>>();

	private bool m_blnCheckRepeat = false;

	private byte[] m_bytCurrentData = null;

	public List<string> m_lstRepeat = new List<string>();

	protected const byte Pro_bytSTX = 2;

	protected const byte Pro_bytETX = 3;

	protected const byte Pro_bytEnter = 13;

	protected const byte Pro_bytWrap = 10;

	protected const byte Pro_bytActiveScan = 49;

	protected const byte Pro_bytActiveScanEnd = 50;

	private List<Edc_Process> m_lstProcess { get; set; }

	public Edc_ConvertSignal(Edc_ConfigBase i_Config, Inf_MesTaskAttributes i_edcMesTaskAttributes, Inf_Logger i_edcLogger)
		: base(i_edcMesTaskAttributes, i_edcLogger)
	{
		m_Config = i_Config;
		base.m_edcLogger = i_edcLogger;
		m_lstProcess = i_Config.m_clsDevice.m_lstProcess;
		base.m_edcLogger.Debug("Constructor  Edc_ConvertSignal successed...", null, ".ctor", 92);
	}

	public override Task Sub_Act()
	{
		try
		{
			Sub_InitailizeDevice();
			m_blnOnlyOnce = true;
			base.m_edcLogger.Debug("Initailize Device successed...", null, "Sub_Act", 106);
		}
		catch (Exception ex)
		{
			base.m_edcLogger.Error(ex.Message, null, "Sub_Act", 110);
			OnShowMessage(Enum_LogType.Error, "Initailize Device failed...Details:" + ex.Message);
		}
		return Task.CompletedTask;
	}

	private void Sub_InitailizeDevice()
	{
		m_lstDevice.Clear();
		Edc_Device a_edcDevice = m_Config.m_clsDevice;
		List<Edc_ConfigSerialPort> a_edcSerialPorts = a_edcDevice.m_lstSerialPorts;
		for (int j = 0; j < a_edcSerialPorts.Count; j++)
		{
			Edc_DeviceSerialPort a_edcDeviceSerialPort = new Edc_DeviceSerialPort
			{
				Pro_strCom = a_edcSerialPorts[j].m_strPortName,
				Pro_i32BaudRate = a_edcSerialPorts[j].m_i32Rate,
				Pro_i32DataBits = a_edcSerialPorts[j].m_i32DataBits,
				Pro_edcParity = a_edcSerialPorts[j].m_edcParity,
				Pro_edcStopBits = a_edcSerialPorts[j].m_strStopBits,
				Pro_blnDtrEnable = a_edcSerialPorts[j].m_blnDtrEnable,
				Pro_blnRtsEnable = a_edcSerialPorts[j].m_blnRtsEnable,
				m_strRemark = a_edcSerialPorts[j].m_strRemark,
				m_blnCacheActive = true
			};
			a_edcDeviceSerialPort.m_evtReceived += Sub_DeviceReceived;
			m_lstDevice.Add(a_edcDeviceSerialPort);
		}
		List<Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware.Edc_Socket> a_edcSockets = a_edcDevice.m_lstSockets;
		for (int i = 0; i < a_edcSockets.Count; i++)
		{
			Edc_DeviceNet a_edcDeviceNet = new Edc_DeviceNet(a_edcDevice.m_lstSockets[i].m_blnServer)
			{
				Pro_strIP = a_edcSockets[i].m_strIP,
				Pro_i32Port = a_edcSockets[i].m_i32Port,
				m_strRemark = a_edcSockets[i].m_strRemark
			};
			a_edcDeviceNet.Evt_Received += Sub_DeviceReceived;
			m_lstDevice.Add(a_edcDeviceNet);
		}
		if (m_lstDevice == null || m_lstDevice.Count == 0)
		{
			OnShowMessage(Enum_LogType.Error, "Device is empty, Please set it up first...");
		}
		foreach (Inf_Device item in m_lstDevice)
		{
			if (item.Fun_blnOpen())
			{
				OnShowMessage(Enum_LogType.Info, item.Fun_strGetName() + "-" + item.m_strRemark + " open successed...");
			}
			else
			{
				OnShowMessage(Enum_LogType.Error, item.Fun_strGetName() + "-" + item.m_strRemark + " open failed...");
			}
		}
	}

	protected virtual async void Sub_DeviceReceived(object sender, Evt_PortDataEventHandlerArgs e)
	{
		byte[] a_bytData = e.m_bytData;
		string a_strData = Encoding.Default.GetString(a_bytData);
		string a_strDeviceName = e.m_strDeviceName;
		string a_strMessage = a_strDeviceName + "  Received:" + a_strData.Replace('\r', ' ') + "  " + ConvertHelper.Fun_strConvert16(a_bytData, " ") + "'";
		base.m_edcLogger.Debug(a_strMessage, null, "Sub_DeviceReceived", 192);
		if (m_dicCache.TryGetValue(a_strDeviceName, out var value))
		{
			value.AddRange(a_bytData);
		}
		else
		{
			m_dicCache.Add(a_strDeviceName, a_bytData.ToList());
		}
		string a_strDeviceRemark = string.Empty;
		Edc_ConfigSerialPort item = m_Config.m_clsDevice.m_lstSerialPorts.Where((Edc_ConfigSerialPort s) => s.m_strPortName.Equals(a_strDeviceName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
		if (item != null)
		{
			_ = item.m_i32TrackId;
			a_strDeviceRemark = item.m_strRemark;
		}
		List<Edc_Process> a_lstProcess = m_lstProcess.Where((Edc_Process s) => s.m_strInput.Equals(a_strDeviceName)).ToList();
		foreach (Edc_Process a_edcProcess in a_lstProcess)
		{
			Enum.TryParse<Enum_DataFormat>(a_edcProcess.m_strInputDataFormat, out var o_enuIn);
			Enum.TryParse<Enum_DataFormat>(a_edcProcess.m_strOutputDataFormat, out var o_enuOut);
			if (m_bytCurrentData != null)
			{
				continue;
			}
			try
			{
				if (!Fun_blnCheckDataFormat(a_strDeviceName, o_enuIn, ref a_bytData))
				{
					continue;
				}
				m_bytCurrentData = a_bytData;
				if (!m_blnCheckRepeat && a_edcProcess.m_blnInputDataRepeat && !Fun_blnCheckDataNotRepeat(a_bytData))
				{
					continue;
				}
				m_blnCheckRepeat = true;
				if (await Fun_blnProcessData(a_strDeviceName, a_strDeviceRemark, ref a_bytData))
				{
					if (o_enuOut != 0)
					{
						Sub_AddDataFormat(o_enuOut, ref a_bytData);
					}
					if (!string.IsNullOrEmpty(a_edcProcess.m_strOutput))
					{
						Fun_blnSendData(a_edcProcess.m_strOutput, a_bytData, a_strDeviceRemark);
					}
				}
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				base.m_edcLogger.Error(MethodBase.GetCurrentMethod().DeclaringType.Name + "  " + MethodBase.GetCurrentMethod().Name + "  " + ex.Message, null, "Sub_DeviceReceived", 262);
			}
		}
		m_bytCurrentData = null;
		m_blnCheckRepeat = false;
	}

	protected virtual bool Fun_blnCheckDataFormat(string i_strCacheName, Enum_DataFormat i_enuDataCheck, ref byte[] r_bytData)
	{
		List<byte> i_lstCache = (m_dicCache.ContainsKey(i_strCacheName) ? m_dicCache[i_strCacheName] : new List<byte>());
		switch (i_enuDataCheck)
		{
		case Enum_DataFormat.Default:
		{
			string a_strData = Encoding.Default.GetString(i_lstCache.ToArray());
			string a_strReplace = Encoding.Default.GetString(r_bytData.ToArray());
			int a_i32Index = a_strData.LastIndexOf(a_strReplace);
			a_strData = a_strData.Remove(a_i32Index, r_bytData.Length);
			m_dicCache[i_strCacheName].RemoveRange(0, r_bytData.Length);
			return true;
		}
		case Enum_DataFormat.STXETX:
		{
			int i_i32BeginIndex = i_lstCache.IndexOf(2);
			if (i_i32BeginIndex < 0)
			{
				return false;
			}
			int i_i32EndIndex = i_lstCache.FindIndex(i_i32BeginIndex, (byte s) => s == 3);
			if (i_i32BeginIndex >= 0 && i_i32EndIndex >= 0)
			{
				r_bytData = new byte[i_i32EndIndex - i_i32BeginIndex - 1];
				i_lstCache.CopyTo(i_i32BeginIndex + 1, r_bytData, 0, r_bytData.Length);
				i_lstCache.RemoveRange(i_i32BeginIndex, i_i32EndIndex - i_i32BeginIndex + 1);
				return true;
			}
			break;
		}
		case Enum_DataFormat.OnlyEnd0D:
		{
			List<int> lstIndex = new List<int>();
			for (int i = 0; i < i_lstCache.Count; i++)
			{
				if (i_lstCache[i] == 13)
				{
					lstIndex.Add(i);
				}
			}
			if (lstIndex.Count == 0)
			{
				return false;
			}
			if (lstIndex.Count == 1)
			{
				int index = i_lstCache.IndexOf(13);
				int length = index;
				r_bytData = new byte[length];
				i_lstCache.CopyTo(0, r_bytData, 0, index);
				i_lstCache.Clear();
				return true;
			}
			if (lstIndex.Count >= 2)
			{
				int maxIndex = lstIndex.Count - 1;
				int dataLength = lstIndex[maxIndex] - lstIndex[maxIndex - 1] - 1;
				int beginIndex = lstIndex[maxIndex - 1];
				int endIndex = lstIndex[maxIndex];
				r_bytData = new byte[dataLength];
				Array.Copy(i_lstCache.ToArray(), lstIndex[maxIndex - 1] + 1, r_bytData, 0, dataLength);
				i_lstCache.RemoveRange(beginIndex, dataLength + 1);
				i_lstCache.Clear();
				return true;
			}
			break;
		}
		case Enum_DataFormat.STXEnterWrap:
		{
			int i_i32BeginIndex2 = i_lstCache.IndexOf(2);
			if (i_i32BeginIndex2 < 0)
			{
				return false;
			}
			int i_i32EndIndex2 = i_lstCache.FindIndex(i_i32BeginIndex2, (byte s) => s == 10);
			if (i_i32BeginIndex2 >= 0 && i_i32EndIndex2 >= 0)
			{
				r_bytData = new byte[i_i32EndIndex2 - i_i32BeginIndex2 - 2];
				i_lstCache.CopyTo(i_i32BeginIndex2 + 1, r_bytData, 0, r_bytData.Length);
				i_lstCache.RemoveRange(i_i32BeginIndex2, i_i32EndIndex2 - i_i32BeginIndex2 + 1);
				return true;
			}
			break;
		}
		case Enum_DataFormat.EnterWrap:
			if (i_lstCache.IndexOf(13) > 0 && i_lstCache.IndexOf(10) > 0)
			{
				r_bytData = new byte[i_lstCache.IndexOf(16) - 1];
				i_lstCache.CopyTo(0, r_bytData, 0, r_bytData.Length);
				i_lstCache.RemoveRange(0, i_lstCache.IndexOf(16) + 1);
				return true;
			}
			break;
		}
		return false;
	}

	protected virtual void Sub_AddDataFormat(Enum_DataFormat i_enuDataCheck, ref byte[] i_bytData)
	{
		switch (i_enuDataCheck)
		{
		case Enum_DataFormat.Default:
			break;
		case Enum_DataFormat.EnterWrap:
			i_bytData = i_bytData.Concat(new byte[1] { 13 }).Concat(new byte[1] { 10 }).ToArray();
			break;
		case Enum_DataFormat.STXETX:
			i_bytData = new byte[1] { 2 }.Concat(i_bytData).Concat(new byte[1] { 3 }).ToArray();
			break;
		case Enum_DataFormat.OnlyEnd0D:
			i_bytData = i_bytData.Concat(new byte[1] { 13 }).ToArray();
			break;
		case Enum_DataFormat.STXEnterWrap:
			i_bytData = new byte[1] { 2 }.Concat(i_bytData).Concat(new byte[1] { 13 }).Concat(new byte[1] { 10 })
				.ToArray();
			break;
		}
	}

	protected virtual bool Fun_blnCheckDataNotRepeat(byte[] i_bytOut)
	{
		string code = Encoding.Default.GetString(i_bytOut);
		if (m_lstRepeat.Contains(code))
		{
			OnShowMessage(Enum_LogType.Warn, "Repeat Code..." + code);
			return false;
		}
		m_lstRepeat.Add(code);
		return true;
	}

	protected virtual byte[] Fun_bytConvertData(byte[] i_bytData)
	{
		return i_bytData;
	}

	protected virtual Task<bool> Fun_blnProcessData(string i_strPortName, string i_strDeviceRemark, ref byte[] r_bytData)
	{
		return Task.FromResult(result: true);
	}

	protected virtual bool Fun_blnSendData(string i_strOutputDevice, byte[] i_bytData, string i_strRemark = "")
	{
		m_lstDevice.Where((Inf_Device s) => s.Fun_strGetName().Equals(i_strOutputDevice)).FirstOrDefault()?.Fun_blnSend(i_bytData);
		string a_strData = Encoding.Default.GetString(i_bytData);
		a_strData = a_strData.Replace("\u0002", "[STX]").Replace("\u0003", "[ETX]").Replace("\r", "[CR]");
		OnShowMessage(Enum_LogType.Info, i_strOutputDevice + "-" + i_strRemark + " Send:" + a_strData);
		base.m_edcLogger.Debug(i_strOutputDevice + "-" + i_strRemark + " send:" + a_strData + " - " + ConvertHelper.Fun_strConvert16(i_bytData, " "), null, "Fun_blnSendData", 491);
		return true;
	}

	protected virtual bool Fun_blnActiveScan(byte[] i_bytData)
	{
		if (i_bytData.Length == 1)
		{
			if (i_bytData[0] == 49)
			{
				return true;
			}
		}
		else if (i_bytData.Length == 2)
		{
			if (i_bytData[0] == 49 && i_bytData[1] == 13)
			{
				return true;
			}
			if (i_bytData[0] == 50 && i_bytData[1] == 49)
			{
				return true;
			}
		}
		else if (i_bytData.Length == 4 && i_bytData[0] == 50 && i_bytData[1] == 13 && i_bytData[2] == 49 && i_bytData[3] == 13)
		{
			return true;
		}
		return false;
	}

	protected virtual bool Fun_blnActiveScanEnd(byte[] i_bytData)
	{
		if (i_bytData.Length == 1 && i_bytData[0] == 50)
		{
			return true;
		}
		return false;
	}

	public override void Sub_End()
	{
		if (m_lstDevice != null)
		{
			foreach (Inf_Device item in m_lstDevice)
			{
				item.Fun_blnClose();
			}
		}
		m_dicCache.Clear();
		base.Sub_End();
	}
}
