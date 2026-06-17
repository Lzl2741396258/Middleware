using System;
using System.Reflection;
using System.Xml.Serialization;
using Ersa.Mes.Common;
using Ersa.Mes.Logging;

namespace Ersa.Mes.FileSystem.Model.Config.Trace05;

public class Edc_ConfigTrace05
{
	private Inf_Logger m_Loging = new Edc_Logger(Enum_LogLevels.All);

	private void GetTrace05()
	{
		try
		{
			string a_strPathAndFilename = string.Empty;
			m_Loging.Info(MethodBase.GetCurrentMethod().Name + "  获得ConfigTrace05全路径+文件名  " + a_strPathAndFilename);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Struct_ConfigTrace05));
			Struct_ConfigTrace05 a_sttConfigTrace5 = a_strPathAndFilename.Fun_DeserializeContent<Struct_ConfigTrace05>();
		}
		catch (Exception ex)
		{
			m_Loging.Error(MethodBase.GetCurrentMethod().Name + "  " + ex.Message, null, "GetTrace05", 34);
		}
	}
}
