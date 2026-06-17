using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using IWshRuntimeLibrary;

namespace Ersa.Mes.Common.Helper;

public class AutoStart
{
	private const string Pro_QuickName = "Ersa Update";

	private string m_strPathSystemStart => Environment.GetFolderPath(Environment.SpecialFolder.Startup);

	private string m_strPathApp => Process.GetCurrentProcess().MainModule.FileName;

	private string m_strDesktopPath => Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

	public void Sub_SetAutoStart(bool i_blnActive = true)
	{
		if (i_blnActive)
		{
			List<string> a_lstPathShortcut = Fun_GetQuickFromFolder(m_strPathSystemStart, m_strPathApp);
			if (a_lstPathShortcut.Count >= 2)
			{
				for (int j = 1; j < a_lstPathShortcut.Count; j++)
				{
					Sub_DeleteFile(a_lstPathShortcut[j]);
				}
			}
			else if (a_lstPathShortcut.Count < 1)
			{
				Fun_CreateShortcut(m_strPathSystemStart, "Ersa Update", m_strPathApp, "Ersa Update");
			}
			return;
		}
		List<string> shortcutPaths = Fun_GetQuickFromFolder(m_strPathSystemStart, m_strPathApp);
		if (shortcutPaths.Count > 0)
		{
			for (int i = 0; i < shortcutPaths.Count; i++)
			{
				Sub_DeleteFile(shortcutPaths[i]);
			}
		}
	}

	private bool Fun_CreateShortcut(string directory, string shortcutName, string targetPath, string description = null, string iconLocation = null)
	{
		try
		{
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
			string shortcutPath = Path.Combine(directory, $"{shortcutName}.lnk");
			WshShell shell = (WshShell)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
			IWshShortcut shortcut = (IWshShortcut)(dynamic)shell.CreateShortcut(shortcutPath);
			shortcut.TargetPath = targetPath;
			shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
			shortcut.WindowStyle = 1;
			shortcut.Description = description;
			shortcut.IconLocation = (string.IsNullOrWhiteSpace(iconLocation) ? targetPath : iconLocation);
			shortcut.Save();
			return true;
		}
		catch (Exception ex)
		{
			string temp = ex.Message;
			temp = "";
		}
		return false;
	}

	private List<string> Fun_GetQuickFromFolder(string i_strDirectory, string i_strFullname)
	{
		List<string> a_lstTemp = new List<string>();
		string a_strTemp = string.Empty;
		string[] files = Directory.GetFiles(i_strDirectory, "*.lnk");
		if (files == null || files.Length < 1)
		{
			return a_lstTemp;
		}
		for (int i = 0; i < files.Length; i++)
		{
			a_strTemp = Fun_GetAppPathFromQuick(files[i]);
			if (a_strTemp == i_strFullname)
			{
				a_lstTemp.Add(files[i]);
			}
		}
		return a_lstTemp;
	}

	private string Fun_GetAppPathFromQuick(string i_strPathShortcut)
	{
		if (File.Exists(i_strPathShortcut))
		{
			WshShell shell = (WshShell)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
			IWshShortcut shortct = (IWshShortcut)(dynamic)shell.CreateShortcut(i_strPathShortcut);
			return shortct.TargetPath;
		}
		return "";
	}

	private void Sub_DeleteFile(string path)
	{
		FileAttributes attr = File.GetAttributes(path);
		if (attr == FileAttributes.Directory)
		{
			Directory.Delete(path, recursive: true);
		}
		else
		{
			File.Delete(path);
		}
	}

	public void Sub_CreateDesktopQuick(string desktopPath = "", string quickName = "", string appPath = "")
	{
		List<string> shortcutPaths = Fun_GetQuickFromFolder(desktopPath, appPath);
		if (shortcutPaths.Count < 1)
		{
			Fun_CreateShortcut(desktopPath, quickName, appPath, "Description of Ersa Update");
		}
	}
}
