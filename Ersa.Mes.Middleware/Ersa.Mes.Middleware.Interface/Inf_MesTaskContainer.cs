using Ersa.Mes.Middleware.Factory;

namespace Ersa.Mes.Middleware.Interface;

public interface Inf_MesTaskContainer : Inf_MesFunctionContainer
{
	bool m_blnTaskStart { get; set; }

	event Evt_ShowMessageEventHandler Evt_ShowMessage;

	event Evt_ShowOeeEventHandler Evt_ShowOEE;

	event Evt_ShowErsasoftEventHandler Evt_ShowErsasoft;

	event Evt_ShowPlcEventHandler Evt_ShowPLC;

	void Sub_Initialize();

	void Sub_EndTask();

	void Func_Test();
}
