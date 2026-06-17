namespace Ersa.Mes.Middleware.Definition;

public struct Struct_Ztxt
{
	public string TimedataFrom { get; set; }

	public double m_dblWaitingTimeManual { get; set; }

	public double m_dblWorkingTimeProduction { get; set; }

	public double m_dblWaitingTimeAUTO { get; set; }

	public double m_dblWorkingTimeTotal { get; set; }

	public double m_dblWaitingTimeCongestion { get; set; }

	public string ConsumedPower { get; set; }

	public int ProducedPCBs { get; set; }

	public int DefectivePCBs { get; set; }

	public int TotalPCBs { get; set; }

	public void SUB_Init()
	{
		TimedataFrom = string.Empty;
		m_dblWaitingTimeManual = 0.0;
		m_dblWorkingTimeProduction = 0.0;
		m_dblWaitingTimeAUTO = 0.0;
		m_dblWorkingTimeTotal = 0.0;
		m_dblWaitingTimeCongestion = 0.0;
		ConsumedPower = string.Empty;
		ProducedPCBs = 0;
		DefectivePCBs = 0;
		TotalPCBs = 0;
	}
}
