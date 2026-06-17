using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.Trace05;

[Serializable]
public struct Struct_TcpipSettings
{
	[XmlElement("Port")]
	public int m_i32Port { get; set; }

	[XmlElement("ReceiveTimeout")]
	public int m_i32RceiveTimeout { get; set; }

	[XmlElement("SendTimeout")]
	public int m_i32SendTimeout { get; set; }

	[XmlElement("NoDelay")]
	public int m_i32NoDelay { get; set; }

	[XmlElement("EncodeSendMessage")]
	public int m_i32EncodeSendMessage { get; set; }

	[XmlElement("DecodeReceivedMessage")]
	public int m_i32DecodeReceivedMessage { get; set; }

	public void SUB_Init()
	{
		m_i32Port = 0;
		m_i32RceiveTimeout = 0;
		m_i32SendTimeout = 0;
		m_i32NoDelay = 0;
		m_i32EncodeSendMessage = 0;
		m_i32DecodeReceivedMessage = 0;
	}
}
