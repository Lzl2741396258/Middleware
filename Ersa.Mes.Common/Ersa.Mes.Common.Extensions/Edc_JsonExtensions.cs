using Newtonsoft.Json;

namespace Ersa.Mes.Common.Extensions;

public static class Edc_JsonExtensions
{
	public static string Fun_strJson(this object o)
	{
		if (o == null)
		{
			return null;
		}
		return JsonConvert.SerializeObject(o);
	}

	public static T Fun_Json<T>(this string input)
	{
		return JsonConvert.DeserializeObject<T>(input);
	}
}
