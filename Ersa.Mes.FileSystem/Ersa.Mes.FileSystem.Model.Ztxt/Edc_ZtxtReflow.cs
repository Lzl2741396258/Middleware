using System;

namespace Ersa.Mes.FileSystem.Model.Ztxt;

[Serializable]
public class Edc_ZtxtReflow
{
	public string TheTime { get; set; }

	public string ManualWaitTime { get; set; }

	public string AutoWaitTime { get; set; }

	public string CongestionWaitTime { get; set; }

	public string ProductionWorkTime { get; set; }

	public string TotalWorkTime { get; set; }

	public int ConsumedPower { get; set; }

	public int T1ProducedPCB { get; set; }

	public int T1DefactivedPCB { get; set; }

	public int T1TotalPCB { get; set; }

	public int T2ProducedPCB { get; set; }

	public int T2DefactivedPCB { get; set; }

	public int T2TotalPCB { get; set; }

	public int T3ProducedPCB { get; set; }

	public int T3DefactivedPCB { get; set; }

	public int T3TotalPCB { get; set; }

	public int T4ProducedPCB { get; set; }

	public int T4DefactivedPCB { get; set; }

	public int T4TotalPCB { get; set; }
}
