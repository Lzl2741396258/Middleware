using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Ersa.Mes.Middleware.Helper;

public class SocketHelper
{
	public delegate void ReceiveDataDelegate(int state, string message);

	private bool canReadData;

	private Socket _socketClient;

	private Socket m_SocketServer;

	private string errormessage;

	private Thread ReceiveDataThread;

	private Thread listenConnectThread;

	public bool Active => canReadData;

	public string Errormessage => errormessage;

	public event ReceiveDataDelegate SockReceiveData;

	public event ReceiveDataDelegate SockStateEvent;

	public SocketHelper()
	{
		canReadData = false;
		errormessage = " ";
	}

	public bool OpenServer(string _serverip, int _port)
	{
		try
		{
			m_SocketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			m_SocketServer.Bind(new IPEndPoint(IPAddress.Parse(_serverip), _port));
			m_SocketServer.Listen(5);
			listenConnectThread = new Thread(ListenClient);
			listenConnectThread.IsBackground = true;
			listenConnectThread.Start();
			return true;
		}
		catch (ArgumentNullException ex)
		{
			errormessage = ex.Message.ToString();
			this.SockStateEvent(3, errormessage);
			return false;
		}
		catch (SocketException ex2)
		{
			errormessage = ex2.Message.ToString();
			return false;
		}
	}

	private void ListenClient()
	{
		while (true)
		{
			try
			{
				_socketClient = m_SocketServer.Accept();
				_socketClient.Send(new byte[1] { 8 });
				ReceiveDataThread = new Thread(ReceiveDataServer);
				ReceiveDataThread.IsBackground = true;
				ReceiveDataThread.Start();
				Thread.Sleep(5);
			}
			catch
			{
			}
		}
	}

	private void ReceiveDataServer()
	{
		while (true)
		{
			Thread.Sleep(5);
			try
			{
				byte[] buffer = new byte[256];
				int receivedBytesCount = m_SocketServer.Receive(buffer);
				string receivedStr = Encoding.Default.GetString(buffer).TrimEnd(default(char));
				if (receivedBytesCount == 0)
				{
					canReadData = false;
					m_SocketServer.Shutdown(SocketShutdown.Both);
					m_SocketServer.Close();
					this.SockStateEvent(1, "网络断开连接");
					break;
				}
				this.SockReceiveData(receivedBytesCount, receivedStr);
			}
			catch (Exception ex)
			{
				errormessage = ex.Message.ToString();
			}
			bool flag = true;
		}
	}

	public int ServerSend(string a_Message)
	{
		if (m_SocketServer != null)
		{
			if (m_SocketServer.Connected)
			{
				byte[] send = Encoding.ASCII.GetBytes(a_Message);
				return m_SocketServer.Send(send, send.Length, SocketFlags.None);
			}
			return 0;
		}
		return 0;
	}

	public int ServerSend(byte[] a_Message)
	{
		if (m_SocketServer != null)
		{
			if (m_SocketServer.Connected)
			{
				return m_SocketServer.Send(a_Message, a_Message.Length, SocketFlags.None);
			}
			return 0;
		}
		return 0;
	}

	public bool OpenClient(string _ip, int _port)
	{
		if (_socketClient != null && _socketClient.Connected)
		{
			return true;
		}
		try
		{
			IPEndPoint serverRemote = new IPEndPoint(IPAddress.Parse(_ip), _port);
			_socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_socketClient.Connect(serverRemote);
			canReadData = true;
			ReceiveDataThread = new Thread(ReceiveDataClient);
			ReceiveDataThread.IsBackground = true;
			ReceiveDataThread.Start();
		}
		catch (ArgumentNullException ex)
		{
			errormessage = ex.Message.ToString();
			this.SockStateEvent(3, errormessage);
			return false;
		}
		catch (SocketException ex2)
		{
			errormessage = ex2.Message.ToString();
			return false;
		}
		return true;
	}

	private void ReceiveDataClient()
	{
		do
		{
			Application.DoEvents();
			Thread.Sleep(5);
			try
			{
				byte[] buffer = new byte[256];
				int receivedBytesCount = _socketClient.Receive(buffer);
				string receivedStr = Encoding.Default.GetString(buffer).TrimEnd(default(char));
				if (receivedBytesCount == 0)
				{
					canReadData = false;
					_socketClient.Shutdown(SocketShutdown.Both);
					_socketClient.Close();
					this.SockStateEvent(1, "网络断开连接");
					break;
				}
				this.SockReceiveData(receivedBytesCount, receivedStr);
				continue;
			}
			catch (Exception ex)
			{
				errormessage = ex.Message.ToString();
				continue;
			}
		}
		while (canReadData);
		ReceiveDataThread.Abort();
	}

	public int SendClient(string message)
	{
		if (_socketClient != null)
		{
			if (_socketClient.Connected)
			{
				byte[] send = Encoding.ASCII.GetBytes(message);
				return _socketClient.Send(send, send.Length, SocketFlags.None);
			}
			return 0;
		}
		return 0;
	}

	public void CloseClient()
	{
		canReadData = false;
		if (ReceiveDataThread != null)
		{
			ReceiveDataThread.Abort();
		}
		if (_socketClient != null)
		{
			try
			{
				_socketClient.Shutdown(SocketShutdown.Both);
			}
			finally
			{
				_socketClient.Close();
			}
		}
	}
}
