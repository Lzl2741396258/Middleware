using System;
using System.Threading.Tasks;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Service;

namespace Ersa.Mes.FileSystem.Model.Config;

public class Edc_ErsasoftIni : Inf_IniFiles, Inf_FileService
{
	public string m_strPath { get; set; }

	public T Fun_edcGetData<T>(string i_strFullname) where T : class
	{
		return (T)Fun_GetAllData(i_strFullname);
	}

	public object Fun_GetAllData(string i_strFullname)
	{
		throw new NotImplementedException();
	}

	public string Sub_GetParameterValue(string i_strGroup, string i_strParameter)
	{
		return string.Empty;
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
