using System;
using System.Collections.Generic;

namespace Ersa.Mes.PLC;

public class Edc_ActionForParameterTypes
{
	private readonly Inf_PLCProvider m_edcSpsProvider;

	private Inf_PLC m_edcSpsServer;

	private Inf_PLC Pro_edcSpsService => m_edcSpsServer ?? (m_edcSpsServer = m_edcSpsProvider.Fun_edcActiveSps());

	public IDictionary<Enum_PLCType, Func<string, object>> Pro_dicReadAction { get; set; }

	public IDictionary<Enum_PLCType, Action<string, object>> Pro_dicWriteAction { get; set; }

	public Edc_ActionForParameterTypes(Inf_PLCProvider i_edcSpsProvider)
	{
		m_edcSpsProvider = i_edcSpsProvider;
	}

	private void Sub_InitializeReadAction()
	{
		Pro_dicReadAction = new Dictionary<Enum_PLCType, Func<string, object>>
		{
			{
				Enum_PLCType.enmUInt32,
				(string s) => Pro_edcSpsService.Fun_u32ReadValue(s)
			},
			{
				Enum_PLCType.enmInt32,
				(string s) => Pro_edcSpsService.Fun_i32ReadValue(s)
			},
			{
				Enum_PLCType.enmUInt16,
				(string s) => Pro_edcSpsService.Fun_i16ReadValue(s)
			},
			{
				Enum_PLCType.enmInt16,
				(string s) => Pro_edcSpsService.Fun_i16ReadValue(s)
			},
			{
				Enum_PLCType.enmByte,
				(string s) => Pro_edcSpsService.Fun_bytReadValue(s)
			},
			{
				Enum_PLCType.enmBool,
				(string s) => Pro_edcSpsService.Fun_blnReadValue(s)
			},
			{
				Enum_PLCType.enmString,
				(string s) => Pro_edcSpsService.Fun_strReadValue(s)
			},
			{
				Enum_PLCType.enmSingle,
				(string s) => Pro_edcSpsService.Fun_sngReadValue(s)
			}
		};
	}

	private void Sub_InitializeWriteAction()
	{
		Pro_dicWriteAction = new Dictionary<Enum_PLCType, Action<string, object>>();
		foreach (Enum_PLCType item in Enum.GetValues(typeof(Enum_PLCType)))
		{
			Pro_dicWriteAction.Add(item, delegate(string s, object o)
			{
				Pro_edcSpsService.Sub_WriteValue(s, o.ToString());
			});
		}
	}
}
