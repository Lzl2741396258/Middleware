using System;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.FileSystem.Service;

namespace Ersa.Mes.FileSystem.Model.Protocol;

public class Edc_ProtocolSelectiveDefault : Inf_Protocol, Inf_FileService
{
	Enum_MachineType Inf_Protocol.Pro_enuMachineType => Enum_MachineType.Selektiv;

	Enum_ProtocolType Inf_Protocol.Pro_enuProtocolType => Enum_ProtocolType.XmlDefault;

	public object Fun_dicGetData(string i_strFullname)
	{
		return i_strFullname.Fun_edcDeserializeByFilePath<Edc_ProtocolElementSelectiveDefault>();
	}

	public T Fun_edcGetData<T>(string i_strFullname) where T : class
	{
		return (T)Fun_dicGetData(i_strFullname);
	}

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

	public Task<string> Fun_strGetData(string i_strFullname)
	{
		throw new NotImplementedException();
	}
}
