using System.Data.Entity;
using Ersa.Mes.Database.Migrations;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

public class Ersasoft4 : DbContext
{
	public virtual DbSet<Machine> Machines { get; set; }

	public virtual DbSet<MachineCondition> MachineConditions { get; set; }

	public virtual DbSet<TransferMessage> TransferMessages { get; set; }

	public virtual DbSet<Initialize> Initializes { get; set; }

	public virtual DbSet<CurrentStatus> CurrentStatus { get; set; }

	public virtual DbSet<SolderingProgramReflow> SolderingProgramReflows { get; set; }

	public virtual DbSet<PopupDialog> PopupDialogs { get; set; }

	public virtual DbSet<ConfirmRecipe> ConfirmRecipes { get; set; }

	public virtual DbSet<PcbInfeed> PcbInfeeds { get; set; }

	public virtual DbSet<PcbOutfeed> PcbOutfeeds { get; set; }

	public virtual DbSet<Ztxt> Ztxts { get; set; }

	public virtual DbSet<ErsasoftTrigger> ErsasoftTriggers { get; set; }

	public virtual DbSet<SelectProgram> SelectPrograms { get; set; }

	public virtual DbSet<TransferUserInput> TransferUserInputs { get; set; }

	public virtual DbSet<ProgramContentChangeRecord> ProgramContentChangeRecords { get; set; }

	public virtual DbSet<TransferParameter> TransferParameters { get; set; }

	public virtual DbSet<TransferParameterBase> TransferParameterBases { get; set; }

	public virtual DbSet<TransferParameterHeader> TransferParameterHeaders { get; set; }

	public virtual DbSet<TransferParameterConvection> TransferParameterConvections { get; set; }

	public virtual DbSet<TransferParameterConvery> TransferParameterConverys { get; set; }

	public virtual DbSet<TransferParameterO2N2> TransferParameterO2N2s { get; set; }

	public virtual DbSet<TransferParameterPyro> TransferParameterPyros { get; set; }

	public virtual DbSet<TransferParameterTempActual> TransferParameterTempActuals { get; set; }

	public virtual DbSet<TransferParameterTempOutputHz> TransferParameterTempOutputHzs { get; set; }

	public virtual DbSet<TransferParameterTempSet> TransferParameterTempSets { get; set; }

	public Ersasoft4()
		: base("name=Ersasoft4")
	{
		System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<Ersasoft4, Configuration>());
	}

	protected override void OnModelCreating(DbModelBuilder modelBuilder)
	{
	}
}
