using System;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Ersa.Mes.Common.Helper;

public class IniHelper_bak
{
	public string FileName;

	[DllImport("kernel32")]
	private static extern bool m_blnWritePrivateProfileString(string section, string key, string val, string filePath);

	[DllImport("kernel32")]
	private static extern int m_i32GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

	public IniHelper_bak(string i_strFileName)
	{
		FileInfo fileInfo = new FileInfo(i_strFileName);
		if (!fileInfo.Exists)
		{
			StreamWriter sw = new StreamWriter(i_strFileName, append: false, Encoding.Default);
			try
			{
				sw.Write("#表格配置档案");
				sw.Close();
			}
			catch
			{
				throw new ApplicationException("Ini文件不存在");
			}
		}
		FileName = fileInfo.FullName;
	}

	~IniHelper_bak()
	{
		Sub_UpdateFile();
	}

	public string Fun_strRead(string Section, string Ident, string Default)
	{
		byte[] Buffer = new byte[65535];
		int bufLen = m_i32GetPrivateProfileString(Section, Ident, Default, Buffer, Buffer.GetUpperBound(0), FileName);
		string s = Encoding.GetEncoding(0).GetString(Buffer);
		s = s.Substring(0, bufLen);
		return s.Trim();
	}

	public int Fun_i32Read(string Section, string Ident, int Default)
	{
		string intStr = Fun_strRead(Section, Ident, Convert.ToString(Default));
		try
		{
			return Convert.ToInt32(intStr);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return Default;
		}
	}

	public bool Fun_blnRead(string Section, string Ident, bool Default)
	{
		try
		{
			return Convert.ToBoolean(Fun_strRead(Section, Ident, Convert.ToString(Default)));
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return Default;
		}
	}

	public void Sub_Write(string Section, string Ident, string Value)
	{
		if (!m_blnWritePrivateProfileString(Section, Ident, Value, FileName))
		{
			throw new ApplicationException("写Ini文件出错");
		}
	}

	public void Sub_Write(string Section, string Ident, int Value)
	{
		Sub_Write(Section, Ident, Value.ToString());
	}

	public void Sub_Write(string Section, string Ident, bool Value)
	{
		Sub_Write(Section, Ident, Convert.ToString(Value));
	}

	public void Sub_ReadSection(string Section, StringCollection Idents)
	{
		byte[] Buffer = new byte[16384];
		int bufLen = m_i32GetPrivateProfileString(Section, null, null, Buffer, Buffer.GetUpperBound(0), FileName);
		Sub_GetStringsFromBuffer(Buffer, bufLen, Idents);
	}

	private void Sub_GetStringsFromBuffer(byte[] Buffer, int bufLen, StringCollection Strings)
	{
		Strings.Clear();
		if (bufLen == 0)
		{
			return;
		}
		int start = 0;
		for (int i = 0; i < bufLen; i++)
		{
			if (Buffer[i] == 0 && i - start > 0)
			{
				string s = Encoding.GetEncoding(0).GetString(Buffer, start, i - start);
				Strings.Add(s);
				start = i + 1;
			}
		}
	}

	public void Sub_ReadSections(StringCollection SectionList)
	{
		byte[] Buffer = new byte[65535];
		int bufLen = 0;
		bufLen = m_i32GetPrivateProfileString(null, null, null, Buffer, Buffer.GetUpperBound(0), FileName);
		Sub_GetStringsFromBuffer(Buffer, bufLen, SectionList);
	}

	public void Sub_ReadSectionValues(string Section, NameValueCollection Values)
	{
		StringCollection KeyList = new StringCollection();
		Sub_ReadSection(Section, KeyList);
		Values.Clear();
		StringEnumerator enumerator = KeyList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				string key = enumerator.Current;
				Values.Add(key, Fun_strRead(Section, key, ""));
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
	}

	public void Sub_EraseSection(string Section)
	{
		if (!m_blnWritePrivateProfileString(Section, null, null, FileName))
		{
			throw new ApplicationException("无法清除Ini文件中的Section");
		}
	}

	public void Sub_DeleteKey(string Section, string Ident)
	{
		m_blnWritePrivateProfileString(Section, Ident, null, FileName);
	}

	public void Sub_UpdateFile()
	{
		m_blnWritePrivateProfileString(null, null, null, FileName);
	}

	public bool Fun_blnValueExists(string Section, string Ident)
	{
		StringCollection Idents = new StringCollection();
		Sub_ReadSection(Section, Idents);
		return Idents.IndexOf(Ident) > -1;
	}
}
