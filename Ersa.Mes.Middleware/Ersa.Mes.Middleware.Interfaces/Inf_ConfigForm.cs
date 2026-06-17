using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;

namespace Ersa.Mes.Middleware.Interfaces;

public interface Inf_ConfigForm
{
	int m_i32FormID { get; set; }

	string Pro_FormName { get; }

	bool Fun_blnSave(Edc_ConfigBase i_Config, bool i_blnShowMessagebox = true);
}
