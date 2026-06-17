using System.Collections.Generic;
using Ersa.Mes.FileSystem.Model.Protocol;

namespace Ersa.Mes.FileSystem.Interface;

public interface Inf_ProtocolMapper
{
	Enum_ProtocolType Pro_enuProtocol { get; }

	Edc_ProtocolWaveZevi Fun_edcMap(List<Edc_ParameterProcess> i_lstProcess, List<Edc_ParameterMeasuring> i_lstMeasuring);
}
