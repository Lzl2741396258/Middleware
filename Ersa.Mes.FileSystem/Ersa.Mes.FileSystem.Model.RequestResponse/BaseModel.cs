using Ersa.Mes.FileSystem.Model.RequestResponse.Request;
using Ersa.Mes.FileSystem.Model.RequestResponse.Response;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public class BaseModel
{
	public Request_BuildConnection m_RequestBuildConnection;

	public Request_ChooseRecipe m_RequestChooseRecipe;

	public Request_CloseConnection m_RequestCloseConnection;

	public Request_HeartBeat m_RequestHeartBeat;

	public Request_ReleaseInfeed m_RequestInfeedRelease;

	public Request_MachineCondition m_RequestMachineCondition;

	public Request_TransferMessages m_RequestTransferMessages;

	public ViewRequestBuildConnection m_ViewRequestBuildConnection;

	public Response_BuildConnection m_ResponseBuildConnection;

	public Response_ChooseRecipe m_ResponseChooseRecipe;

	public Response_CloseConnection m_ResponseCloseConnection;

	public Response_HeartBeat m_ResponseHeartBeat;

	public Response_ReleaseInfeed m_ResponseInfeedRelease;
}
