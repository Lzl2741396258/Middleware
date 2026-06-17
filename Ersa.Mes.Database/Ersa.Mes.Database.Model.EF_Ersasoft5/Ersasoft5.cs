using System.Data.Entity;

namespace Ersa.Mes.Database.Model.EF_Ersasoft5;

public class Ersasoft5 : DbContext
{
	public virtual DbSet<activeuser> activeusers { get; set; }

	public virtual DbSet<aoiprogram> aoiprograms { get; set; }

	public virtual DbSet<aoiresult> aoiresults { get; set; }

	public virtual DbSet<aoistepdata> aoistepdatas { get; set; }

	public virtual DbSet<cadcncstep> cadcncsteps { get; set; }

	public virtual DbSet<cadflowstep> cadflowsteps { get; set; }

	public virtual DbSet<cadforbiddenarea> cadforbiddenareas { get; set; }

	public virtual DbSet<cadimage> cadimages { get; set; }

	public virtual DbSet<cadmotiongroup> cadmotiongroups { get; set; }

	public virtual DbSet<cadroutingdata> cadroutingdatas { get; set; }

	public virtual DbSet<cadroutingstep> cadroutingsteps { get; set; }

	public virtual DbSet<cadsetting> cadsettings { get; set; }

	public virtual DbSet<codecache> codecaches { get; set; }

	public virtual DbSet<codeconfiguration> codeconfigurations { get; set; }

	public virtual DbSet<codeconfigurationtracking> codeconfigurationtrackings { get; set; }

	public virtual DbSet<codepipeline> codepipelines { get; set; }

	public virtual DbSet<codepipelinetracking> codepipelinetrackings { get; set; }

	public virtual DbSet<codetablemember> codetablemembers { get; set; }

	public virtual DbSet<codetable> codetables { get; set; }

	public virtual DbSet<cyclicmessagelib> cyclicmessagelibs { get; set; }

	public virtual DbSet<equipment> equipments { get; set; }

	public virtual DbSet<equipmenttool> equipmenttools { get; set; }

	public virtual DbSet<languageentry> languageentries { get; set; }

	public virtual DbSet<linkcache> linkcaches { get; set; }

	public virtual DbSet<machinecondition> machineconditions { get; set; }

	public virtual DbSet<machineconfiguration> machineconfigurations { get; set; }

	public virtual DbSet<machinegroupmember> machinegroupmembers { get; set; }

	public virtual DbSet<machineoperatingdatahead> machineoperatingdataheads { get; set; }

	public virtual DbSet<machineoperatingdatavalue> machineoperatingdatavalues { get; set; }

	public virtual DbSet<machine> machines { get; set; }

	public virtual DbSet<machinesetting> machinesettings { get; set; }

	public virtual DbSet<maschinegroup> maschinegroups { get; set; }

	public virtual DbSet<messagecontext> messagecontexts { get; set; }

	public virtual DbSet<message> messages { get; set; }

	public virtual DbSet<messagescyclic> messagescyclics { get; set; }

	public virtual DbSet<messagescyclictemplate> messagescyclictemplates { get; set; }

	public virtual DbSet<nozzlegeometry> nozzlegeometries { get; set; }

	public virtual DbSet<nozzleoperatingactualvalue> nozzleoperatingactualvalues { get; set; }

	public virtual DbSet<nozzleoperatingchanx> nozzleoperatingchanges { get; set; }

	public virtual DbSet<nozzleoperatingtargetvalue> nozzleoperatingtargetvalues { get; set; }

	public virtual DbSet<nozzle> nozzles { get; set; }

	public virtual DbSet<nozzlesetvalue> nozzlesetvalues { get; set; }

	public virtual DbSet<operatingmaterial> operatingmaterials { get; set; }

	public virtual DbSet<packagemacro> packagemacros { get; set; }

	public virtual DbSet<package> packages { get; set; }

	public virtual DbSet<paneldata> paneldatas { get; set; }

	public virtual DbSet<parameter> parameters { get; set; }

	public virtual DbSet<placerprogram> placerprograms { get; set; }

	public virtual DbSet<productioncontrol> productioncontrols { get; set; }

	public virtual DbSet<programecp3data> programecp3data { get; set; }

	public virtual DbSet<programhistory> programhistories { get; set; }

	public virtual DbSet<programimage> programimages { get; set; }

	public virtual DbSet<programpanelparameter> programpanelparameters { get; set; }

	public virtual DbSet<programparameter> programparameters { get; set; }

	public virtual DbSet<programsetdata> programsetdatas { get; set; }

	public virtual DbSet<protocoldata_ma18> protocoldata_ma18 { get; set; }

	public virtual DbSet<protocoldata_ma54369> protocoldata_ma54369 { get; set; }

	public virtual DbSet<protocoldata_ma55794> protocoldata_ma55794 { get; set; }

	public virtual DbSet<protocolheads_ma18> protocolheads_ma18 { get; set; }

	public virtual DbSet<protocolheads_ma54369> protocolheads_ma54369 { get; set; }

	public virtual DbSet<protocolheads_ma55794> protocolheads_ma55794 { get; set; }

	public virtual DbSet<protocolparameter_ma18> protocolparameter_ma18 { get; set; }

	public virtual DbSet<protocolparameter_ma54369> protocolparameter_ma54369 { get; set; }

	public virtual DbSet<protocolparameter_ma55794> protocolparameter_ma55794 { get; set; }

	public virtual DbSet<protocolvariable> protocolvariables { get; set; }

	public virtual DbSet<recorderdata_ma1> recorderdata_ma1 { get; set; }

	public virtual DbSet<recorderdata_ma18> recorderdata_ma18 { get; set; }

	public virtual DbSet<recorderdata_ma217328> recorderdata_ma217328 { get; set; }

	public virtual DbSet<recorderdata_ma54369> recorderdata_ma54369 { get; set; }

	public virtual DbSet<recorderdata_ma55794> recorderdata_ma55794 { get; set; }

	public virtual DbSet<recordervariable> recordervariables { get; set; }

	public virtual DbSet<solderinglibrary> solderinglibraries { get; set; }

	public virtual DbSet<solderingprogram> solderingprograms { get; set; }

	public virtual DbSet<solderingversionvalid> solderingversionvalids { get; set; }

	public virtual DbSet<usermachinemapping> usermachinemappings { get; set; }

	public virtual DbSet<user> users { get; set; }

	public virtual DbSet<usertracking> usertrackings { get; set; }

	public Ersasoft5()
		: base("name=Ersasoft5")
	{
	}

	protected override void OnModelCreating(DbModelBuilder modelBuilder)
	{
	}
}
