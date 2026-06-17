using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Ersa.Mes.Middleware.Device;

public class Edc_Socket
{
	private Dictionary<string, Socket> m_dicSocketClients = new Dictionary<string, Socket>();

	private bool _blnConnect = false;

	private Thread m_edcAcceptSocketThread;

	public bool m_blnServer { get; set; }

	public Socket m_edcSocketWatch { get; set; }

	public Socket m_edcSocketSend { get; set; }

	public string Pro_strIP { get; set; }

	public int Pro_i32Port { get; set; }

	public bool Pro_blnConnect
	{
		get
		{
			if (m_dicSocketClients.Count <= 1)
			{
				return _blnConnect;
			}
			foreach (Socket item in m_dicSocketClients.Values)
			{
				if (item.Connected)
				{
					_blnConnect = true;
					return _blnConnect;
				}
			}
			return _blnConnect;
		}
		set
		{
			_blnConnect = value;
		}
	}

	public Encoding Pro_edcEncoding { get; set; } = Encoding.ASCII;


	public int Pro_i32ReadTimeout { get; set; }

	public int Pro_i32WriteTimeout { get; set; }

	public event Evt_DataReceivedEventHandler m_evtDataReceived;

	public event Evt_ErrorReceivedEventHandler m_evtErrorReceived;

	public Edc_Socket(bool i_blnServer)
	{
		m_blnServer = i_blnServer;
	}

	public bool Fun_blnOpen()
	{
		try
		{
			if (m_edcSocketWatch != null && m_edcSocketWatch.Connected)
			{
				return true;
			}
			IPAddress a_edcIPAddress = (m_blnServer ? IPAddress.Any : IPAddress.Parse(Pro_strIP));
			IPEndPoint a_edcIPEndPoint = new IPEndPoint(a_edcIPAddress, Pro_i32Port);
			m_edcSocketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			if (m_blnServer)
			{
				m_edcSocketWatch.Bind(a_edcIPEndPoint);
				m_edcSocketWatch.Listen(10);
				Pro_blnConnect = true;
				m_edcAcceptSocketThread = new Thread(Sub_StartListen);
				m_edcAcceptSocketThread.IsBackground = true;
				m_edcAcceptSocketThread.Start(m_edcSocketWatch);
			}
			else
			{
				m_edcSocketWatch.Connect(a_edcIPEndPoint);
				Pro_blnConnect = true;
				m_edcAcceptSocketThread = new Thread(Sub_ListenClient);
				m_edcAcceptSocketThread.IsBackground = true;
				m_edcAcceptSocketThread.Start();
			}
		}
		catch (ArgumentNullException ex)
		{
			this.m_evtErrorReceived?.Invoke(ex.Message);
			return false;
		}
		catch (SocketException ex2)
		{
			this.m_evtErrorReceived?.Invoke(ex2.Message);
			return false;
		}
		return true;
	}

	public void Sub_Close()
	{
		Pro_blnConnect = false;
		Thread.Sleep(10);
		if (m_edcAcceptSocketThread != null)
		{
			m_edcAcceptSocketThread.Abort();
		}
		if (m_edcSocketWatch != null)
		{
			try
			{
				m_edcSocketWatch.Shutdown(SocketShutdown.Both);
			}
			finally
			{
				m_edcSocketWatch.Close();
			}
		}
	}

	private void Sub_ListenClient()
	{
		string a_strRecieve = string.Empty;
		byte[] a_bytes = new byte[512];
		do
		{
			Application.DoEvents();
			Thread.Sleep(1);
			try
			{
				int bytes = m_edcSocketWatch.Receive(a_bytes, a_bytes.Length, SocketFlags.None);
				if (bytes == 0)
				{
					Pro_blnConnect = false;
					m_edcSocketWatch.Shutdown(SocketShutdown.Both);
					m_edcSocketWatch.Close();
					this.m_evtErrorReceived?.Invoke("Net connect error");
					break;
				}
				a_strRecieve += Pro_edcEncoding.GetString(a_bytes, 0, bytes);
				this.m_evtDataReceived?.Invoke(a_strRecieve);
				a_strRecieve = string.Empty;
				continue;
			}
			catch (Exception ex)
			{
				this.m_evtErrorReceived?.Invoke(ex.Message);
				continue;
			}
		}
		while (Pro_blnConnect);
		m_edcAcceptSocketThread.Abort();
	}

	private void Sub_StartListen(object obj)
	{
		Socket a_edcScoketWatch = obj as Socket;
		while (true)
		{
			m_edcSocketSend = a_edcScoketWatch.Accept();
			m_dicSocketClients.Add(m_edcSocketSend.RemoteEndPoint.ToString(), m_edcSocketSend);
			Thread threadReceive = new Thread(Sub_Recieve);
			threadReceive.IsBackground = true;
			threadReceive.Start(m_edcSocketSend);
		}
	}

	private void Sub_Recieve(object obj)
	{
		string a_strRecieve = string.Empty;
		Socket socketSend = obj as Socket;
		try
		{
			while (true)
			{
				byte[] a_bytes = new byte[2048];
				int a_i32Bytes = socketSend.Receive(a_bytes, a_bytes.Length, SocketFlags.None);
				if (a_i32Bytes == 0)
				{
					break;
				}
				a_strRecieve += Pro_edcEncoding.GetString(a_bytes, 0, a_i32Bytes);
				this.m_evtDataReceived?.Invoke(a_strRecieve);
				a_strRecieve = string.Empty;
			}
			Pro_blnConnect = false;
			m_edcSocketWatch.Shutdown(SocketShutdown.Both);
			m_edcSocketWatch.Close();
			this.m_evtErrorReceived?.Invoke("Net connect error");
		}
		catch (Exception ex)
		{
			this.m_evtErrorReceived?.Invoke(ex.Message);
		}
	}

	public int Fun_i32Send(string a_strData)
	{
		try
		{
			byte[] bytes = Pro_edcEncoding.GetBytes(a_strData);
			m_edcSocketWatch.Send(bytes, bytes.Length, SocketFlags.None);
		}
		catch (Exception ex2)
		{
			this.m_evtErrorReceived?.Invoke(ex2.Message);
		}
		try
		{
			using Dictionary<string, Socket>.ValueCollection.Enumerator enumerator = m_dicSocketClients.Values.GetEnumerator();
			if (enumerator.MoveNext())
			{
				Socket socket = enumerator.Current;
				if (socket != null)
				{
					if (socket.Connected)
					{
						byte[] bytes2 = Pro_edcEncoding.GetBytes(a_strData);
						return socket.Send(bytes2, bytes2.Length, SocketFlags.None);
					}
					return 0;
				}
				return 0;
			}
		}
		catch (Exception ex)
		{
			this.m_evtErrorReceived?.Invoke(ex.Message);
		}
		return 0;
	}
}
