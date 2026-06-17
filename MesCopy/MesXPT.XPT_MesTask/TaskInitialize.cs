using System;
using System.Threading.Tasks;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Factory;
using Ersa.Mes.Middleware.Interfaces;
using MesXPT.Model;

namespace MesXPT.XPT_MesTask;

public class TaskInitialize : Edc_MesTask, Inf_MesTask
{
	private XPT_Config m_Config { get; set; }

	public TaskInitialize(XPT_Config i_Config, Inf_Logger i_edcLogger, Inf_MesTaskAttributes i_edcMesTaskBase)
		: base(i_edcMesTaskBase, i_edcLogger)
	{
		m_Config = i_Config;
	}

	public override Task Sub_Act()
	{
		try
		{
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, ex.Message);
		}
		return Task.CompletedTask;
	}
}
