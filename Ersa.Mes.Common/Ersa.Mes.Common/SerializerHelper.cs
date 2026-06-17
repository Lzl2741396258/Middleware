using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.Common;

public static class SerializerHelper
{
	public static string Fun_strSerializerModel<T>(object i_objResponse, XmlSerializerNamespaces i_edcNamespaces = null)
	{
		using MemoryStream ms = new MemoryStream();
		XmlSerializerNamespaces _namespaces;
		if (i_edcNamespaces == null)
		{
			_namespaces = new XmlSerializerNamespaces();
			_namespaces.Add("", "");
		}
		else
		{
			_namespaces = i_edcNamespaces;
		}
		XmlSerializer a_XmlSerializer = new XmlSerializer(typeof(T));
		a_XmlSerializer.Serialize(ms, i_objResponse, _namespaces);
		ms.Position = 0L;
		byte[] array = new byte[(int)ms.Length];
		ms.Read(array, 0, (int)ms.Length);
		return Encoding.UTF8.GetString(array);
	}

	public static string Fun_strSerializerModel_XmlWriter(object i_objResponse, XmlSerializerNamespaces i_edcNamespaces)
	{
		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.OmitXmlDeclaration = false;
		settings.Encoding = Encoding.UTF8;
		using MemoryStream ms = new MemoryStream();
		using XmlWriter writer = XmlWriter.Create(ms, settings);
		XmlSerializer xs = new XmlSerializer(i_objResponse.GetType());
		xs.Serialize(writer, i_objResponse, i_edcNamespaces);
		return Encoding.UTF8.GetString(ms.ToArray());
	}

	public static T Fun_DeserializeContent<T>(this string i_strXmlContent)
	{
		i_strXmlContent = i_strXmlContent.Replace("&", "&amp;");
		using StringReader sr = new StringReader(i_strXmlContent);
		XmlSerializer a_XmlSerializer = new XmlSerializer(typeof(T));
		return (T)a_XmlSerializer.Deserialize(sr);
	}

	public static T Fun_edcDeserializeByFilePath<T>(this string i_strXmlPath)
	{
		if (string.IsNullOrEmpty(i_strXmlPath))
		{
			return default(T);
		}
		FileInfo fileInfo = new FileInfo(i_strXmlPath);
		using StreamReader streamReader = new StreamReader(fileInfo.FullName);
		string a_strContent = streamReader.ReadToEnd();
		return a_strContent.Fun_DeserializeContent<T>();
	}

	public static string Fun_strPrepareForDeserialize<T>(this string instance)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(instance);
		foreach (XmlElement item in xmlDocument.OfType<XmlElement>())
		{
			string xmlRootAttributeNameSpace = typeof(T).Fun_strGetXmlRootAttributeNameSpace();
			if (string.IsNullOrEmpty(item.NamespaceURI) && xmlRootAttributeNameSpace != null)
			{
				item.SetAttribute("xmlns", xmlRootAttributeNameSpace);
			}
			string xmlTypeAttributeNameSpace = typeof(T).Fun_strGetXmlTypeAttributeNameSpace();
			foreach (XmlElement item2 in item.ChildNodes.OfType<XmlElement>())
			{
				if (string.IsNullOrEmpty(item2.NamespaceURI) && xmlTypeAttributeNameSpace != null)
				{
					item2.SetAttribute("xmlns", xmlTypeAttributeNameSpace);
				}
			}
		}
		return xmlDocument.InnerXml;
	}

	public static string Fun_strGetXmlRootAttributeNameSpace(this Type instance)
	{
		CustomAttributeData customAttributeData = instance.GetCustomAttributesData().FirstOrDefault((CustomAttributeData a) => a.AttributeType == typeof(XmlRootAttribute));
		if (customAttributeData != null && customAttributeData.NamedArguments != null)
		{
			CustomAttributeNamedArgument customAttributeNamedArgument = customAttributeData.NamedArguments.FirstOrDefault((CustomAttributeNamedArgument a) => a.MemberName == "Namespace" && a.TypedValue.Value != null);
			if (string.IsNullOrEmpty(customAttributeNamedArgument.TypedValue.Value.ToString()))
			{
				return null;
			}
			return customAttributeNamedArgument.TypedValue.Value.ToString();
		}
		return null;
	}

	public static string Fun_strGetXmlTypeAttributeNameSpace(this Type instance)
	{
		CustomAttributeData customAttributeData = instance.GetCustomAttributesData().FirstOrDefault((CustomAttributeData a) => a.AttributeType == typeof(XmlTypeAttribute));
		if (customAttributeData != null && customAttributeData.NamedArguments != null)
		{
			CustomAttributeNamedArgument customAttributeNamedArgument = customAttributeData.NamedArguments.FirstOrDefault((CustomAttributeNamedArgument a) => a.MemberName == "Namespace" && a.TypedValue.Value != null);
			if (string.IsNullOrEmpty(customAttributeNamedArgument.TypedValue.Value.ToString()))
			{
				return null;
			}
			return customAttributeNamedArgument.TypedValue.Value.ToString();
		}
		return null;
	}

	public static string Fun_strSerializerJson<T>(object obj)
	{
		return JsonConvert.SerializeObject(obj);
	}

	public static T Fun_DeserializeJson<T>(string i_strXmlPath)
	{
		if (string.IsNullOrEmpty(i_strXmlPath))
		{
			return default(T);
		}
		FileInfo fileInfo = new FileInfo(i_strXmlPath);
		using StreamReader streamReader = new StreamReader(fileInfo.FullName);
		string a_strContent = streamReader.ReadToEnd();
		return JsonConvert.DeserializeObject<T>(a_strContent);
	}
}
