using System.Collections.Specialized;
using System.Data.Common;

namespace Ersa.Mes.Database.Interface;

public abstract class Edc_BasicProvider
{
	protected NameValueCollection Pro_fdcSettings { get; private set; }

	public Edc_BasicProvider(NameValueCollection i_fdcAppSettings)
	{
		Pro_fdcSettings = i_fdcAppSettings;
	}

	public NameValueCollection Fun_fdcGetProviderSettings()
	{
		return Pro_fdcSettings;
	}

	protected abstract DbConnectionStringBuilder Fun_CreateConnectionStringBuilder();
}
