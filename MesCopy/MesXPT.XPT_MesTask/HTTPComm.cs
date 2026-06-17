using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MesXPT.XPT_MesTask
{
    class HTTPComm
    {
        public static string Get(string sURL, Dictionary<string, string> dic, int iTimtout)
        {
            try
            {
                sURL += "?";

                foreach (string key in dic.Keys)
                {
                    sURL += key + "=" + dic[key] + "&";
                }

                sURL = sURL.Trim();

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL);

                request.Proxy = null;
                request.KeepAlive = false;
                request.Method = "GET";
                request.ContentType = "application/json; charset=UTF-8";
                request.Timeout = iTimtout;
                request.AutomaticDecompression = DecompressionMethods.GZip;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();

                StreamReader streamReader = new StreamReader(responseStream);

                string reVal = streamReader.ReadToEnd();

                streamReader.Close();

                responseStream.Close();

                return reVal;
            }
            catch (Exception ex)
            {
                //Log.Write(sLogPath, "POST", ex.Message);
                return string.Empty;
            }
        }
        public static async Task<string> PostAsync(string sURL, string sJSON, int iTimtout)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL);

                request.Proxy = null;
                request.KeepAlive = false;
                request.Method = "POST";
                request.ContentType = "application/json; charset=UTF-8";
                request.Timeout = iTimtout;
                request.AutomaticDecompression = DecompressionMethods.GZip;
                

                byte[] bArray = System.Text.Encoding.UTF8.GetBytes(sJSON);

                request.ContentLength = bArray.Length;

                using (Stream writer = await request.GetRequestStreamAsync())
                {
                    await writer.WriteAsync(bArray, 0, bArray.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                        {
                            return await streamReader.ReadToEndAsync();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                //Log.Write(sLogPath, "POST", e.Message);
                return string.Empty;
            }
            catch (Exception ex)
            {
                //Log.Write(sLogPath, "POST", ex.Message);
                return string.Empty;
            }
        }


        public static string Post(string sURL, string sJSON, int iTimtout)
        {
            try
            {
           
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL);

                ServicePointManager.UseNagleAlgorithm = true;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.CheckCertificateRevocationList = true;
                ServicePointManager.DefaultConnectionLimit = ServicePointManager.DefaultPersistentConnectionLimit;

                request.Proxy = null;
                request.KeepAlive = false;
                request.Method = "POST";
                request.ContentType = "application/json; charset=UTF-8";
                request.Timeout = iTimtout;
                request.ProtocolVersion = HttpVersion.Version11;
                request.KeepAlive = false;
                request.AutomaticDecompression = DecompressionMethods.GZip;
         
                byte[] bArray = System.Text.Encoding.UTF8.GetBytes(sJSON);
           
                request.ContentLength = bArray.Length;             

                Stream writer = request.GetRequestStream();
               
                writer.Write(bArray, 0, bArray.Length);
                
                writer.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();

                StreamReader streamReader = new StreamReader(responseStream);
              
                string reVal = streamReader.ReadToEnd();

                streamReader.Close();

                responseStream.Close();

                return reVal;
            }
            catch (WebException e)
            {
                MessageBox.Show("No MES Response : " + e.Message);

                string logContent = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "  [HTTP-Post]  " + e.Message + "\r\n";
               
                return string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No MES Response Rean : " + ex.Message);

                string logContent = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "  [HTTP-Post]  " + ex.Message + "\r\n";           

                return string.Empty;
            }

        }
        }
    }
