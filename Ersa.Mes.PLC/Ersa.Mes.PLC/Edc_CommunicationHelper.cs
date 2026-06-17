using System.Configuration;

namespace Ersa.Mes.PLC;

public static class Edc_CommunicationHelper
{
	public static string Pro_strSpsType => ConfigurationManager.AppSettings["SpsTyp"];
}
