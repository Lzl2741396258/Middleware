using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Ersa.Mes.FileSystem.Model.RequestResponse;

namespace Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;

public class Edc_BasicSettings
{
	[XmlElement("WindowsStartup")]
	public bool m_blnWindowsStartup { get; set; } = false;


	[XmlElement("ErsasoftStartup")]
	public bool m_blnErsasoftStartup { get; set; }

	[XmlElement("AutoFunction")]
	public bool m_blnAutoFunction { get; set; }

	[XmlElement("AutoFunctionDelay")]
	public int m_i32AutoFunctionDelay { get; set; } = 0;


	[XmlElement("ExitVerification")]
	public bool m_blnExitVerification { get; set; } = false;


	[XmlElement("AllowMinimization")]
	public bool m_blnAllowMini { get; set; } = false;


	[XmlElement("LoginFirst")]
	public bool m_blnLoginFirst { get; set; } = false;


	[XmlElement("AutoUpdateUrl")]
	public string m_strAutoUpdateUrl { get; set; }

	[XmlElement("EquipmentID")]
	public string m_strMachineId { get; set; }

	[XmlElement("ZoneNumber")]
	public int m_i32MaxHZoneNumber { get; set; } = 10;


	[XmlElement("MachineType")]
	public string m_strMachineType { get; set; } = Enum_MachineType.Reflow.ToString();


	public Enum_MachineType m_enuMachineType
	{
		get
		{
			return (Enum_MachineType)Enum.Parse(typeof(Enum_MachineType), m_strMachineType);
		}
		set
		{
			m_strMachineType = value.ToString();
		}
	}

	[XmlElement("SpecificModel")]
	public string m_strSpecificModel { get; set; }

	[XmlElement("TrackId")]
	public List<int> m_strTrackIds { get; set; } = new List<int>();


	[XmlElement("SoftName")]
	public string m_strSoftName { get; set; }

	[XmlElement("NoUserLogin")]
	public string m_strNoUserLogin { get; set; }

	[Obsolete("所有的Interval都创建在Mes Task中")]
	[XmlElement("HeartBeatInterval")]
	public int m_i32IntervalHeartBeat { get; set; }

	[XmlElement("CRC")]
	public bool m_blnCRCAuthentication { get; set; }

	[XmlElement("LogLevel")]
	public int m_strLogLevel { get; set; }

	[XmlElement("User")]
	public string m_strUser { get; set; }

	[XmlElement("Password")]
	public string m_strPassword { get; set; }

	[XmlElement("MainFormLevel")]
	public int m_i32MainFormLevel { get; set; } = 0;


	[XmlElement("ShowLamp")]
	public bool m_blnShowLamp { get; set; } = true;


	[XmlElement("TestMode")]
	public bool m_blnTestMode { get; set; } = false;


	[XmlElement("Language")]
	public string m_strLanguage { get; set; } = "en";


	[XmlElement("ErsaKey")]
	public string m_strErsaKey { get; set; } = string.Empty;

}
