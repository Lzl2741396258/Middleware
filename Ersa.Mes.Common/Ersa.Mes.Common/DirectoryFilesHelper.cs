using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Ersa.Mes.Logging;
using IWshRuntimeLibrary;

namespace Ersa.Mes.Common;

public class DirectoryFilesHelper
{
	private static Edc_Logger m_edcLogger = new Edc_Logger(Enum_LogLevels.All);

	private static string mC_strErsasoftIni = "ersasoft.ini";

	private const int MAX_PATH = 260;

	private const int CSIDL_COMMON_DESKTOPDIRECTORY = 25;

	public static FileSystemWatcher Fun_CreateFileWatcher(string i_strPath, string i_strFileType, bool i_blnEnable, bool i_blnInclude)
	{
		return new FileSystemWatcher
		{
			Path = i_strPath,
			Filter = i_strFileType,
			EnableRaisingEvents = i_blnEnable,
			IncludeSubdirectories = i_blnInclude,
			NotifyFilter = (NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size)
		};
	}

	public static bool Fun_blnFileInUse(string i_strFilename)
	{
		bool i_blnInUse = true;
		FileStream fs = null;
		try
		{
			fs = new FileStream(i_strFilename, FileMode.Open, FileAccess.Read, FileShare.None);
			i_blnInUse = false;
		}
		catch
		{
		}
		finally
		{
			fs?.Close();
		}
		return i_blnInUse;
	}

	public static void Sub_OpenFile(Action<string> i_delShowPath)
	{
		OpenFileDialog ofd = new OpenFileDialog();
		ofd.Title = "Please Choose The File...";
		ofd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
		if (ofd.ShowDialog() == DialogResult.OK)
		{
			string a_strPath = ofd.FileName;
			i_delShowPath(a_strPath);
		}
	}

	public static string Fun_strOpenBrowser(Action<string> i_delShowPath)
	{
		FolderBrowserDialog dialog = new FolderBrowserDialog
		{
			Description = "Please Choose The Folder..."
		};
		if (dialog.ShowDialog() == DialogResult.OK)
		{
			string a_strPath = dialog.SelectedPath;
			i_delShowPath(a_strPath);
			return a_strPath;
		}
		return string.Empty;
	}

	public static void Sub_WriteToTxt(string i_strPath, string i_strFileName, string i_strContent)
	{
		if (!Directory.Exists(i_strPath))
		{
			Directory.CreateDirectory(i_strPath);
		}
		byte[] bytes = Encoding.UTF8.GetBytes(i_strContent);
		using FileStream fs = new FileStream(Path.Combine(i_strPath, i_strFileName), FileMode.Create);
		fs.Write(bytes, 0, bytes.Length);
		fs.Close();
	}

	public static void Sub_DeleteAllFiles(string i_strPath)
	{
		if (!Directory.Exists(i_strPath))
		{
			return;
		}
		DirectoryInfo dir = new DirectoryInfo(i_strPath);
		FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();
		FileSystemInfo[] array = fileinfo;
		foreach (FileSystemInfo i in array)
		{
			if (i is DirectoryInfo)
			{
				DirectoryInfo subdir = new DirectoryInfo(i.FullName);
				subdir.Delete(recursive: true);
			}
			else
			{
				File.Delete(i.FullName);
			}
		}
	}

	public static string Fun_strGetErsasoftPath()
	{
		try
		{
			string a_strDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			string[] files = Directory.GetFiles(a_strDesktopPath);
			string[] array = files;
			foreach (string item in array)
			{
				if (item.IndexOf("ersasoft", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					WshShell shell = (WshShell)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
					IWshShortcut lnkPath = (IWshShortcut)(dynamic)shell.CreateShortcut(Path.Combine(a_strDesktopPath, item));
					return lnkPath.WorkingDirectory;
				}
			}
			a_strDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
			files = Directory.GetFiles(a_strDesktopPath);
			string[] array2 = files;
			foreach (string item2 in array2)
			{
				if (item2.IndexOf("ersasoft", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					WshShell shell2 = (WshShell)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
					IWshShortcut lnkPath2 = (IWshShortcut)(dynamic)shell2.CreateShortcut(Path.Combine(a_strDesktopPath, item2));
					return lnkPath2.WorkingDirectory;
				}
			}
		}
		catch
		{
		}
		return string.Empty;
	}

	public static string Fun_strGetErsasoftSettingFilePath(string i_strErsasoftPath)
	{
		try
		{
			string[] files = Directory.GetFiles(i_strErsasoftPath, mC_strErsasoftIni);
			if (files.Length == 0)
			{
				return string.Empty;
			}
			return files[0];
		}
		catch
		{
		}
		return string.Empty;
	}

	[DllImport("shfolder.dll", CharSet = CharSet.Auto)]
	private static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder lpszPath);

	public static string Fun_strGetAllUsersDesktopFolderPath()
	{
		StringBuilder sbPath = new StringBuilder(260);
		SHGetFolderPath(IntPtr.Zero, 25, IntPtr.Zero, 0, sbPath);
		return sbPath.ToString();
	}
}
