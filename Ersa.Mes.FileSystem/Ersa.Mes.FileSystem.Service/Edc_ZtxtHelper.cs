using System;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Model.Ztxt;

namespace Ersa.Mes.FileSystem.Service;

public class Edc_ZtxtHelper : Inf_Ztxt, Inf_FileService
{
	private object Fun_edcGetDataReflow(string i_strFullpath)
	{
		return new Edc_ZtxtReflow
		{
			TheTime = Edc_IniHelper.Fun_strRead("Comment", "Timedata from", "", i_strFullpath),
			ManualWaitTime = Edc_IniHelper.Fun_strRead("Times", "Waiting time MANUAL", "", i_strFullpath),
			AutoWaitTime = Edc_IniHelper.Fun_strRead("Times", "Waiting time AUTO", "", i_strFullpath),
			CongestionWaitTime = Edc_IniHelper.Fun_strRead("Times", "Waiting time congestion", "", i_strFullpath),
			ProductionWorkTime = Edc_IniHelper.Fun_strRead("Times", "Working time production", "", i_strFullpath),
			TotalWorkTime = Edc_IniHelper.Fun_strRead("Times", "Working time total", "", i_strFullpath),
			ConsumedPower = int.Parse(Edc_IniHelper.Fun_strRead("Power values", "Consumed power", "", i_strFullpath)),
			T1ProducedPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 1", "Produced PCBs", "0", i_strFullpath)),
			T1DefactivedPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 1", "Defective PCBs", "0", i_strFullpath)),
			T1TotalPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 1", "Total PCBs", "0", i_strFullpath)),
			T2ProducedPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 2", "Produced PCBs", "0", i_strFullpath)),
			T2DefactivedPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 2", "Defective PCBs", "0", i_strFullpath)),
			T2TotalPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 2", "Total PCBs", "0", i_strFullpath)),
			T3ProducedPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 3", "Produced PCBs", "0", i_strFullpath)),
			T3DefactivedPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 3", "Defective PCBs", "0", i_strFullpath)),
			T3TotalPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 3", "Total PCBs", "0", i_strFullpath)),
			T4ProducedPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 4", "Produced PCBs", "0", i_strFullpath)),
			T4DefactivedPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 4", "Defective PCBs", "0", i_strFullpath)),
			T4TotalPCB = int.Parse(Edc_IniHelper.Fun_strRead("Conveyor 4", "Total PCBs", "0", i_strFullpath))
		};
	}

	private object Fun_edcGetDataSelective(string i_strFullpath)
	{
		return new Edc_ZtxtSelective
		{
			TheTime = Edc_IniHelper.Fun_strRead("Comment", "Operating data from", "", i_strFullpath),
			m_strTotalTime = Edc_IniHelper.Fun_strRead("Times", "Total time", "", i_strFullpath),
			m_strWaitingTime = Edc_IniHelper.Fun_strRead("Times", "Waiting time while production mode", "", i_strFullpath),
			m_strMaintenanceModeTime = Edc_IniHelper.Fun_strRead("Times", "Time in maintenance mode", "", i_strFullpath),
			m_strProductionModeTime = Edc_IniHelper.Fun_strRead("Times", "Time in production mode", "", i_strFullpath),
			m_strCongestionTime = Edc_IniHelper.Fun_strRead("Times", "Waiting time congestion successor", "", i_strFullpath),
			m_i32Currently = int.Parse(Edc_IniHelper.Fun_strRead("Numbers of Boards", "Currently in machine", "0", i_strFullpath)),
			m_i32Total = int.Parse(Edc_IniHelper.Fun_strRead("Numbers of Boards", "Total boards", "0", i_strFullpath)),
			m_i32PassCount = int.Parse(Edc_IniHelper.Fun_strRead("Numbers of Boards", "Produced boards", "0", i_strFullpath)),
			m_i32SuccessedPanel = int.Parse(Edc_IniHelper.Fun_strRead("Numbers of Boards", "Produced boards (Panel)", "0", i_strFullpath)),
			m_i32FailedCount = int.Parse(Edc_IniHelper.Fun_strRead("Numbers of Boards", "Defective boards", "0", i_strFullpath)),
			m_i32OneDayPcbs = int.Parse(Edc_IniHelper.Fun_strRead("Numbers of Boards", "Piece of a day", "0", i_strFullpath)),
			m_i32Charge = int.Parse(Edc_IniHelper.Fun_strRead("Numbers of Boards", "Piece of charge", "0", i_strFullpath))
		};
	}

	private object Fun_edcGetDataPowerflow(string i_strFullpath)
	{
		return new Edc_ZtxtWave
		{
			TheTime = Edc_IniHelper.Fun_strRead("Comment", "Timedata from", "", i_strFullpath),
			m_strMaintenanceModeTime = Edc_IniHelper.Fun_strRead("Times", "Time in maintenance mode", "", i_strFullpath),
			m_strProductionModeTime = Edc_IniHelper.Fun_strRead("Times", "Time in production mode", "", i_strFullpath),
			m_strWaitingTime = Edc_IniHelper.Fun_strRead("Times", "Waiting time while production mode", "", i_strFullpath),
			m_strTotalTime = Edc_IniHelper.Fun_strRead("Times", "Total time", "", i_strFullpath),
			m_strCongestionTime = Edc_IniHelper.Fun_strRead("Times", "Waiting time congestion successor", "", i_strFullpath),
			m_i32Total = int.Parse(Edc_IniHelper.Fun_strRead("Times", "Total boards", "0", i_strFullpath)),
			m_i32Produced = int.Parse(Edc_IniHelper.Fun_strRead("Times", "Produced boards", "0", i_strFullpath)),
			m_i32Defective = int.Parse(Edc_IniHelper.Fun_strRead("Times", "Defective boards", "0", i_strFullpath)),
			m_i32Charge = int.Parse(Edc_IniHelper.Fun_strRead("Times", "Piece of charge", "0", i_strFullpath))
		};
	}

	public T Fun_edcGetData<T>(string i_strFullpath) where T : class
	{
		if (typeof(T) == typeof(Edc_ZtxtReflow))
		{
			return (T)Fun_edcGetDataReflow(i_strFullpath);
		}
		if (typeof(T) == typeof(Edc_ZtxtSelective))
		{
			return (T)Fun_edcGetDataSelective(i_strFullpath);
		}
		if (typeof(T) == typeof(Edc_ZtxtWave))
		{
			return (T)Fun_edcGetDataPowerflow(i_strFullpath);
		}
		return null;
	}

	public bool Fun_blnUploadToDatabase(object i_objData)
	{
		throw new NotImplementedException();
	}

	public bool Fun_blnUploadToPlatform(object i_objData)
	{
		throw new NotImplementedException();
	}

	public Task<string> Fun_strGetData(string i_strFullname)
	{
		throw new NotImplementedException();
	}
}
