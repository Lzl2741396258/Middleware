using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Ersa.Mes.Middleware.Helper;

public static class MappingHelper<TIn, TOut>
{
	private static Func<TIn, TOut> _func;

	static MappingHelper()
	{
		ParameterExpression pe = Expression.Parameter(typeof(TIn), "p");
		List<MemberBinding> lstMemberBinding = new List<MemberBinding>();
		PropertyInfo[] properties = typeof(TOut).GetProperties();
		foreach (PropertyInfo item in properties)
		{
			MemberExpression property = Expression.Property(pe, typeof(TIn).GetProperty(item.Name));
			MemberBinding memberBinding = Expression.Bind(item, property);
			lstMemberBinding.Add(memberBinding);
		}
		FieldInfo[] fields = typeof(TOut).GetFields();
		foreach (FieldInfo item2 in fields)
		{
			MemberExpression property2 = Expression.Field(pe, typeof(TIn).GetField(item2.Name));
			MemberBinding memberBinding2 = Expression.Bind(item2, property2);
			lstMemberBinding.Add(memberBinding2);
		}
		MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), lstMemberBinding.ToArray());
		Expression<Func<TIn, TOut>> lamda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[1] { pe });
		_func = lamda.Compile();
	}

	public static TOut Convert(TIn t)
	{
		return _func(t);
	}
}
