using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Ersa.Mes.Common.Helper;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Mapper;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.FileSystem.Service;

namespace Ersa.Mes.FileSystem.Model.Protocol;

public class Edc_ProtocolWaveTxt : Inf_Protocol, Inf_FileService
{
	private string m_strVersion = string.Empty;

	private int Pro_i32VersionIndex = 0;

	private int Pro_i32TitleIndex = 1;

	private int Pro_i32Active = 2;

	private int Pro_i32UpperLimitIndex = 3;

	private int Pro_i32LowerLimitIndex = 4;

	private int Pro_i32SetValueIndex = 5;

	private int Pro_i32ActualValueIndex = 6;

	private char Pro_chrSeparate;

	Enum_MachineType Inf_Protocol.Pro_enuMachineType => Enum_MachineType.Welle;

	Enum_ProtocolType Inf_Protocol.Pro_enuProtocolType => Enum_ProtocolType.protocol;

	public Edc_ProtocolWaveTxt(string a_strVersion, int i_i32Separate)
	{
		m_strVersion = a_strVersion;
		Pro_chrSeparate = (char)i_i32Separate;
		Pro_i32SetValueIndex = (a_strVersion.Equals("1.2") ? 4 : 5);
		Pro_i32ActualValueIndex = (a_strVersion.Equals("1.2") ? 5 : 6);
	}

	public object Fun_dicGetData(string i_strFullpath)
	{
		string[] a_strErsasoftProtocl = File.ReadAllLines(i_strFullpath, Encoding.UTF8);
		string[] strTitles = a_strErsasoftProtocl[Pro_i32TitleIndex].Split(Pro_chrSeparate);
		string[] strValues = a_strErsasoftProtocl[Pro_i32ActualValueIndex].Split(Pro_chrSeparate);
		Dictionary<string, string> a_dic = new Dictionary<string, string>();
		for (int i = 0; i < strTitles.Length; i++)
		{
			if (!a_dic.ContainsKey(strTitles[i]))
			{
				a_dic.Add(strTitles[i], strValues[i]);
			}
		}
		return a_dic;
	}

	public Edc_ProtocolWaveZevi Fun_edcGetData(string i_strFullpath)
	{
		List<Edc_ParameterProcess> a_lstProcess = new List<Edc_ParameterProcess>();
		List<Edc_ParameterMeasuring> a_lstMeasuring = new List<Edc_ParameterMeasuring>();
		string[] a_strErsasoftProtocl = File.ReadAllLines(i_strFullpath, Encoding.UTF8);
		string[] strTitles = a_strErsasoftProtocl[Pro_i32TitleIndex].Split(Pro_chrSeparate);
		string[] strSetValues = a_strErsasoftProtocl[Pro_i32SetValueIndex].Split(Pro_chrSeparate);
		string[] strActualValues = a_strErsasoftProtocl[Pro_i32ActualValueIndex].Split(Pro_chrSeparate);
		for (int i = 0; i < strTitles.Length; i++)
		{
			int a_i32UnitBegin = strTitles[i].IndexOf('[');
			int a_i32UnitEnd = strTitles[i].IndexOf(']');
			string a_strUnit = string.Empty;
			if (a_i32UnitBegin > 0 && a_i32UnitEnd > 0)
			{
				a_strUnit = strTitles[i].Substring(a_i32UnitBegin + 1, a_i32UnitEnd - a_i32UnitBegin - 1);
			}
			if (string.IsNullOrEmpty(strSetValues[i]))
			{
				a_lstProcess.Add(new Edc_ParameterProcess
				{
					m_strName = strTitles[i],
					m_strValue = strActualValues[i]
				});
				continue;
			}
			a_lstMeasuring.Add(new Edc_ParameterMeasuring
			{
				m_strEquipment = strTitles[i],
				m_strChannel = new Edc_MeasuringChannel
				{
					m_strName = strTitles[i],
					m_strUnitOfMeasure = a_strUnit,
					m_strSample = new Edc_MeasuringSample
					{
						m_strTime = DateTimeHelper.Fun_strGetDatetimeStandard(DateTime.Now),
						m_strValue = strActualValues[i]
					},
					m_strNominalValue = new Edc_MeasuringNominalValue
					{
						m_strValue = strSetValues[i]
					}
				}
			});
		}
		Edc_XmlMapper mapper = new Edc_XmlMapper();
		return mapper.Fun_edcMap(a_lstProcess, a_lstMeasuring);
	}

	public T Fun_edcGetData<T>(string i_strFullpath) where T : class
	{
		return (T)Fun_dicGetData(i_strFullpath);
	}

	public object Fun_objConvert(object i_objData)
	{
		throw new NotImplementedException();
	}

	public bool Fun_blnUploadToDatabase(object i_objData)
	{
		throw new NotImplementedException();
	}

	public bool Fun_blnUploadToPlatform(object i_objData)
	{
		throw new NotImplementedException();
	}

	public Task<string> Fun_strGetData(string i_strFullname)
	{
		throw new NotImplementedException();
	}
}
