using System;
using System.Runtime.CompilerServices;

namespace Ersa.Mes.Logging;

public interface Inf_Logger
{
	Enum_LogLevels m_enuLogLevel { get; }

	void Debug(string i_strMessage, Exception ex = null, [CallerMemberName] string i_strFunctionName = "", [CallerLineNumber] int i_i32Line = 0);

	void Error(string i_strMessage, Exception ex = null, [CallerMemberName] string i_strFunctionName = "", [CallerLineNumber] int i_i32Line = 0);

	void Info(string i_strMessage, Exception ex = null);

	void Warn(string i_strMessage, Exception ex = null);

	void InfoPlatform(string i_strMessage, Exception ex = null);

	void Addition(string i_strLoggerName, string i_strMessage);
}
