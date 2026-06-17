using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;

namespace Ersa.Mes.Common.Helper;

public class DeviceHelper
{
	public static void Sub_GetOSVersion(out string o_strOSName, out string o_strOSVersion)
	{
		try
		{
			ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * from Win32_OperatingSystem");
			string a_strCPUSerialNumber = "";
			string a_strTest = string.Empty;
			foreach (ManagementObject mo in searcher.Get())
			{
				a_strCPUSerialNumber = mo["Name"].ToString().Trim();
				a_strTest = a_strTest + "系统启动分区:" + mo["BootDevice"].ToString().Trim() + "  ";
				a_strTest = a_strTest + "当前运行的进程数:" + mo["NumberOfProcesses"].ToString().Trim() + "  ";
				a_strTest = a_strTest + "操作系统序列号:" + mo["SerialNumber"].ToString().Trim() + "  ";
				a_strTest = a_strTest + "操作系统的语言:" + mo["OSLanguage"].ToString().Trim() + "  ";
				a_strTest = a_strTest + "Manufacturer:" + mo["Manufacturer"].ToString().Trim();
			}
			o_strOSName = a_strCPUSerialNumber.Split(' ')[1];
			o_strOSVersion = a_strCPUSerialNumber.Split(' ')[2];
		}
		catch (Exception)
		{
			o_strOSName = string.Empty;
			o_strOSVersion = string.Empty;
		}
	}

	public static string Fun_strGetLocalIp()
	{
		string AddressIP = string.Empty;
		IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
		foreach (IPAddress _IPAddress in addressList)
		{
			if (_IPAddress.AddressFamily.ToString() == "InterNetwork" && !_IPAddress.ToString().Equals("129.202.101.2"))
			{
				AddressIP = _IPAddress.ToString();
			}
		}
		return AddressIP;
	}

	public static string Fun_strGetMacByIpConfig()
	{
		List<string> macs = new List<string>();
		string runCmd = ExecuteInCmd("chcp 437&&ipconfig/all");
		foreach (string line in from l in runCmd.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
			select l.Trim())
		{
			if (!string.IsNullOrEmpty(line))
			{
				if (line.StartsWith("Physical Address"))
				{
					macs.Add(line.Substring(36));
				}
				else if (line.StartsWith("DNS Servers") && line.Length > 36 && line.Substring(36).Contains("::"))
				{
					macs.Clear();
				}
				else if (macs.Count > 0 && line.StartsWith("NetBIOS") && line.Contains("Enabled"))
				{
					return macs.Last();
				}
			}
		}
		return macs.FirstOrDefault();
	}

	private static string ExecuteInCmd(string cmdline)
	{
		using Process process = new Process();
		process.StartInfo.FileName = "cmd.exe";
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.RedirectStandardInput = true;
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.RedirectStandardError = true;
		process.StartInfo.CreateNoWindow = true;
		process.Start();
		process.StandardInput.AutoFlush = true;
		process.StandardInput.WriteLine(cmdline + "&exit");
		string output = process.StandardOutput.ReadToEnd();
		process.WaitForExit();
		process.Close();
		return output;
	}
}
