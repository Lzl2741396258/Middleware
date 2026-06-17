using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.MesFunctionFolder;
using MesXPT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MesXPT.XPT_MesFunction
{
    public class XTP_ConfirmRecipe : ConfirmRecipe
    {
        private XPT_Config m_Config { get; set; }

       // private Edc_MagnaCommunicationService m_edcService { get; set; }

        public XTP_ConfirmRecipe(XPT_Config i_Config, Inf_Logger i_edcLogger)
        : base(i_Config.m_lstMesFunction, i_edcLogger, i_Config.m_edcFilesPath.m_strPathBibs)
        {
            m_Config = i_Config;
        }

        protected override Task Fun_blnConnectMesPlatform()
        {
            return Task.FromResult(result: true);
        }
    }
}
