using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ersa.Mes.Database.Model.EF_Ersasoft4;

[Table("public.currentstatus")]
public class CurrentStatus
{
	[Key]
	public int StatusId { get; set; } = 1;


	public bool SelectProgramActive { get; set; } = false;


	public string SelectProgramGuid { get; set; }

	public string RequestMessage { get; set; }

	public string SelectProgramLibrary { get; set; }

	public string SelectProgramName { get; set; }

	public string SelectProgramResult { get; set; }

	public string SelectProgramText { get; set; }

	public bool InfeedNotification { get; set; }

	public bool ReleaseInfeed { get; set; }

	public bool OutfeedNotification { get; set; }

	public bool ReleaseOutfeed { get; set; }

	public DateTime LastResetInfeedTime { get; set; }

	public bool MESActive { get; set; }
}
