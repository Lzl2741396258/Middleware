using System;
using BR.AN.PviServices;

namespace Ersa.Mes.PLC.Modules;

public class ComboboxContorl
{
	public static string[] Sub_GetDeviceType()
	{
		return Enum.GetNames(typeof(DeviceType));
	}
}
