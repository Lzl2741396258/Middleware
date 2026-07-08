using System;
using System.Xml.Serialization;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;

namespace MesXPT.Model;

[Serializable]
[XmlRoot("root")]
[XmlInclude(typeof(Edc_ConfigBase))]
public class XPT_Config : Edc_ConfigBase
{
	[XmlElement("TestMode")]
	public string m_strTestMode { get; set; }

	[XmlElement("LineCode")]
	public string m_strLineCode { get; set; }

	[XmlElement("WorkStationCode")]
	public string m_strWorkStationCode { get; set; }

	[XmlElement("Account")]
	public string m_strAccount { get; set; }

	[XmlElement("MoNo")]
	public string m_strMoNo { get; set; }

	[XmlElement("PartNo")]
	public string m_strPartNo { get; set; }

	[XmlElement("Version")]
	public string m_strVersion { get; set; }

	[XmlElement("No")]
	public string m_strNo { get; set; }

	[XmlElement("EquipNo")]
	public string m_strEquipNo { get; set; }

	[XmlElement("database")]
	public string m_strDatabase { get; set; }

	[XmlElement("Code")]
	public string m_strCode { get; set; }

	[XmlElement("DatabaseSever")]
	public string m_strDatabaseSever { get; set; }

	[XmlElement("DatabaseUser")]
	public string m_strDatabaseUser { get; set; }

	[XmlElement("DatabasePassword")]
	public string m_strDatabasePassword { get; set; }

	[XmlElement("comPort")]
	public string m_strcomPort { get; set; }

	[XmlElement("ersaetx")]
	public string m_strersa { get; set; }

	[XmlElement("CustemCode")]
	public string m_strCustemCode { get; set; }

    [XmlElement("UrlResultUpload")]
    public string m_strUrlResultUpload { get; set; }

    [XmlElement("HttpMethod")]
    public string m_strHttpMethod { get; set; }

    [XmlElement("HttpTimeout")]
    public int m_i32HttpTimeout { get; set; }

    [XmlElement("ContentType")]
    public string m_strContentType { get; set; }

    [XmlElement("ITACWorkNodeID")]
    public string m_strITACWorkNodeID { get; set; }
    [XmlElement("ITACUserID")]
    public string m_strITACUserID { get; set; }

    [XmlElement("ITACUploadStatusURL")]
    public string m_strITACUploadStatusURL { get; set; }

    [XmlElement("ITACUploadResultURL")]
    public string m_strITACUploadResultURL { get; set; }

    // WCF 换型服务配置
    [XmlElement("ChangeOverServiceEnabled")]
    public string m_strChangeOverServiceEnabled { get; set; } = "true";

    [XmlElement("ChangeOverServiceIP")]
    public string m_strChangeOverServiceIP { get; set; } = "127.0.0.1";

    [XmlElement("ChangeOverServicePort")]
    public int m_i32ChangeOverServicePort { get; set; } = 8080;

    // ADD BY Jaden
    public string m_ersaDownLineUrl { get; set; }

	public string m_snCode { get; set; }

    public string m_CommandType { get; set; }//命令类型
    public string m_LocalTime { get; set; }//本地时间
    public string m_Line { get; set; }//产线名称
    public string m_MachineCode { get; set; }//设备编号
    public string m_Barcode { get; set; }//条码
    public string m_Lane { get; set; }//轨道类型
    public string m_Layer { get; set; }//面别
    public string m_Program { get; set; }//程序名

    public string m_Account { get; set; }//登陆账号
    public string m_RecipeName { get; set; }//设备名称
	public string m_ResourceName { get; set; }//设备程序名称

    public string m_strITACMESType { get; set; } //Itac标志

	public bool	m_ErsachooseRecipe { get; set; } = false;
    public string m_checkCompeletResult { get; set; }

    public string m_ersaOnlineUrl { get; set; }
    public string m_completedUrl { get; set; }
    public string m_completed { get; set; }
    public string m_txtLibrary { get; set; }
    public string m_txtFilePath { get; set; }

    public string m_checkChangeOver { get; set; }

    public string m_processCode { get; set; }

	public string m_carrierCode { get; set; }

	public string m_adapterPlateCode { get; set; }

	public string m_programFilePath { get; set; }

	public string m_side { get; set; }

    public XPT_Config()
	{
		base.m_edcFilesPath.m_strPathErsasoft = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\MesXPT.xml";
	}

	public XPT_Config(string m_strPath = "")
		: base(m_strPath)
	{
		base.m_edcFilesPath.m_strPathErsasoft = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\MesXPT.xml";
	}
}
