using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Model;
using Ersa.Mes.Middleware.Properties;

namespace Ersa.Mes.Middleware.Helper;

public static class Edc_OlvHelper
{
	private static ImageList Fun_edcGetImageList()
	{
		ImageList a_ImageList = new ImageList();
		a_ImageList.Images.Add("0", Resources.delete);
		a_ImageList.Images.SetKeyName(0, "delete");
		a_ImageList.Images.Add("1", Resources.select);
		a_ImageList.Images.SetKeyName(1, "select");
		a_ImageList.Images.Add("2", Resources.information);
		a_ImageList.Images.SetKeyName(2, "information");
		return a_ImageList;
	}

	public static ObjectListView Fun_olvMessage(ObjectListView olv)
	{
		OLVColumn ch1 = new OLVColumn("Time", "m_strTime");
		ch1.Width = 100;
		OLVColumn ch2 = new OLVColumn("LogLevel", "m_enuLogLevel");
		ch2.Width = 80;
		OLVColumn ch3 = new OLVColumn("Message", "m_strMessage");
		ch3.Width = 1000;
		olv.Columns.Add(ch1);
		olv.Columns.Add(ch2);
		olv.Columns.Add(ch3);
		olv.SmallImageList = Fun_edcGetImageList();
		olv.LargeImageList = olv.SmallImageList;
		ch2.AspectGetter = (object row) => ((OlvMessageModel)row).m_enuMessageLevel;
		ch2.ImageGetter = (object row) => ((OlvMessageModel)row).m_enuMessageLevel switch
		{
			Enum_LogType.Info => "select", 
			Enum_LogType.Error => "delete", 
			Enum_LogType.Debug => "information", 
			_ => "information", 
		};
		olv.EmptyListMsg = "No Infomation Now...";
		olv.HeaderStyle = ColumnHeaderStyle.Nonclickable;
		olv.ShowImagesOnSubItems = true;
		olv.ShowGroups = false;
		olv.FullRowSelect = true;
		olv.Scrollable = true;
		return olv;
	}

	public static ObjectListView Fun_olvCreatePlcMonitor(ObjectListView olv)
	{
		OLVColumn ch1 = new OLVColumn("Variable", "FullName");
		ch1.Width = 280;
		OLVColumn ch2 = new OLVColumn("Value", "Value");
		ch2.Width = 250;
		olv.Columns.Add(ch1);
		olv.Columns.Add(ch2);
		olv.EmptyListMsg = "No Infomation Now...";
		olv.HeaderStyle = ColumnHeaderStyle.Nonclickable;
		return olv;
	}

	public static void Sub_CreatePLCVariable(ref ObjectListView olv, string i_strTitle1, string i_strBindName, int i_uintWidth)
	{
		OLVColumn ch1 = new OLVColumn(i_strTitle1, i_strBindName);
		ch1.Width = i_uintWidth;
		olv.Columns.Add(ch1);
		olv.EmptyListMsg = "No Infomation Now...";
		olv.HeaderStyle = ColumnHeaderStyle.Nonclickable;
	}

	public static void Sub_Show(ObjectListView i_olv, Enum_LogType i_enuLogType, string i_strMessage)
	{
		OlvMessageModel model = new OlvMessageModel
		{
			m_strTime = DateTime.Now.ToString("HH:mm:ss,fff"),
			m_enuMessageLevel = i_enuLogType,
			m_strMessage = i_strMessage
		};
		if (i_olv.Items.Count > 1000)
		{
			i_olv.Items.Clear();
		}
		i_olv.AddObject(model);
		i_olv.EnsureModelVisible(model);
	}
}
