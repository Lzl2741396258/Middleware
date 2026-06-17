using System;

namespace Ersa.Mes.Middleware.Interfaces;

[Obsolete]
public interface Inf_Initiative
{
	string m_strRequest { get; set; }

	int m_i32ActCount { get; set; }

	bool Fun_blnContains();

	void Sub_GetRequest();

	string Fun_strGetResponse();
}
