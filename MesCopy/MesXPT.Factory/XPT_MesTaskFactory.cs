using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Factory;
using MesXPT.Model;
using MesXPT.XPT_MesPLC;
using System.Linq;

namespace MesXPT.Factory;

public class XPT_MesTaskFactory : Edc_MesTaskContainer
{
	public XPT_Config m_Config { get; set; }

	public XPT_MesTaskFactory(XPT_Config i_Config, Inf_Logger i_Logger)
		: base(i_Config, i_Logger)
	{
		m_Config = i_Config;
	}

/*	protected override void Sub_AddMesTaskReflow()
	{
		Sub_AddObject<Edc_MesFunctionContainer>(new XPT_MesFunctionFactory(m_Config, m_edcLogger));
	}*/

	protected override void Sub_AddMesTaskSelective()
	{
		// John
        //base.Sub_AddMesTaskSelective();
      //  Sub_AddObject<Edc_MesFunctionContainer>(new XPT_MesFunctionFactory(m_Config, m_edcLogger));

    }

	protected override void Sub_AddMesTaskWave()
	{
		base.Sub_AddMesTaskWave();
	}


    protected override void Sub_AddMesTaskReflow()
    {
        Edc_MesTaskAttributes a_MesAttributes1 = m_Config.ma_MesTask.Where((Edc_MesTaskAttributes s) => s.Pro_strName.Equals("Mes")).FirstOrDefault();
        if (a_MesAttributes1 != null && a_MesAttributes1.Pro_blnActive)
        {
            Edc_MesTask task1 = new XPT_MesFunctionFactory(m_Config, m_edcLogger, a_MesAttributes1);
            Sub_AddObject<Edc_MesFunctionContainer>(task1);
            OnShowMessage(Enum_LogType.Info, "Task 'Mes' Start");


        }
        Edc_MesTaskAttributes a_MesAttributes2 = m_Config.ma_MesTask.Where((Edc_MesTaskAttributes s) => s.Pro_strName.Equals("Magna_PLC")).FirstOrDefault();
        if (a_MesAttributes2 != null && a_MesAttributes2.Pro_blnActive)
        {
            Sub_AddObject<XPT_PLC>(new XPT_PLC(m_Config, a_MesAttributes2, m_edcLogger));
            OnShowMessage(Enum_LogType.Info, "Task 'Magna_PLC' Start");
        }
    }
}
