using System.Threading.Tasks;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Factory;
using Ersa.Mes.Middleware.Interfaces;

namespace Ersa.Mes.Middleware.MesTaskFolder;

public class Edc_SendStatus : Edc_MesTask, Inf_MesTask
{
	public Edc_SendStatus(Inf_MesTaskAttributes i_edcMesTask, Inf_Logger i_edcLogger)
		: base(i_edcMesTask, i_edcLogger)
	{
	}

	public override Task Sub_Act()
	{
		return Task.CompletedTask;
	}
}
