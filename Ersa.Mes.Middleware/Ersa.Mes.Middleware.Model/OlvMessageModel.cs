using Ersa.Mes.Logging;

namespace Ersa.Mes.Middleware.Model;

public class OlvMessageModel
{
	public string m_strTime { get; set; }

	public Enum_LogType m_enuMessageLevel { get; set; }

	public string m_strMessage { get; set; }

	public OlvMessageModel()
	{
		m_strTime = "00:00:00,000";
		m_enuMessageLevel = Enum_LogType.Info;
		m_strMessage = string.Empty;
	}
}
