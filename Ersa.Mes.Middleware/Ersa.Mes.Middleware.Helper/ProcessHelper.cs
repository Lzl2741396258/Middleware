using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Ersa.Mes.Middleware.Helper;

public static class ProcessHelper
{
	public static bool Fun_blnProcessContain(string i_strProcessName)
	{
		Process[] processes = Process.GetProcesses();
		foreach (Process p in processes)
		{
			if (p.ProcessName.ToUpper().Contains(i_strProcessName))
			{
				return true;
			}
		}
		return false;
	}

	public static bool Fun_blnProcessKill(string i_strProcessName)
	{
		Process[] processes = Process.GetProcesses();
		foreach (Process p in processes)
		{
			if (p.ProcessName.ToUpper().Contains(i_strProcessName))
			{
				try
				{
					p.Kill();
					p.WaitForExit();
					return true;
				}
				catch (Win32Exception ex2)
				{
					MessageBox.Show(ex2.Message);
					return false;
				}
				catch (InvalidOperationException ex)
				{
					MessageBox.Show(ex.Message);
					return false;
				}
			}
		}
		return true;
	}
}
