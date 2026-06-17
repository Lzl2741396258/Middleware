using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ersa.Mes.Common;

public static class Edc_CsvHelper
{
	public static DataTable Fun_dtOpenCSV(string i_strPath, int i_i32DataFirstLine)
	{
		DataTable dt = new DataTable();
		FileStream fs = new FileStream(i_strPath, FileMode.Open, FileAccess.Read);
		StreamReader sr = new StreamReader(fs, Encoding.UTF8);
		string a_strLine = "";
		string[] aryLine = null;
		string[] tableHead = null;
		int a_i32RowIndex = 1;
		int a_i32ColumnCount = 0;
		while ((a_strLine = sr.ReadLine()) != null)
		{
			if (a_i32RowIndex < i_i32DataFirstLine)
			{
				a_i32RowIndex++;
			}
			else if (a_i32RowIndex == i_i32DataFirstLine)
			{
				tableHead = a_strLine.Split(';');
				a_i32ColumnCount = tableHead.Length;
				for (int i = 0; i < a_i32ColumnCount; i++)
				{
					DataColumn dc = new DataColumn("Column" + i);
					dt.Columns.Add(dc);
				}
				DataRow dr2 = dt.NewRow();
				for (int k = 0; k < a_i32ColumnCount; k++)
				{
					dr2[k] = tableHead[k];
				}
				dt.Rows.Add(dr2);
				a_i32RowIndex++;
			}
			else
			{
				aryLine = a_strLine.Split(';');
				DataRow dr = dt.NewRow();
				for (int j = 0; j < a_i32ColumnCount; j++)
				{
					dr[j] = aryLine[j];
				}
				dt.Rows.Add(dr);
			}
		}
		if (aryLine != null && aryLine.Length != 0)
		{
			dt.DefaultView.Sort = tableHead[0] + "0";
		}
		sr.Close();
		fs.Close();
		return dt;
	}

	public static DataTable Fun_dtOpenCSV(string i_strPath, int i_i32DataFirstLine, bool i_blnHead = false)
	{
		DataTable dt = new DataTable();
		using (FileStream fs = new FileStream(i_strPath, FileMode.Open, FileAccess.Read))
		{
			using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
			{
				string a_strLine = "";
				string[] aryLine = null;
				int a_i32RowIndex = 1;
				while ((a_strLine = sr.ReadLine()) != null)
				{
					if (a_i32RowIndex < i_i32DataFirstLine)
					{
						a_i32RowIndex++;
						continue;
					}
					aryLine = a_strLine.Split(';');
					if (a_i32RowIndex == i_i32DataFirstLine)
					{
						for (int i = 0; i < aryLine.Length; i++)
						{
							DataColumn dc = new DataColumn("Column" + i);
							dt.Columns.Add(dc);
						}
						DataRow dr2 = dt.NewRow();
						for (int k = 0; k < aryLine.Length; k++)
						{
							dr2[k] = aryLine[k];
						}
						dt.Rows.Add(dr2);
						a_i32RowIndex++;
					}
					else
					{
						DataRow dr = dt.NewRow();
						for (int j = 0; j < aryLine.Length; j++)
						{
							dr[j] = aryLine[j];
						}
						dt.Rows.Add(dr);
					}
				}
				sr.Close();
			}
			fs.Close();
		}
		return dt;
	}

	public static int Fun_i32GetMaxRow(string i_strPath)
	{
		int i_i32MaxRowIndex = 0;
		try
		{
			using StreamReader sr = File.OpenText(i_strPath);
			while (sr.ReadLine() != null)
			{
				i_i32MaxRowIndex++;
			}
			sr.Close();
		}
		catch
		{
		}
		return i_i32MaxRowIndex;
	}

	public static void Sub_SaveCSV(DataTable dt, string fullPath)
	{
		FileInfo fi = new FileInfo(fullPath);
		if (!fi.Directory.Exists)
		{
			fi.Directory.Create();
		}
		FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
		StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
		string data = "";
		for (int j = 0; j < dt.Columns.Count; j++)
		{
			data += dt.Columns[j].ColumnName.ToString();
			if (j < dt.Columns.Count - 1)
			{
				data += ",";
			}
		}
		sw.WriteLine(data);
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			data = "";
			for (int k = 0; k < dt.Columns.Count; k++)
			{
				string str = dt.Rows[i][k].ToString();
				str = str.Replace("\"", "\"\"");
				if (str.Contains(',') || str.Contains('"') || str.Contains('\r') || str.Contains('\n'))
				{
					str = $"\"{str}\"";
				}
				data += str;
				if (k < dt.Columns.Count - 1)
				{
					data += ",";
				}
			}
			sw.WriteLine(data);
		}
		sw.Close();
		fs.Close();
		DialogResult result = MessageBox.Show("CSV文件保存成功！");
		if (result != DialogResult.OK)
		{
		}
	}
}
