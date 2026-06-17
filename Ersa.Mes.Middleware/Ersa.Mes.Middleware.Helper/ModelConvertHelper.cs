using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Ersa.Mes.Middleware.Helper;

public class ModelConvertHelper<T> where T : new()
{
	public static IList<T> ConvertToModel(DataTable dt)
	{
		IList<T> ts = new List<T>();
		Type type = typeof(T);
		string tempName = "";
		foreach (DataRow dr in dt.Rows)
		{
			T t = new T();
			PropertyInfo[] propertys = t.GetType().GetProperties();
			PropertyInfo[] array = propertys;
			foreach (PropertyInfo pi in array)
			{
				tempName = pi.Name;
				if (dt.Columns.Contains(tempName) && pi.CanWrite)
				{
					object value = dr[tempName];
					if (value != DBNull.Value)
					{
						pi.SetValue(t, value, null);
					}
				}
			}
			ts.Add(t);
		}
		return ts;
	}
}
