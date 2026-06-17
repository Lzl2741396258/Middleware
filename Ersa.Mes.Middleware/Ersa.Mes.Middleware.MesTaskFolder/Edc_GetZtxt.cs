using System;
using System.IO;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.FileSystem.Model.Ztxt;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Factory;
using Ersa.Mes.Middleware.Interfaces;

namespace Ersa.Mes.Middleware.MesTaskFolder;

public class Edc_GetZtxt : Edc_MesTask, Inf_MesTask
{
	public string m_strPath { get; set; }

	public string m_strFullName { get; set; }

	public Edc_ZtxtReflow m_edcZtxt { get; set; }

	public Edc_GetZtxt(Enum_MachineType i_enuMachineType, string i_strFullName, Inf_MesTaskAttributes i_edcMesTask, Inf_Logger i_edcLogger)
		: base(i_edcMesTask, i_edcLogger)
	{
		m_strFullName = i_strFullName;
		m_strPath = new FileInfo(m_strFullName).DirectoryName;
	}

	public override Task Sub_Act()
	{
		return Task.CompletedTask;
	}

	protected void OnChanged(object sender, FileSystemEventArgs e)
	{
		try
		{
			Edc_ZtxtReflow ztxt = new Edc_ZtxtReflow
			{
				TheTime = Edc_IniHelper.Fun_strRead("Comment", "Timedata from", "", m_strFullName),
				ManualWaitTime = Edc_IniHelper.Fun_strRead("Times", "Waiting time MANUAL", "", m_strFullName),
				AutoWaitTime = Edc_IniHelper.Fun_strRead("Times", "Waiting time AUTO", "", m_strFullName),
				CongestionWaitTime = Edc_IniHelper.Fun_strRead("Times", "Waiting time congestion", "", m_strFullName),
				ProductionWorkTime = Edc_IniHelper.Fun_strRead("Times", "Working time production", "", m_strFullName),
				TotalWorkTime = Edc_IniHelper.Fun_strRead("Times", "Working time total", "", m_strFullName),
				ConsumedPower = int.Parse(Edc_IniHelper.Fun_strRead("Power values", "Consumed power", "", m_strFullName)),
				T1ProducedPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 1", "Produced PCBs", "0", m_strFullName)),
				T1DefactivedPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 1", "Defective PCBs", "0", m_strFullName)),
				T1TotalPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 1", "Total PCBs", "0", m_strFullName)),
				T2ProducedPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 2", "Produced PCBs", "0", m_strFullName)),
				T2DefactivedPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 2", "Defective PCBs", "0", m_strFullName)),
				T2TotalPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 2", "Total PCBs", "0", m_strFullName)),
				T3ProducedPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 3", "Produced PCBs", "0", m_strFullName)),
				T3DefactivedPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 3", "Defective PCBs", "0", m_strFullName)),
				T3TotalPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 3", "Total PCBs", "0", m_strFullName)),
				T4ProducedPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 4", "Produced PCBs", "0", m_strFullName)),
				T4DefactivedPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 4", "Defective PCBs", "0", m_strFullName)),
				T4TotalPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 4", "Total PCBs", "0", m_strFullName))
			};
			m_edcZtxt = ztxt;
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "Create Ztxt Model Error....Details:'" + ex.Message + "'");
		}
	}
}
