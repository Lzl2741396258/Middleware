using System.IO;
using System.Text;
using System.Xml;

namespace Ersa.Mes.Common.Helper;

public static class XmlHelper
{
	public static string Fun_strConvertXmlToString(XmlDocument doc)
	{
		string a_strXml = string.Empty;
		using (MemoryStream stream = new MemoryStream())
		{
			XmlTextWriter writer = new XmlTextWriter(stream, null);
			writer.Formatting = Formatting.Indented;
			doc.Save(writer);
			using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
			{
				stream.Position = 0L;
				a_strXml = sr.ReadToEnd();
				sr.Close();
			}
			stream.Close();
		}
		return a_strXml;
	}
}
