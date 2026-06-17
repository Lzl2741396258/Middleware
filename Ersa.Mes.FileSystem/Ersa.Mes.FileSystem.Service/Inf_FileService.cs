using System.Threading.Tasks;

namespace Ersa.Mes.FileSystem.Service;

public interface Inf_FileService
{
	Task<string> Fun_strGetData(string i_strFullname);

	T Fun_edcGetData<T>(string i_strFullname) where T : class;

	bool Fun_blnUploadToPlatform(object i_objData);

	bool Fun_blnUploadToDatabase(object i_objData);
}
