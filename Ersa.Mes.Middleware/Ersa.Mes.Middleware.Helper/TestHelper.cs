using System.Net.Sockets;
using System.Threading.Tasks;

namespace Ersa.Mes.Middleware.Helper;

public static class TestHelper
{
	public static async Task<bool> Sub_TestTcpConnect(string i_strIP, int i_i32Port)
	{
		using (TcpClient a_TcpClient = new TcpClient())
		{
			if (a_TcpClient.Client == null && !(await Sub_ConnectAsync(a_TcpClient, i_strIP, i_i32Port)))
			{
				return false;
			}
			if (!a_TcpClient.Client.Connected)
			{
				bool flag = a_TcpClient != null;
				bool flag2 = flag;
				if (flag2)
				{
					flag2 = !(await Sub_ConnectAsync(a_TcpClient, i_strIP, i_i32Port));
				}
				if (flag2)
				{
					return false;
				}
			}
			if (a_TcpClient.Client.Poll(0, SelectMode.SelectRead))
			{
				byte[] buffer = new byte[1];
				if (a_TcpClient.Client.Receive(buffer, SocketFlags.Peek) == 0 && !(await Sub_ConnectAsync(a_TcpClient, i_strIP, i_i32Port)))
				{
					return false;
				}
			}
		}
		return true;
	}

	private static async Task<bool> Sub_ConnectAsync(TcpClient i_tcpClient, string i_strIP, int i_i32Port)
	{
		try
		{
			i_tcpClient = new TcpClient();
			i_tcpClient.ReceiveBufferSize = 20000;
			i_tcpClient.Connect(i_strIP, i_i32Port);
			return true;
		}
		catch
		{
			return false;
		}
	}
}
