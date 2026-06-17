using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.FileSystem.Service;

namespace Ersa.Mes.FileSystem.Model.Protocol;

public class Edc_ProtocolReflowTxt : Inf_Protocol, Inf_FileService
{
	Enum_MachineType Inf_Protocol.Pro_enuMachineType => Enum_MachineType.Reflow;

	Enum_ProtocolType Inf_Protocol.Pro_enuProtocolType => Enum_ProtocolType.protocol;

	public object Fun_dicGetData(string i_strFullpath)
	{
		string[] a_strErsasoftProtocl = File.ReadAllLines(i_strFullpath, Encoding.UTF8);
		string[] strTitles = a_strErsasoftProtocl[0].Split(';');
		string[] strValues = a_strErsasoftProtocl[1].Split(';');
		Dictionary<string, string> a_dic = new Dictionary<string, string>();
		for (int i = 0; i < strTitles.Length; i++)
		{
			a_dic.Add(strTitles[i], strValues[i]);
		}
		return a_dic;
	}

	public T Fun_edcGetData<T>(string i_strFullpath) where T : class
	{
		return (T)Fun_dicGetData(i_strFullpath);
	}

	public object Fun_objConvert(object i_objData)
	{
		throw new NotImplementedException();
	}

	public bool Fun_blnUploadToDatabase(object i_objData)
	{
		throw new NotImplementedException();
	}

	public bool Fun_blnUploadToPlatform(object i_objData)
	{
		throw new NotImplementedException();
	}

	public Task<string> Fun_strGetData(string i_strFullname)
	{
		throw new NotImplementedException();
	}
}
