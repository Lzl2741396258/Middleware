using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ersa.Mes.Common.Model;

public class Edc_LanguageItem
{
	[JsonProperty("Items")]
	public List<LanguageMode> Items { get; set; }
}
