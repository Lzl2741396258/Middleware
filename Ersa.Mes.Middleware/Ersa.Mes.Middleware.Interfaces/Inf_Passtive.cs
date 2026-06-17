using System;
using System.Threading.Tasks;

namespace Ersa.Mes.Middleware.Interfaces;

[Obsolete]
public interface Inf_Passtive
{
	string m_strRequest { get; set; }

	int m_i32ActCount { get; set; }

	bool Fun_blnContains();

	void Sub_GetRequest();

	Task<string> Fun_strExecute();
}
