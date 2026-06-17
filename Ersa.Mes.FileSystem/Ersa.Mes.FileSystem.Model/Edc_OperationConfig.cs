using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;
using Ersa.Mes.Common;
using Ersa.Mes.Logging;

namespace Ersa.Mes.FileSystem.Model;

public static class Edc_OperationConfig
{
	private static Inf_Logger m_Logger = new Edc_Logger(Enum_LogLevels.All);

	public static T Fun_ReadConfig<T>(string i_strConfigFullname)
	{
		try
		{
			if (!File.Exists(i_strConfigFullname))
			{
				return default(T);
			}
			return i_strConfigFullname.Fun_edcDeserializeByFilePath<T>();
		}
		catch (Exception ex)
		{
			m_Logger.Error("Read config error... ConfigPath:'" + i_strConfigFullname + "'  Inner Exception:'" + ex.InnerException.Message + "'  'Message:'" + ex.Message + "'", null, "Fun_ReadConfig", 40);
		}
		return default(T);
	}

	public static bool Fun_WriteConfig<T>(string i_strPathConfig, object i_objData)
	{
		try
		{
			using (FileStream fs = new FileStream(i_strPathConfig, FileMode.Create, FileAccess.ReadWrite))
			{
				XmlSerializer xml = new XmlSerializer(typeof(T));
				xml.Serialize(fs, (T)i_objData);
				fs.Close();
			}
			return true;
		}
		catch (Exception ex)
		{
			string a_strMessage = "反射方法:'" + MethodBase.GetCurrentMethod().Name + "'  错误信息:'" + ex.Message + "'";
			m_Logger.Error("Write Config Serialization Error...Detail:'" + ex.Message + "'", null, "Fun_WriteConfig", 68);
			return false;
		}
	}

	public static bool Fun_SaveConfig<T>(string i_strConfigPath, object i_objData)
	{
		try
		{
			using (FileStream fs = new FileStream(i_strConfigPath, FileMode.Create, FileAccess.ReadWrite))
			{
				XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
				ns.Add("", "");
				XmlSerializer xml = new XmlSerializer(typeof(T));
				xml.Serialize(fs, (T)i_objData, ns);
				fs.Close();
			}
			return true;
		}
		catch (Exception ex)
		{
			m_Logger.Error("反射方法:'" + MethodBase.GetCurrentMethod().Name + "'  错误信息:'" + ex.Message + "'", null, "Fun_SaveConfig", 99);
			return false;
		}
	}

	public static T GetModel<T>(string i_strConfigPath)
	{
		using StreamReader a_StreamReader = new StreamReader(i_strConfigPath);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		return (T)xmlSerializer.Deserialize(a_StreamReader);
	}

	public static T GetModelbyJson<T>(string i_strConfigPath)
	{
		byte[] a_bytResult;
		using (StreamReader sr = new StreamReader(i_strConfigPath))
		{
			string a_strContent = sr.ReadToEnd();
			a_bytResult = Encoding.UTF8.GetBytes(a_strContent);
		}
		using MemoryStream ms = new MemoryStream(a_bytResult);
		DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
		return (T)serializer.ReadObject(ms);
	}
}
