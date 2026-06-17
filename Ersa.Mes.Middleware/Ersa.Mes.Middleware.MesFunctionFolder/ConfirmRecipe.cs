using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.Common.Extensions;
using Ersa.Mes.FileSystem.Model.Bibs;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.FileSystem.Model.RequestResponse.Request;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Definition;
using Ersa.Mes.Middleware.Factory;

namespace Ersa.Mes.Middleware.MesFunctionFolder;

public class ConfirmRecipe : Edc_MesFunction
{
	public string m_strLibrary = string.Empty;

	public string m_strProgram = string.Empty;

	protected override string m_strFunctionName => "ConfirmRecipe";

	public override bool m_blnResponse => false;

	public Request_ConfirmRecipe m_Request { get; set; }

	public string m_strPathBibs { get; set; }

	public ConfirmRecipe(List<Edc_ConfigMesFunction> i_lstMesFunction, Inf_Logger i_edcLogger, string i_strPathBibs)
		: base(i_edcLogger)
	{
		Edc_ConfigMesFunction function = i_lstMesFunction.Where((Edc_ConfigMesFunction s) => s.Pro_strName.Equals(m_strFunctionName)).FirstOrDefault();
		if (function != null)
		{
			base.m_blnActivePlatform = function.Pro_blnActivePlatform;
			base.m_blnActiveDatabase = function.Pro_blnActiveDatabase;
			base.m_blnActiveLocalFile = function.Pro_blnActiveLocalFile;
		}
		m_strPathBibs = i_strPathBibs;
		i_edcLogger.Debug($"Assmebly MES Function {m_strFunctionName}   Platform:{base.m_blnActivePlatform}  Database:{base.m_blnActiveDatabase}  LocalFile:{base.m_blnActiveLocalFile}", null, ".ctor", 69);
	}

	public override bool Fun_blnContains()
	{
		return base.m_strRequest.IndexOf(Enum_MesFunction.RezeptBestaetigen_Anfo_EL.Fun_strGetDescription(), StringComparison.OrdinalIgnoreCase) > 0;
	}

	public override void Sub_GetRequest()
	{
		m_Request = base.m_strRequest.Fun_DeserializeContent<Request_ConfirmRecipe>();
	}

	public override Task<string> Fun_strExecute()
	{
		try
		{
			Sub_GetRequest();
			if (base.m_blnActivePlatform)
			{
				base.m_edcLogger.Debug("Begin " + MethodBase.GetCurrentMethod().DeclaringType.Name + ".Fun_blnConnectMesPlatform()...", null, "Fun_strExecute", 97);
				Task.Run((Func<Task>)Fun_blnConnectMesPlatform);
			}
			if (base.m_blnActiveDatabase)
			{
				Task.Run((Action)Sub_AddToDatabase);
			}
			if (base.m_blnActiveLocalFile)
			{
				Task.Run(delegate
				{
					Sub_WriteToLocalFile(MethodBase.GetCurrentMethod().DeclaringType.Name);
				});
			}
			OnShowMessage(Enum_LogType.Info, "Middleware -> ConfirmRecipe Triggered..." + Fun_strGetBibsPath());
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "'Confirm Recipe' Error...Details:'" + ex.Message + "'");
		}
		return Task.FromResult(string.Empty);
	}

	public string Fun_strGetBibsPath()
	{
		string a_Path = string.Empty;
		try
		{
			m_strLibrary = m_Request.m_strLibraryName;
			m_strProgram = m_Request.m_strProgramName;
			a_Path = Path.Combine(m_strPathBibs, m_strLibrary, m_strProgram);
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "MES Function 'Confirm Recipe' Error...Details:'" + ex.Message + "'");
		}
		return a_Path;
	}

	public string Fun_strGetBibsPathAndFilename(string i_strLibrary, string i_strRecipeName)
	{
		return Path.Combine(m_strPathBibs, i_strLibrary, i_strRecipeName);
	}

	public Edc_BibsReflow Fun_edcGetReflowData()
	{
		try
		{
			string a_strFullName = Fun_strGetBibsPathAndFilename(m_Request.m_strLibraryName, m_Request.m_strProgramName);
			if (!File.Exists(a_strFullName))
			{
				return null;
			}
			string a_strContent = string.Empty;
			using (StreamReader streamReader = new StreamReader(a_strFullName))
			{
				a_strContent = streamReader.ReadToEnd();
				a_strContent = a_strContent.Replace(',', '.');
			}
			return a_strContent.Fun_DeserializeContent<Edc_BibsReflow>();
		}
		catch
		{
			return null;
		}
	}
}
