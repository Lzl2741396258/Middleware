using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using log4net;

namespace Ersa.Mes.Logging;

public class Edc_Logger : Inf_Logger
{
	private Enum_LogLevels m_enuLogLevel = Enum_LogLevels.All;

	private readonly ILog logger = LogManager.GetLogger("Ersa");

	private const string m_strPath = "logs";

	Enum_LogLevels Inf_Logger.m_enuLogLevel => m_enuLogLevel;

	private Dictionary<string, Edc_AdditionLogger> m_dicNewLogger { get; set; } = new Dictionary<string, Edc_AdditionLogger>();


	public Edc_Logger(Enum_LogLevels i_enuLogLevel, bool i_blnInitialize = false, string i_strFullname1 = "", string i_strFullname2 = "", string i_strFullname3 = "", string i_strFullname4 = "", string i_strFullname5 = "")
	{
		m_enuLogLevel = i_enuLogLevel;
		if (i_blnInitialize)
		{
			Info("########################################################################");
			Info($"Ersa SH logging start... v{Assembly.GetExecutingAssembly().GetName().Version} by Ersa MesMiddleware");
			Info("########################################################################");
			Debug("########################################################################", null, ".ctor", 47);
			Debug($"Ersa SH logging start... v{Assembly.GetExecutingAssembly().GetName().Version} by Ersa MesMiddleware", null, ".ctor", 48);
			Debug("########################################################################", null, ".ctor", 49);
			if (!string.IsNullOrEmpty(i_strFullname1))
			{
				string a_strFullName3 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", i_strFullname1);
				m_dicNewLogger.Add(i_strFullname1, new Edc_AdditionLogger(a_strFullName3));
			}
			if (!string.IsNullOrEmpty(i_strFullname2))
			{
				string a_strFullName2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", i_strFullname2);
				m_dicNewLogger.Add(i_strFullname2.Split(',')[0], new Edc_AdditionLogger(a_strFullName2));
			}
			if (!string.IsNullOrEmpty(i_strFullname3))
			{
				string a_strFullName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", i_strFullname3);
				m_dicNewLogger.Add(i_strFullname3.Split(',')[0], new Edc_AdditionLogger(a_strFullName));
			}
		}
	}

	public void Debug(string i_strMessage, Exception ex = null, [CallerMemberName] string i_strFunctionName = "", [CallerLineNumber] int i_i32Line = 0)
	{
		if ((m_enuLogLevel & Enum_LogLevels.Debug) != 0)
		{
			logger.Debug($"{i_strMessage}  Function:{i_strFunctionName}  Line:{i_i32Line}");
		}
	}

	public void Error(string i_strMessage, Exception ex = null, [CallerMemberName] string i_strFunctionName = "", [CallerLineNumber] int i_i32Line = 0)
	{
		if ((m_enuLogLevel & Enum_LogLevels.Error) != 0)
		{
			logger.Error($"{i_strMessage}  Function:{i_strFunctionName}  Line:{i_i32Line}");
		}
	}

	public void Info(string i_strMessage, Exception ex = null)
	{
		if ((m_enuLogLevel & Enum_LogLevels.Info) != 0)
		{
			logger.Info(i_strMessage);
		}
	}

	public void InfoPlatform(string i_strMessage, Exception ex = null)
	{
		if ((m_enuLogLevel & Enum_LogLevels.Platform) != 0)
		{
			logger.Info(i_strMessage);
		}
	}

	public void Warn(string i_strMessage, Exception ex = null)
	{
		if ((m_enuLogLevel & Enum_LogLevels.Warn) != 0)
		{
			logger.Warn(i_strMessage);
		}
	}

	public void Addition(string i_strLoggerName, string i_strMessage)
	{
		Edc_AdditionLogger a_edcAdditionLogger = null;
		m_dicNewLogger.TryGetValue(i_strLoggerName, out a_edcAdditionLogger);
		if (!string.IsNullOrEmpty(i_strLoggerName) && a_edcAdditionLogger == null)
		{
			throw new Exception("No logs found");
		}
		a_edcAdditionLogger?.Sub_WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss,fff} - {i_strMessage}");
	}
}
