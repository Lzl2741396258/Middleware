using System.Threading.Tasks;
using Ersa.Mes.FileSystem.Model.RequestResponse;

namespace Ersa.Mes.Middleware.Interfaces;

public interface Inf_MesTask
{
	Enum_MachineType Pro_enuMachineType { get; set; }

	void Sub_Invoke();

	void Sub_StartTask();

	void Sub_Load();

	Task Sub_Act();

	void Sub_End();
}
