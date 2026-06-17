using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ersa.Mes.PLC.Model;

namespace Ersa.Mes.PLC.Interfaces;

public interface Inf_CommunicationService : IDisposable
{
	Task Fun_fdcConnect(bool i_blnOnline);

	void Sub_ReadValue(Edc_PLCElement i_edcSpsElement);

	Task<string> Fun_fdcReadValueAsync(string i_strTaskName, string i_strPVariable, string i_strVariableName);

	Task Fun_fdcReadValueAsync(IEnumerable<Edc_PLCElement> i_lstSpsElement, CancellationToken i_fdcCancellationToken);

	Task Fun_fdcCreateVariableToGroup(IEnumerable<Edc_PLCElement> i_lstElement, string i_strGroupName, int i_i32CycleTime = 100);
}
