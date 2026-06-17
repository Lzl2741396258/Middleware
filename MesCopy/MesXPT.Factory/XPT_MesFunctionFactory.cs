using System;
using System.Collections.Generic;
using System.Linq;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Factory;
using Ersa.Mes.Middleware.Interfaces;
using Ersa.Mes.Middleware.MesFunctionFolder;
using MesDBG.DBG_MesFunction;
using MesXPT.Model;
using MesXPT.XPT_MesFunction;

namespace MesXPT.Factory;

public class XPT_MesFunctionFactory : Edc_MesFunctionContainer, Inf_MesTask
{
	public XPT_Config m_Config { get; set; }

	public List<string> m_lstSpray { get; set; }

	public XPT_MesFunctionFactory(XPT_Config i_Config, Inf_Logger i_edcLogger, Edc_MesTaskAttributes a_MesAttributes1)
		: base(i_Config, i_edcLogger, i_Config.ma_MesTask.Where((Edc_MesTaskAttributes s) => s.Pro_strName.Equals(Enum_TaskName.MES.ToString(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault())
	{
		m_Config = i_Config;
    }

	protected override void Sub_AddMesFunction()
	{

        Sub_AddObject<MachineCondition>(new XPT_MachineCondition(m_Config, base.m_edcLogger));
        Sub_AddObject<ChooseRecipe>(new XPT_ChooseRecipe(m_Config, base.m_edcLogger));
        Sub_AddObject<ReleaseInfeed>(new XPT_ReleaseInfeed(m_Config, base.m_edcLogger));
        //Sub_AddObject<SelectProgram>(new XPT_SelectProgram(m_Config, base.m_edcLogger));
        Sub_AddObject<OutfeedCreateProtocol>(new XPT_OutfeedCreateProtocolSelektiv(m_Config, base.m_edcLogger));
        Sub_AddObject<SelectProgram>(new XPT_SelectProgram(m_Config, base.m_edcLogger));
        Sub_AddObject<PopupDialog>(new XPT_PopupDialog(m_Config, base.m_edcLogger));
    }
}
