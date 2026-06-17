using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.Trace05;

[Serializable]
public struct Struct_Diretory
{
	[XmlElement("RezeptAuswaehlen")]
	public Struct_AnfoStat SelectRecipe { get; set; }

	[XmlElement("RezeptBestaetigen")]
	public Struct_Anfo ConfirmRecipe { get; set; }

	[XmlElement("EinlaufFreigeben")]
	public Struct_AnfoStat ReleaseInfeed { get; set; }

	[XmlElement("PcbEingelaufen")]
	public Struct_Anfo PCBRunInto { get; set; }

	[XmlElement("AuslaufProtokollErstellen")]
	public Struct_AnfoStat OutfeedProtocal { get; set; }

	[XmlElement("AuslaufFreigeben")]
	public Struct_AnfoStat ReleaseSpout { get; set; }

	[XmlElement("PcbAusgelaufen")]
	public Struct_Anfo PcbLeakedout { get; set; }

	[XmlElement("Maschinenzustand")]
	public Struct_Anfo MachineCondition { get; set; }

	[XmlElement("VerlustzeitenMelden")]
	public Struct_AnfoStat ReportLossTimes { get; set; }

	[XmlElement("TestmodusAuswaehlen")]
	public Struct_Anfo SelecTestMode { get; set; }

	[XmlElement("StatusinformationAnzeigen")]
	public Struct_Anfo InformationStatusDisplay { get; set; }

	[XmlElement("PopupAnzeigen")]
	public Struct_Anfo ShowPopup { get; set; }

	[XmlElement("MeldungenUebertragen")]
	public Struct_Anfo TransferMessages { get; set; }

	[XmlElement("ParameterUebertragen")]
	public Struct_AnfoStat TransferParameter { get; set; }

	[XmlElement("HeartbeatAuswerten")]
	public Struct_AnfoStat EvaluateHeartbeat { get; set; }

	[XmlElement("Initialisierung")]
	public Struct_AnfoStat Initialization { get; set; }

	[XmlElement("PcbBearbeitungAbgebrochen")]
	public Struct_Anfo PcbEditingAborted { get; set; }

	[XmlElement("ProgrammAuswaehlen")]
	public Struct_AnfoStat ProgramSelect { get; set; }

	[XmlElement("ErsasoftTriggern")]
	public Struct_Anfo TriggenErsasoft { get; set; }

	[XmlElement("LoetprogrammeUebertragen")]
	public Struct_AnfoStat TransferSolderingPrograms { get; set; }

	[XmlElement("VerbindungAufbauen")]
	public Struct_AnfoStat BuildingConnections { get; set; }

	[XmlElement("TextZuordnung")]
	public Struct_TextAssignment TextAssignment { get; set; }
}
