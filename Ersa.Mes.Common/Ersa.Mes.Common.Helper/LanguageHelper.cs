using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Ersa.Mes.Common.Model;
using Newtonsoft.Json;

namespace Ersa.Mes.Common.Helper;

public class LanguageHelper
{
	public static string m_strLanguage;

	private static List<LanguageMode> m_lstLanguage;

	private static Dictionary<string, string> m_dicLanguage;

	static LanguageHelper()
	{
		m_strLanguage = "en";
		m_lstLanguage = new List<LanguageMode>();
		m_dicLanguage = new Dictionary<string, string>();
		try
		{
			string a_strFullname = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\Language.json";
			string a_strContent = File.ReadAllText(a_strFullname, Encoding.GetEncoding("gb2312"));
			m_lstLanguage = JsonConvert.DeserializeObject<Edc_LanguageItem>(a_strContent).Items;
		}
		catch (Exception)
		{
		}
	}

	public static void Sub_ChangeLanguage(Form form, string i_strLanguage)
	{
		CultureInfo cultureInfo = (CultureInfo.DefaultThreadCurrentUICulture = (CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(i_strLanguage)));
		m_strLanguage = i_strLanguage;
		Thread.CurrentThread.CurrentCulture = cultureInfo;
		Thread.CurrentThread.CurrentUICulture = cultureInfo;
		List<LanguageMode> list = m_lstLanguage.Where((LanguageMode s) => s.Language.Equals(i_strLanguage)).ToList();
		m_dicLanguage.Clear();
		foreach (LanguageMode item in list)
		{
			m_dicLanguage.Add(item.TextKey, item.Text);
		}
		Sub_LocalizeForm(form);
	}

	private static void Sub_LocalizeForm(Form form)
	{
		ComponentResourceManager manager = new ComponentResourceManager(form.GetType());
		Sub_ApplyControls(manager, form.Controls);
	}

	private static void Sub_ApplyControls(ComponentResourceManager manager, Control.ControlCollection controls)
	{
		foreach (Control control in controls)
		{
			if (control is ToolStrip)
			{
				Sub_ApplyToolStripItems(manager, ((ToolStrip)control).Items);
				continue;
			}
			if (control is DataGridView)
			{
				Sub_ApplyDataGridColumns(manager, ((DataGridView)control).Columns);
				continue;
			}
			Sub_ApplyControls(manager, control.Controls);
			if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
			{
				m_dicLanguage.TryGetValue(control.Tag.ToString(), out var value);
				if (!string.IsNullOrEmpty(value))
				{
					control.Text = value;
				}
			}
		}
	}

	private static void Sub_ApplyToolStripItems(ComponentResourceManager manager, ToolStripItemCollection tstripitmes)
	{
		foreach (ToolStripItem tstripitem in tstripitmes)
		{
			manager.ApplyResources(tstripitem, tstripitem.Name);
			if (tstripitem is ToolStripMenuItem)
			{
				Sub_ApplyToolStripDropDownItems(manager, ((ToolStripMenuItem)tstripitem).DropDownItems);
			}
		}
	}

	private static void Sub_ApplyToolStripDropDownItems(ComponentResourceManager manager, ToolStripItemCollection dropdownitems)
	{
		foreach (ToolStripItem dropdown in dropdownitems)
		{
			if (dropdown is ToolStripDropDownItem)
			{
				if (dropdown.Text.StartsWith("&"))
				{
					Console.WriteLine(dropdown);
				}
				manager.ApplyResources(dropdown, dropdown.Name);
				Sub_ApplyToolStripDropDownItems(manager, ((ToolStripDropDownItem)dropdown).DropDownItems);
			}
		}
	}

	private static void Sub_ApplyDataGridColumns(ComponentResourceManager manager, DataGridViewColumnCollection columns)
	{
		foreach (DataGridViewColumn column in columns)
		{
			manager.ApplyResources(column, column.Name);
		}
	}
}
