using System.IO.Ports;
using System.Text;
using Ersa.Mes.Middleware.MesTaskFolder;

namespace Ersa.Mes.Middleware.Device;

public interface Inf_DeviceSerialPort : Inf_Device
{
	SerialPort Pro_SystemSerialPort { get; set; }

	string Pro_strCom { get; set; }

	int Pro_i32BaudRate { get; set; }

	Parity Pro_edcParity { get; set; }

	int Pro_i32DataBits { get; set; }

	Encoding Pro_edcEncoding { get; set; }

	int Pro_i32ReadTimeout { get; set; }

	int Pro_i32WriteTimeout { get; set; }

	event SerialDataReceivedEventHandle m_evtReceived;

	event SerialErrorReceivedEventHandler m_evtError;

	bool Fun_blnCheckDataFormat(Enum_DataFormat i_enuDataCheck, ref byte[] i_bytOut);

	bool Fun_blnSendData(byte[] a_bytData);

	bool Fun_blnSendData(byte[] a_bytData, int a_i32Offset, int a_i32Count);
}
