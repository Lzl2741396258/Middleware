using System.Text;

namespace Ersa.Mes.Middleware.Device;

public interface Inf_DeviceNet : Inf_Device
{
	int Pro_i32Interval { get; set; }

	bool Pro_blnFlagRecieveEvent { get; set; }

	Edc_Socket Pro_edcSocketSystem { get; set; }

	string Pro_strIP { get; set; }

	int Pro_i32Port { get; set; }

	Encoding Pro_edcEncoding { get; set; }

	int Pro_i32ReadTimeout { get; set; }

	int Pro_i32WriteTimeout { get; set; }

	event Evt_SocketDataReceivedEventHandle Evt_Received;

	event Evt_SocketErrorDataReceivedEventHandle Error;

	bool Fun_blnSendData(byte[] a_bytes);

	bool Fun_blnSendData(byte[] a_bytes, int i_i32Offset, int i_i32Count);
}
