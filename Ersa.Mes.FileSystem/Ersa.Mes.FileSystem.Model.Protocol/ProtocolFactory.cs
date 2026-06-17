using Ersa.Mes.FileSystem.Interface;

namespace Ersa.Mes.FileSystem.Model.Protocol;

public class ProtocolFactory
{
	public static Inf_Protocol Fun_edcCreateProtocol(Enum_ProtocolType i_enuProtocolType)
	{
		Inf_Protocol a_edcProtocol = null;
		switch (i_enuProtocolType)
		{
		case Enum_ProtocolType.WaveTxt:
			a_edcProtocol = new Edc_ProtocolWaveTxt("1.2", 59);
			break;
		case Enum_ProtocolType.WaveZevi:
			a_edcProtocol = new Edc_ProtocolWaveZevi();
			break;
		}
		return a_edcProtocol;
	}
}
