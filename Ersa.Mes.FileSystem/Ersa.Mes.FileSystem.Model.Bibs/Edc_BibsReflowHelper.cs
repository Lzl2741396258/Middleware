using System;
using System.IO;
using System.Xml;
using Ersa.Mes.Common;

namespace Ersa.Mes.FileSystem.Model.Bibs;

public class Edc_BibsReflowHelper
{
	public static Edc_BibsReflow Fun_edcGetBibReflow(string i_strFullname)
	{
		using StreamReader streamReader = new StreamReader(i_strFullname);
		string a_strContent = streamReader.ReadToEnd().Replace(',', '.');
		return a_strContent.Fun_DeserializeContent<Edc_BibsReflow>();
	}

	public static XmlDocument Fun_edcXmlRepairEmpty(string i_strFullname)
	{
		XmlDocument doc = new XmlDocument();
		doc.Load(i_strFullname);
		foreach (XmlNode node1 in doc.DocumentElement.ChildNodes)
		{
			foreach (XmlNode node2 in node1.ChildNodes)
			{
				if (string.IsNullOrEmpty(node2.InnerText))
				{
					node2.InnerText = "0";
				}
			}
		}
		doc.Save(i_strFullname);
		return doc;
	}

	public static void Fun_strGetSplitValue(string i_strInfo, out string o_strPCBLength, out string o_strPCBWidth, out string o_strPCBThickness, out string o_strPCBSide)
	{
		o_strPCBLength = string.Empty;
		o_strPCBWidth = string.Empty;
		o_strPCBThickness = string.Empty;
		o_strPCBSide = string.Empty;
		try
		{
			string[] a_strInfo = i_strInfo.Split(new string[1] { "\n" }, StringSplitOptions.None);
			o_strPCBLength = a_strInfo[0].Split('=')[1];
			o_strPCBWidth = a_strInfo[1].Split('=')[1];
			o_strPCBThickness = a_strInfo[2].Split('=')[1];
			o_strPCBSide = a_strInfo[3].Split('=')[1];
		}
		catch
		{
		}
	}

	public static void Sub_ReadXmlNodeHistory(XmlDocument r_objXmlDoc, Edc_BibsReflow i_edcBibsReflow)
	{
		int num = i_edcBibsReflow.m_edcHistory.m_i32Items;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				string i_strName = $"SOLDER_PRG / History / Item_{i}";
				XmlNode node = r_objXmlDoc.SelectSingleNode(i_strName);
				i_edcBibsReflow.m_edcHistory.ma_strHistory.Add(node.InnerText);
			}
		}
	}
}
