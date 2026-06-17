using System;

namespace Ersa.Mes.FileSystem.Model.Ztxt;

[Serializable]
public class Edc_ZtxtSelective
{
	public string TheTime { get; set; }

	public string m_strTotalTime { get; set; }

	public string m_strWaitingTime { get; set; }

	public string m_strMaintenanceModeTime { get; set; }

	public string m_strProductionModeTime { get; set; }

	public string m_strCongestionTime { get; set; }

	public int m_i32Currently { get; set; }

	public int m_i32Total { get; set; }

	public int m_i32PassCount { get; set; }

	public int m_i32SuccessedPanel { get; set; }

	public int m_i32FailedCount { get; set; }

	public int m_i32OneDayPcbs { get; set; }

	public int m_i32Charge { get; set; }
}
