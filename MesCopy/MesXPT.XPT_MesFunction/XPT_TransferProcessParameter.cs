using System.Linq;
using System.Threading.Tasks;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.MesFunctionFolder;
using MesXPT.CommunicationService;
using MesXPT.Model;

namespace MesXPT.XPT_MesFunction;

public class XPT_TransferProcessParameter : TransferProcessParameter
{
	private XPT_Config m_Config { get; set; }

	private Edc_XPTCommunicationService m_edcService { get; set; }

	public XPT_TransferProcessParameter(XPT_Config i_Config, Inf_Logger i_edcLogger)
		: base(Enum_GetParameterType.ALL, i_Config.m_lstMesFunction, i_edcLogger)
	{
		m_Config = i_Config;
		m_edcService = new Edc_XPTCommunicationService(m_Config, base.m_edcLogger);
	}

	protected override Task Fun_blnConnectMesPlatform()
	{
		XPT_Data.m_lstErsaData = m_Request.m_sttParameterGroup.ma_sttParameter.ToList();
		return Task.FromResult(result: true);
	}

	protected override void OnUserChanged(string i_strUserName)
	{
		base.OnUserChanged(i_strUserName);
		Task.Run(delegate
		{
		});
	}
}
