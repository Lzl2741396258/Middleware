using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ersa.Mes.PLC.Model;

namespace Ersa.Mes.PLC;

public interface Inf_PLC : IDisposable
{
	Task Fun_ConnectAsync(bool i_blnOnline, string i_strAddress);

	void Sub_DisConnect();

	string Fun_strReadValue(string i_strVariable);

	float Fun_sngReadValue(string i_strVariable);

	uint Fun_u32ReadValue(string i_strVariable);

	int Fun_i32ReadValue(string i_strVariable);

	short Fun_i16ReadValue(string i_strVariable);

	ushort Fun_u16ReadValue(string i_strVariable);

	byte Fun_bytReadValue(string i_strVariable);

	bool Fun_blnReadValue(string i_strVariable);

	Task<List<Edc_PLCElement>> Sub_lstReadValue(string i_strPVariable);

	void Sub_WriteValue(string i_strVariable, string i_strValue);

	void Sub_WriteValue(string i_strVariable, string i_strValue, string i_strMembers);

	Task Sub_WriteMesAddress(string i_strTaskDotVariable);

	Task Sub_WriteValueAsync(string i_strTaskDotVariable, IEnumerable<KeyValuePair<string, string>> i_listMembers);

	IDisposable Fun_fdcEventHandlerRegister(string i_strVariable, Action i_delHandler);

	Task Sub_VariablesRegisterAsync(IEnumerable<string> i_lstVariable, CancellationToken i_fdcToken);

	Task Sub_VariablesUnregister(IEnumerable<string> i_lstVariable, CancellationToken i_fdcToken);

	void Sub_GroupEventActive();

	void Sub_GroupEventDisactivate();

	Task Fun_fdcGroupCreateVariableAsync(IEnumerable<string> i_enuVariable, string i_strGroupName, int i_i32CycleTime = 100);

	Task<IEnumerable<Edc_PLCElement>> Fun_fdcGroupReadAsync(string i_strGruppenName);

	Task Fun_fdcGroupActiveAsync(string i_strGroupName);

	Task FUN_fdcGroupDisableAsync(string i_strGroupName);
}
