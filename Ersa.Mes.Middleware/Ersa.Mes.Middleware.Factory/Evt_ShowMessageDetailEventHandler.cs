using Ersa.Mes.Logging;

namespace Ersa.Mes.Middleware.Factory;

public delegate void Evt_ShowMessageDetailEventHandler(Enum_LogType i_enuLogType, string i_strMessage, string i_strFunctionName = "", int i_i32Line = -1);
