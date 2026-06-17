using System;

namespace Ersa.Mes.FileSystem.Model.Ztxt;

[Serializable]
public class Edc_ZtxtWave
{
	public string TheTime { get; set; }

	public string m_strMaintenanceModeTime { get; set; }

	public string m_strProductionModeTime { get; set; }

	public string m_strWaitingTime { get; set; }

	public string m_strTotalTime { get; set; }

	public string m_strCongestionTime { get; set; }

	public int m_i32Total { get; set; }

	public int m_i32Produced { get; set; }

	public int m_i32Defective { get; set; }

	public int m_i32Charge { get; set; }
}
