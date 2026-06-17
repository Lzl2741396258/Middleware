using System;
using System.Collections.Generic;

namespace Ersa.Mes.Middleware.Factory;

public class MesFunction_Management
{
	public IDictionary<Type, object> m_dicObjects;

	public MesFunction_Management()
	{
		m_dicObjects = new Dictionary<Type, object>();
	}

	public T Sub_GetModel<T>() where T : class
	{
		if (!m_dicObjects.TryGetValue(typeof(T), out var value))
		{
			return null;
		}
		return (T)value;
	}

	public void Sub_Add<T>(object i_objObject)
	{
		Sub_Remove<T>();
		m_dicObjects.Add(typeof(T), i_objObject);
	}

	public void Sub_Remove<T>()
	{
		m_dicObjects.Remove(typeof(T));
	}
}
