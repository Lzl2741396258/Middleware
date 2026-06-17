using System;
using System.Collections.Generic;

namespace Ersa.Mes.Common.Extensions;

public static class Edc_MesFunctionExtensions
{
	public static IEnumerable<T> Fun_fdcZb<T>(this IEnumerable<T> i_list, Func<T, bool> i_delFunction)
	{
		if (i_list == null || i_delFunction == null)
		{
			throw new Exception("Can not be null...");
		}
		foreach (T item in i_list)
		{
			if (i_delFunction(item))
			{
				yield return item;
			}
		}
	}

	public static Dictionary<Type, T> Fun_fdcContain<T>(this IDictionary<Type, T> i_dic, Func<KeyValuePair<Type, T>, bool> i_delFunction)
	{
		Dictionary<Type, T> list = new Dictionary<Type, T>();
		foreach (KeyValuePair<Type, T> item in i_dic)
		{
			if (i_delFunction(item))
			{
				list.Add(item.Key, item.Value);
			}
		}
		return list;
	}

	public static void Fun_fdcForeach<T>(this IDictionary<Type, T> i_dic, Action<KeyValuePair<Type, T>> i_delFunction)
	{
		foreach (KeyValuePair<Type, T> item in i_dic)
		{
			i_delFunction(item);
		}
	}
}
