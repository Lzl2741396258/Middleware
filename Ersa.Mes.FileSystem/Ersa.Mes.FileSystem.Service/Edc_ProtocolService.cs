using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.Common.Extensions;
using Ersa.Mes.Common.Helper;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Model.Protocol;
using Ersa.Mes.FileSystem.Model.RequestResponse;

namespace Ersa.Mes.FileSystem.Service;

public class Edc_ProtocolService : Inf_Protocol, Inf_FileService
{
	Enum_MachineType Inf_Protocol.Pro_enuMachineType => m_enuMachineType;

	Enum_ProtocolType Inf_Protocol.Pro_enuProtocolType => m_enuProtocolType;

	private Enum_MachineType m_enuMachineType { get; set; }

	private Enum_ProtocolType m_enuProtocolType { get; set; }

	public Edc_ProtocolService(Enum_MachineType i_enuMachineType, Enum_ProtocolType i_enuProtocolType)
	{
		m_enuMachineType = i_enuMachineType;
		m_enuProtocolType = i_enuProtocolType;
	}

	public static Struct_ProtocolElement Sub_GetProtocolElement(Struct_ProtocolElement[] ia_sttPE, Enum_ProtocolElementSelective i_enuPE)
	{
		return ia_sttPE.ToList().Find((Struct_ProtocolElement c) => c.m_strName.Equals(i_enuPE.Fun_strGetDescription()));
	}

	public int Fun_i32GetData(string i_strParaName, object i_objData)
	{
		if (i_objData is Edc_ParameterMeasuring[])
		{
			List<Edc_ParameterMeasuring> list = ((Edc_ParameterMeasuring[])i_objData).ToList();
			Edc_ParameterMeasuring para2 = list.Where((Edc_ParameterMeasuring s) => s.m_strEquipment.Equals(i_strParaName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
			if (para2 != null)
			{
				return ConvertHelper.Fun_i32ConvertToInt32(para2.m_strChannel.m_strSample.m_strValue);
			}
		}
		else if (i_objData is Edc_ParameterProcess[])
		{
			List<Edc_ParameterProcess> list2 = ((Edc_ParameterProcess[])i_objData).ToList();
			Edc_ParameterProcess para = list2.Where((Edc_ParameterProcess s) => s.m_strName.Equals(i_strParaName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
			if (para != null)
			{
				return ConvertHelper.Fun_i32ConvertToInt32(para.m_strValue);
			}
		}
		return 0;
	}

	public Task<string> Fun_strGetData(string i_strFullname)
	{
		return Task.FromResult(File.ReadAllText(i_strFullname));
	}

	public T Fun_edcGetData<T>(string i_strFullname) where T : class
	{
		switch (m_enuProtocolType)
		{
		case Enum_ProtocolType.XmlZevi:
			return i_strFullname.Fun_edcDeserializeByFilePath<T>();
		case Enum_ProtocolType.protocol:
		{
			Inf_Protocol protocol = ProtocolFactory.Fun_edcCreateProtocol(Enum_ProtocolType.WaveTxt);
			return (T)protocol.Fun_edcGetData<IDictionary<string, string>>(i_strFullname);
		}
		default:
			throw new Exception("No Protocol Type...");
		case Enum_ProtocolType.XmlDefault:
		case Enum_ProtocolType.csv:
			return null;
		}
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
}
