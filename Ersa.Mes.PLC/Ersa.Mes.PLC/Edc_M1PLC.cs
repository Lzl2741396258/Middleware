using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ersa.Mes.PLC.Model;

namespace Ersa.Mes.PLC;

public class Edc_M1PLC : Inf_PLC, IDisposable
{
	public void Dispose()
	{
		throw new NotImplementedException();
	}

	public bool Fun_blnReadValue(string i_strVariable)
	{
		throw new NotImplementedException();
	}

	public byte Fun_bytReadValue(string i_strVariable)
	{
		throw new NotImplementedException();
	}

	public IDisposable Fun_fdcEventHandlerRegister(string i_strVariable, Action i_delHandler)
	{
		throw new NotImplementedException();
	}

	public Task Fun_fdcGroupCreateVariableAsync(IEnumerable<string> i_enuVariable, string i_strGroupName, int i_i32CycleTime = 100)
	{
		throw new NotImplementedException();
	}

	public short Fun_i16ReadValue(string i_strVariable)
	{
		throw new NotImplementedException();
	}

	public int Fun_i32ReadValue(string i_strVariable)
	{
		throw new NotImplementedException();
	}

	public float Fun_sngReadValue(string i_strVariable)
	{
		throw new NotImplementedException();
	}

	public string Fun_strReadValue(string i_strVariable)
	{
		throw new NotImplementedException();
	}

	public ushort Fun_u16ReadValue(string i_strVariable)
	{
		throw new NotImplementedException();
	}

	public uint Fun_u32ReadValue(string i_strVariable)
	{
		throw new NotImplementedException();
	}

	public Task Fun_ConnectAsync(bool i_blnOnline, string i_strAddress)
	{
		throw new NotImplementedException();
	}

	public void Sub_DisConnect()
	{
		throw new NotImplementedException();
	}

	public void Sub_GroupEventActive()
	{
		throw new NotImplementedException();
	}

	public void Sub_GroupEventDisactivate()
	{
		throw new NotImplementedException();
	}

	public Task<string> Sub_ReadValueAsync(string i_strTaskName, string i_strPVariable, string i_strVariableName)
	{
		throw new NotImplementedException();
	}

	public Task Sub_VariablesRegisterAsync(IEnumerable<string> i_lstVariable, CancellationToken i_fdcToken)
	{
		throw new NotImplementedException();
	}

	public Task Sub_VariablesUnregister(IEnumerable<string> i_lstVariable, CancellationToken i_fdcToken)
	{
		throw new NotImplementedException();
	}

	public void Sub_WriteValue(string i_strVariable, string i_strValue)
	{
		throw new NotImplementedException();
	}

	public Task<bool> Sub_WriteValueAsync(string i_strTaskName, string i_strPVariable, string i_strVariableName, string i_strValue)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<Edc_PLCElement>> Fun_fdcGroupReadAsync(string i_strGruppenName)
	{
		throw new NotImplementedException();
	}

	public Task Fun_fdcGroupActiveAsync(string i_strGroupName)
	{
		throw new NotImplementedException();
	}

	public Task FUN_fdcGroupDisableAsync(string i_strGroupName)
	{
		throw new NotImplementedException();
	}

	public void Sub_WriteValue(string i_strVariable, string i_strValue, string i_strMembers)
	{
		throw new NotImplementedException();
	}

	public Task Sub_WriteValueAsync(string i_strVariable, IEnumerable<KeyValuePair<string, string>> i_listMembers)
	{
		throw new NotImplementedException();
	}

	public async Task<List<Edc_PLCElement>> Sub_lstReadValue(string i_strPVariable)
	{
		throw new NotImplementedException();
	}

	public Task Sub_WriteMesAddress(string i_strTaskDotVariable)
	{
		throw new NotImplementedException();
	}
}
