namespace Ersa.Mes.Middleware.Device;

public interface Inf_Device
{
	string m_strRemark { get; set; }

	string Fun_strGetName();

	bool Fun_blnOpen();

	bool Fun_blnClose();

	bool Fun_IsOpen();

	bool Fun_blnSend(byte[] i_bytData);

	bool Fun_blnSend(string i_strData);
}
