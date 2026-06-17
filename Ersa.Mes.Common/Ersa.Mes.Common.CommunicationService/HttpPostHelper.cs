using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ersa.Mes.Common.CommunicationService;

public class HttpPostHelper
{
	public static async Task<string> Fun_strHttpApiAsync(string i_strUrl, string i_strPostData, string i_strMethod, int i_i32Timeout, string i_strContentType, string i_strToken = "")
	{
		try
		{
			HttpWebRequest request;
			if (i_strUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				ServicePointManager.ServerCertificateValidationCallback = (object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true;
				ServicePointManager.CheckCertificateRevocationList = false;
				ServicePointManager.DefaultConnectionLimit = 512;
				ServicePointManager.Expect100Continue = false;
				request = WebRequest.Create(i_strUrl) as HttpWebRequest;
				request.ProtocolVersion = HttpVersion.Version10;
			}
			else
			{
				request = WebRequest.Create(i_strUrl) as HttpWebRequest;
			}
			if (!string.IsNullOrEmpty(i_strToken))
			{
				request.Headers.Add("Authorization", i_strToken);
			}
			request.Timeout = i_i32Timeout;
			request.Method = i_strMethod.ToString();
			request.ContentType = i_strContentType;
			if (!i_strMethod.Equals("Get", StringComparison.OrdinalIgnoreCase))
			{
				byte[] a_bytBodys = Encoding.UTF8.GetBytes(i_strPostData);
				request.ContentLength = a_bytBodys.Length;
				Stream stream = request.GetRequestStream();
				stream.Write(a_bytBodys, 0, a_bytBodys.Length);
			}
			WebResponse response = request.GetResponse();
			using StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
			string a_strReturn2 = streamReader.ReadToEnd();
			streamReader.Close();
			return a_strReturn2;
		}
		catch (WebException ex2)
		{
			WebException ex = ex2;
			_ = ex.Status;
			if (ex.Response != null)
			{
				using (StreamReader myStreamReader = new StreamReader(ex.Response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
				{
					string a_strReturn = myStreamReader.ReadToEnd();
					myStreamReader.Close();
					return a_strReturn;
				}
			}
			throw;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static async Task<string> Fun_strHttpApiAsync(string i_strUrl, string i_strPostData, Enum_HttpMethod i_enuMethod, int i_i32Timeout, string i_strContentType, string i_strToken = "")
	{
		return await Fun_strHttpApiAsync(i_strUrl, i_strPostData, i_enuMethod.ToString(), i_i32Timeout, i_strContentType, i_strToken);
	}
}
