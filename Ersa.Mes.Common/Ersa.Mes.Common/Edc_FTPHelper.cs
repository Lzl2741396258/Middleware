using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Ersa.Mes.Common;

public static class Edc_FTPHelper
{
	public static void Ftp_UpLoad(string i_strFilePath, string i_strLocalName, string i_strFtpAddress, string i_strFtpName, string i_strUser, string i_strPassword)
	{
		if (!i_strFilePath.EndsWith("\\"))
		{
			i_strFilePath += "\\";
		}
		if (!i_strFtpAddress.EndsWith("/"))
		{
			i_strFtpAddress += "/";
		}
		if (string.IsNullOrWhiteSpace(i_strFtpName))
		{
			i_strFtpName = i_strLocalName;
		}
		Ftp_UpLoad(i_strFilePath + i_strLocalName, i_strFtpAddress + i_strFtpName, i_strUser, i_strPassword);
	}

	public static void Ftp_UpLoad(string fileFullName, string ftpFullName, string user, string passwd, bool isftps = false)
	{
		FileInfo fi = new FileInfo(fileFullName);
		Uri serverUri = new Uri(ftpFullName);
		if (serverUri.ToString().Contains("#"))
		{
			serverUri = new Uri(serverUri.ToString().Replace("#", Uri.HexEscape('#')));
		}
		FtpWebRequest FtpRequest = (FtpWebRequest)WebRequest.Create(serverUri);
		FtpRequest.Method = "STOR";
		FtpRequest.Credentials = new NetworkCredential(user, passwd);
		FtpRequest.KeepAlive = false;
		FtpRequest.Proxy = null;
		FtpRequest.UseBinary = true;
		FtpRequest.ContentLength = fi.Length;
		FtpRequest.EnableSsl = isftps;
		using Stream requestStream = FtpRequest.GetRequestStream();
		using FileStream fileStream = fi.OpenRead();
		byte[] buffer = new byte[2048];
		for (int contentLen = fileStream.Read(buffer, 0, 2048); contentLen != 0; contentLen = fileStream.Read(buffer, 0, contentLen))
		{
			requestStream.Write(buffer, 0, contentLen);
		}
	}

	public static List<string> Ftp_GetDirctory(string addr, string user, string passwd, bool isFtps = false)
	{
		List<string> strs = new List<string>();
		FtpWebRequest FtpRequest = (FtpWebRequest)WebRequest.Create(addr);
		FtpRequest.Method = "LIST";
		FtpRequest.Credentials = new NetworkCredential(user, passwd);
		FtpRequest.KeepAlive = false;
		FtpRequest.EnableSsl = isFtps;
		using (FtpWebResponse listResponse = (FtpWebResponse)FtpRequest.GetResponse())
		{
			using StreamReader reader = new StreamReader(listResponse.GetResponseStream());
			for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
			{
				if (line.Contains("<DIR>"))
				{
					string msg = line.Substring(line.LastIndexOf("<DIR>") + 5).Trim();
					strs.Add(msg);
				}
			}
		}
		return strs;
	}

	public static List<string> Ftp_GetFiles(string addr, string user, string passwd, bool FtpAttrib = false, bool isFtps = false)
	{
		List<string> strs = new List<string>();
		FtpWebRequest FtpRequest = (FtpWebRequest)WebRequest.Create(addr);
		FtpRequest.Method = "LIST";
		FtpRequest.Credentials = new NetworkCredential(user, passwd);
		FtpRequest.KeepAlive = false;
		FtpRequest.EnableSsl = isFtps;
		using (FtpWebResponse listResponse = (FtpWebResponse)FtpRequest.GetResponse())
		{
			using StreamReader reader = new StreamReader(listResponse.GetResponseStream());
			for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
			{
				if (!line.Contains("<DIR>"))
				{
					if (FtpAttrib && line.Contains("1 ftp ftp"))
					{
						string msg3 = line.Substring(49).Trim();
						strs.Add(msg3);
					}
					else if (FtpAttrib && line.Contains("1 user     group"))
					{
						string msg2 = line.Substring(55).Trim();
						if (msg2.Length > 4)
						{
							strs.Add(msg2);
						}
					}
					else
					{
						string msg = line.Substring(39).Trim();
						strs.Add(msg);
					}
				}
			}
		}
		return strs;
	}

	public static Dictionary<string, long> Ftp_GetFilesAndSize(string addr, string user, string passwd)
	{
		Dictionary<string, long> dicFileAndSize = new Dictionary<string, long>();
		FtpWebRequest FtpRequest = (FtpWebRequest)WebRequest.Create(addr);
		FtpRequest.Method = "LIST";
		FtpRequest.Credentials = new NetworkCredential(user, passwd);
		FtpRequest.KeepAlive = false;
		using (FtpWebResponse listResponse = (FtpWebResponse)FtpRequest.GetResponse())
		{
			using StreamReader reader = new StreamReader(listResponse.GetResponseStream());
			for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
			{
				if (!line.Contains("<DIR>"))
				{
					string msg = line.Substring(39).Trim();
					dicFileAndSize.Add(msg, Convert.ToInt64(line.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2]));
				}
			}
		}
		return dicFileAndSize;
	}

	public static void Ftp_CreateDir(string addr, string user, string passwd)
	{
		FtpWebRequest FtpRequest = (FtpWebRequest)WebRequest.Create(addr);
		FtpRequest.Method = "MKD";
		FtpRequest.Credentials = new NetworkCredential(user, passwd);
		FtpRequest.KeepAlive = false;
		using ((FtpWebResponse)FtpRequest.GetResponse())
		{
		}
	}

	public static void Ftp_DeleteFile(string addr, string user, string passwd, bool isFtps = false)
	{
		FtpWebRequest FtpRequest = (FtpWebRequest)WebRequest.Create(addr);
		FtpRequest.Method = "DELE";
		FtpRequest.Credentials = new NetworkCredential(user, passwd);
		FtpRequest.KeepAlive = false;
		FtpRequest.EnableSsl = isFtps;
		using ((FtpWebResponse)FtpRequest.GetResponse())
		{
		}
	}

	public static void Ftp_DeleteFile(string addr, string filename, string user, string passwd, bool isFtps = false)
	{
		if (!addr.EndsWith("/"))
		{
			addr += "/";
		}
		Ftp_DeleteFile(addr + filename, user, passwd, isFtps);
	}

	public static void Ftp_DownLoad(string filePath, string fName, string address, string user, string passwd, bool isFtps = false)
	{
		if (!filePath.EndsWith("\\"))
		{
			filePath += "\\";
		}
		if (!address.EndsWith("/"))
		{
			address += "/";
		}
		FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(address + fName);
		reqFTP.Method = "RETR";
		reqFTP.KeepAlive = false;
		reqFTP.UseBinary = true;
		reqFTP.Credentials = new NetworkCredential(user, passwd);
		reqFTP.EnableSsl = isFtps;
		using FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
		using Stream ftpStream = response.GetResponseStream();
		int bufferSize = 2048;
		byte[] buffer = new byte[bufferSize];
		using FileStream outputStream = new FileStream(filePath + fName, FileMode.Create);
		for (int readCount = ftpStream.Read(buffer, 0, bufferSize); readCount > 0; readCount = ftpStream.Read(buffer, 0, bufferSize))
		{
			outputStream.Write(buffer, 0, readCount);
		}
	}

	public static MemoryStream Ftp_DownLoad_Stream(string fName, string address, string user, string passwd, bool isFtps = false)
	{
		MemoryStream ms = new MemoryStream();
		if (!address.EndsWith("/"))
		{
			address += "/";
		}
		FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(address + fName);
		reqFTP.Method = "RETR";
		reqFTP.KeepAlive = false;
		reqFTP.UseBinary = true;
		reqFTP.Credentials = new NetworkCredential(user, passwd);
		reqFTP.EnableSsl = isFtps;
		using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
		{
			using Stream ftpStream = response.GetResponseStream();
			int bufferSize = 2048;
			byte[] buffer = new byte[bufferSize];
			for (int readCount = ftpStream.Read(buffer, 0, bufferSize); readCount > 0; readCount = ftpStream.Read(buffer, 0, bufferSize))
			{
				ms.Write(buffer, 0, readCount);
			}
		}
		return ms;
	}

	public static void Ftp_Rename(string oldFilename, string newFilename, string address, string user, string passwd)
	{
		if (!address.EndsWith("/"))
		{
			address += "/";
		}
		FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(address + oldFilename);
		reqFTP.Method = "RENAME";
		reqFTP.RenameTo = newFilename;
		reqFTP.KeepAlive = false;
		reqFTP.UseBinary = true;
		reqFTP.Credentials = new NetworkCredential(user, passwd);
		using ((FtpWebResponse)reqFTP.GetResponse())
		{
		}
	}

	public static bool Ftp_FileExists(string addr, string fileName, string user, string passwd)
	{
		List<string> lst = Ftp_GetFiles(addr, user, passwd);
		foreach (string s in lst)
		{
			if (s == fileName)
			{
				return true;
			}
		}
		return false;
	}
}
