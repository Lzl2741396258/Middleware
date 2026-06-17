using Ersa.Mes.Logging;
using MesXPT.XPT_MesFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            XPT_ChooseRecipe xPT_Choose = new XPT_ChooseRecipe(new MesXPT.Model.XPT_Config()
            {
                m_ersaDownLineUrl = "http://127.0.0.1:3001/BarcodeCheck"
            }, new Edc_Logger(Enum_LogLevels.Error));
            xPT_Choose.Fun_Test();

            //TestApi();
            Console.ReadLine();

        }


        private static async void TestApi()
        {
            try
            {
                var _httpClient = new HttpClient();
                // 使用HttpClient发送POST请求
                using (var cts = new System.Threading.CancellationTokenSource(1))
                {
                    var content = new StringContent("{}", Encoding.UTF8, "application/json");

                    HttpResponseMessage response;
                    try
                    {
                        // 发送POST请求并等待响应
                        response = await _httpClient.PostAsync("http://127.0.0.1:3001/BarcodeCheck", content, cts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        // 处理超时
                        return;
                    }

                    // 读取响应内容
                    string responseJson = await response.Content.ReadAsStringAsync();

                    return ;
                }
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}
