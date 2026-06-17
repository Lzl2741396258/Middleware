using Ersa.Mes.FileSystem.Service;

namespace Ersa.Mes.FileSystem.Interface;

public interface Inf_IniFiles : Inf_FileService
{
	string Sub_GetParameterValue(string i_strGroup, string i_strParameter);
}
