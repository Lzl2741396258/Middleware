using System.Data.Common;

namespace Ersa.Mes.Database.Interface;

public interface Inf_DatabaseProvider
{
	string Pro_ConnectionString { get; }

	DbConnection Fun_fdcGetConnection();
}
