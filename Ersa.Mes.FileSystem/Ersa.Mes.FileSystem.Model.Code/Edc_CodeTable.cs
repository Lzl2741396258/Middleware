using System.Collections.Generic;
using System.IO;

namespace Ersa.Mes.FileSystem.Model.Code;

public class Edc_CodeTable
{
	private static Dictionary<string, string> a_dicCodeTable = new Dictionary<string, string>();

	public string FileName { get; set; }

	public static Dictionary<string, string> Fun_dicGetData(string i_strFilenameCodeTable)
	{
		using (File.OpenRead(i_strFilenameCodeTable))
		{
			foreach (string item in File.ReadLines(i_strFilenameCodeTable))
			{
				Sub_GetValue(item, out var o_strTable, out var o_strLibrary, out var o_strProgram);
				if (!a_dicCodeTable.TryGetValue(o_strTable, out var _))
				{
					a_dicCodeTable.Add(o_strTable, o_strLibrary + ";" + o_strProgram);
				}
			}
		}
		return a_dicCodeTable;
	}

	private static void Sub_GetValue(string i_strDataLine, out string o_strTable, out string o_strLibrary, out string o_strProgram)
	{
		o_strTable = string.Empty;
		o_strLibrary = string.Empty;
		o_strProgram = string.Empty;
		try
		{
			string[] strs = i_strDataLine.Split(';');
			o_strTable = strs[0];
			o_strLibrary = strs[1];
			o_strProgram = strs[2];
		}
		catch
		{
		}
	}
}
