using Ersa.Mes.FileSystem.Model.Protocol;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.FileSystem.Service;

namespace Ersa.Mes.FileSystem.Interface;

public interface Inf_Protocol : Inf_FileService
{
	Enum_MachineType Pro_enuMachineType { get; }

	Enum_ProtocolType Pro_enuProtocolType { get; }

	object Fun_objConvert(object i_objData);
}
