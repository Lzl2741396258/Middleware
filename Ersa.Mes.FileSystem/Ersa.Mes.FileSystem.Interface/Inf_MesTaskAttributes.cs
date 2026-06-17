namespace Ersa.Mes.FileSystem.Interface;

public interface Inf_MesTaskAttributes
{
	string Pro_strName { get; set; }

	bool Pro_blnActive { get; set; }

	int Pro_i32Interval { get; set; }

	int Pro_i32DelayTime { get; set; }

	int Pro_i32Repetition { get; set; }
}
