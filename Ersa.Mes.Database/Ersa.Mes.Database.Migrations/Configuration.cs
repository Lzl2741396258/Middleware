using System.Data.Entity.Migrations;
using Ersa.Mes.Database.Model.EF_Ersasoft4;

namespace Ersa.Mes.Database.Migrations;

internal sealed class Configuration : DbMigrationsConfiguration<Ersasoft4>
{
	public Configuration()
	{
		base.AutomaticMigrationsEnabled = true;
		base.AutomaticMigrationDataLossAllowed = true;
		base.ContextKey = "Ersa.Mes.Database.Model.EF_Ersasoft4.Ersasoft4";
	}

	protected override void Seed(Ersasoft4 context)
	{
	}
}
