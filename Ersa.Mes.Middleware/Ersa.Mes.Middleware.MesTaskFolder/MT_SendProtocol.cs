using System.Threading.Tasks;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Factory;

namespace Ersa.Mes.Middleware.MesTaskFolder;

public abstract class MT_SendProtocol : Edc_MesTask
{
	public string m_strPathAndFilename { get; set; }

	public MT_SendProtocol(Inf_MesTaskAttributes i_edcMesTaskAttributes, Inf_Logger i_edcLogger)
		: base(i_edcMesTaskAttributes, i_edcLogger)
	{
	}

	public virtual bool Sub_SendProtocol(string i_strPathAndFilename)
	{
		return true;
	}

	public virtual Task<bool> Sub_SendProtocolAsync(string i_strPathAndFilename)
	{
		return null;
	}

	public virtual bool Sub_SendProtocol(Struct_ProtocolElement[] ia_sttProtocolElement)
	{
		return true;
	}
}
