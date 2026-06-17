using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public class Request_UserLog
{
	[XmlElement("Header")]
	[JsonProperty(PropertyName = "Header")]
	public Struct_HeaderWithoutTrack m_sttHeader;

	[XmlElement("Maschinentyp")]
	[JsonProperty(PropertyName = "MachineType")]
	public Enum_MachineType m_enmMachineType;

	[XmlElement("Vorgang")]
	[JsonProperty(PropertyName = "User")]
	public Enum_UserOption m_enmUser;

	[XmlElement("BenutzerId")]
	[JsonProperty(PropertyName = "UserID")]
	public string m_strUserID;

	[XmlElement("Passwort")]
	[JsonProperty(PropertyName = "Password")]
	public string m_strPassword;
}
