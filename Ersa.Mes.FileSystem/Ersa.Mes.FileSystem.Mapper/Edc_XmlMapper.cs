using System.Collections.Generic;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Model.Protocol;

namespace Ersa.Mes.FileSystem.Mapper;

public class Edc_XmlMapper : Inf_ProtocolMapper
{
	public Enum_ProtocolType Pro_enuProtocol => Enum_ProtocolType.XmlZevi;

	public Edc_ProtocolWaveZevi Fun_edcMap(List<Edc_ParameterProcess> i_lstProcess, List<Edc_ParameterMeasuring> i_lstMeasuring)
	{
		return new Edc_ProtocolWaveZevi
		{
			ma_strProcessingParameters = i_lstProcess.ToArray(),
			ma_sttMeasuring = i_lstMeasuring.ToArray()
		};
	}
}
