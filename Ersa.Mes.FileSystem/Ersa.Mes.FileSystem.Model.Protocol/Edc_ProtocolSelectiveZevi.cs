using System;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.FileSystem.Service;

namespace Ersa.Mes.FileSystem.Model.Protocol;

[Serializable]
[XmlRoot("unitData")]
public class Edc_ProtocolSelectiveZevi : Inf_Protocol, Inf_FileService
{
	Enum_MachineType Inf_Protocol.Pro_enuMachineType => Enum_MachineType.Selektiv;

	Enum_ProtocolType Inf_Protocol.Pro_enuProtocolType => Enum_ProtocolType.XmlZevi;

	[XmlAttribute("unit")]
	public string m_strUnit { get; set; }

	[XmlAttribute("equipment")]
	public string m_strEquipment { get; set; }

	[XmlAttribute("starttime")]
	public string m_strStarttime { get; set; }

	[XmlAttribute("endtime")]
	public string m_strEndTime { get; set; }

	[XmlAttribute("state")]
	public string m_strState { get; set; }

	[XmlArray("processingParameters")]
	[XmlArrayItem("parameter")]
	public Edc_ParameterProcess[] ma_strProcessingParameters { get; set; }

	[XmlElement("measuring")]
	public Edc_ParameterMeasuring[] ma_sttMeasuring { get; set; }

	public bool Fun_blnUploadToDatabase(object i_objData)
	{
		throw new NotImplementedException();
	}

	public bool Fun_blnUploadToPlatform(object i_objData)
	{
		throw new NotImplementedException();
	}

	public object Fun_objConvert(object i_objData)
	{
		throw new NotImplementedException();
	}

	public T Fun_edcGetData<T>(string i_strFullname) where T : class
	{
		throw new NotImplementedException();
	}

	public Task<string> Fun_strGetData(string i_strFullname)
	{
		throw new NotImplementedException();
	}
}
