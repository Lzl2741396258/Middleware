using System;
using System.Collections.Generic;
using Ersa.Mes.Middleware.Interface;

namespace Ersa.Mes.Middleware.Factory;

public class Edc_MesContainer : Inf_MesFunctionContainer
{
	private IDictionary<Type, object> _dicObjects = new Dictionary<Type, object>();

	public IDictionary<Type, object> m_dicMesFunction
	{
		get
		{
			return _dicObjects;
		}
		set
		{
			_dicObjects = value;
		}
	}

	public void Sub_AddObject<T>(object i_objObject)
	{
		Sub_RemoveObject<T>();
		_dicObjects.Add(typeof(T), i_objObject);
	}

	public T Fun_edcGetObject<T>() where T : class
	{
		if (!_dicObjects.TryGetValue(typeof(T), out var value))
		{
			return null;
		}
		return (T)value;
	}

	public void Sub_RemoveObject<T>()
	{
		_dicObjects.Remove(typeof(T));
	}
}
