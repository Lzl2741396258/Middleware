using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ersa.Mes.Logging;

public class Edc_AdditionLogger
{
	private static Dictionary<long, long> m_dicLock = new Dictionary<long, long>();

	public string m_strPath { get; set; }

	public Edc_AdditionLogger(string a_strPath)
	{
		m_strPath = a_strPath;
	}

	public void Sub_Create(string i_strFileName)
	{
		if (!File.Exists(i_strFileName))
		{
			using (FileStream fs = File.Create(i_strFileName))
			{
				fs.Close();
			}
		}
	}

	private void Sub_Write(string i_strContent, string i_strNewLine)
	{
		if (string.IsNullOrEmpty(m_strPath))
		{
			throw new Exception("路径不能为空！");
		}
		if (!Directory.Exists(m_strPath))
		{
			Directory.CreateDirectory(m_strPath);
		}
		string a_strFilename = Path.Combine(m_strPath, DateTime.Now.ToString("yyyyMMdd") + ".log");
		Sub_Create(a_strFilename);
		using FileStream fs = new FileStream(a_strFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 8, FileOptions.Asynchronous);
		byte[] a_bytData = Encoding.UTF8.GetBytes(i_strContent + i_strNewLine);
		bool flag = true;
		long slen = a_bytData.Length;
		long len = 0L;
		while (flag)
		{
			try
			{
				if (len >= fs.Length)
				{
					fs.Lock(len, slen);
					m_dicLock[len] = slen;
					flag = false;
				}
				else
				{
					len = fs.Length;
				}
			}
			catch (Exception)
			{
				for (; !m_dicLock.ContainsKey(len); len += m_dicLock[len])
				{
				}
			}
		}
		fs.Seek(len, SeekOrigin.Begin);
		fs.Write(a_bytData, 0, a_bytData.Length);
		fs.Close();
	}

	public void Sub_WriteLine(string i_strContent)
	{
		Sub_Write(i_strContent, Environment.NewLine);
	}

	public void Sub_Write(string i_strContent)
	{
		Sub_Write(i_strContent, "");
	}
}
