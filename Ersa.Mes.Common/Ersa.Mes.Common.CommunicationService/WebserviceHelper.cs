using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ersa.Mes.Common.CommunicationService;

public class WebserviceHelper
{
	public Task<string> Fun_strWebservice(string a_strUrl, string a_strPostData, string a_strContentType, int i_i32Timeout)
	{
		string url = a_strUrl + "msg=" + a_strPostData;
		HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
		request.Accept = "Accept:text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
		request.Headers["Accept-Language"] = "zh-CN,zh;q=0.";
		request.Headers["Accept-Charset"] = "GBK,utf-8;q=0.7,*;q=0.3";
		request.UserAgent = "User - Agent:Mozilla / 5.0(Windows NT 5.1) AppleWebKit / 535.1(KHTML, like Gecko) Chrome / 14.0.835.202 Safari / 535.1";
		request.Method = "POST";
		request.KeepAlive = true;
		request.ContentType = "application/x-www-form-urlencoded";
		request.Timeout = i_i32Timeout;
		byte[] postData = Encoding.UTF8.GetBytes(a_strPostData);
		CookieContainer cookieContainer = new CookieContainer();
		request.CookieContainer = cookieContainer;
		request.ContentLength = postData.Length;
		try
		{
			using Stream writer = request.GetRequestStream();
			writer.Write(postData, 0, postData.Length);
		}
		catch (Exception ex2)
		{
			return Task.FromResult(ex2.Message);
		}
		string a_strResult = string.Empty;
		try
		{
			HttpWebResponse response = request.GetResponse() as HttpWebResponse;
			using Stream sr = response.GetResponseStream();
			using StreamReader reader = new StreamReader(sr, Encoding.UTF8);
			a_strResult = reader.ReadToEnd();
		}
		catch (Exception ex)
		{
			return Task.FromResult(ex.Message);
		}
		return Task.FromResult(a_strResult);
	}
}
