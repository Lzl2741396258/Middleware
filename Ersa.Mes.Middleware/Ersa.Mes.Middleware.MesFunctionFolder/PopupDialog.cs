using System;
using System.Reflection;
using System.Threading.Tasks;
using Ersa.Mes.FileSystem.Model.RequestResponse.Request;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Factory;

namespace Ersa.Mes.Middleware.MesFunctionFolder;

public class PopupDialog : Edc_MesFunction
{
	public Request_PopupDialog m_Request;

	protected override string m_strFunctionName => "PopupDialog";

	public override bool m_blnResponse => false;

	public PopupDialog(Inf_Logger i_edcLogger)
		: base(i_edcLogger)
	{
		base.m_blnInitiative = true;
		i_edcLogger.Debug($"Assmebly MES Function {m_strFunctionName}   Platform:{base.m_blnActivePlatform}  Database:{base.m_blnActiveDatabase}  LocalFile:{base.m_blnActiveLocalFile}", null, ".ctor", 37);
	}

	public override Task<string> Fun_strExecute()
	{
		string a_strResponse = string.Empty;
		try
		{
			if (base.m_blnActivePlatform)
			{
				Task.Run((Func<Task>)Fun_blnConnectMesPlatform);
			}
			if (base.m_blnActivePlatform)
			{
				base.m_edcLogger.Debug("Begin " + MethodBase.GetCurrentMethod().DeclaringType.Name + ".Fun_blnConnectMesPlatform()...", null, "Fun_strExecute", 56);
				Task.Run((Func<Task>)Fun_blnConnectMesPlatform);
			}
			if (base.m_blnActiveLocalFile)
			{
				Task.Run(delegate
				{
					Sub_WriteToLocalFile(MethodBase.GetCurrentMethod().DeclaringType.Name);
				});
			}
			base.m_i32ActCount++;
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "MES Function 'PopupDialog' Error...Details:'" + ex.Message + "'");
		}
		return Task.FromResult(a_strResponse);
	}
}
