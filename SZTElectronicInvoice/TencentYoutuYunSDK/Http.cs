using System;
using System.IO;
using System.Net;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace TencentYoutuYun.SDK.Csharp
{
    public class Http
    {
        /// <summary>
        /// send http request with POST method
        /// </summary>
        /// <param name="methodName">请求的接口名称</param>
        /// <param name="postData">请求数据</param>
        /// <param name="authorization">签名</param>
        /// <returns></returns>
        public static string HttpPost(string methodName, string postData, string authorization)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(postData); //转化为UTF8
                HttpWebRequest webReq=null ;

                if (Conf.Instance().END_POINT.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    webReq = WebRequest.Create((Conf.Instance().END_POINT + methodName)) as HttpWebRequest;
                    webReq.ProtocolVersion = HttpVersion.Version11;
                }
                else
                {
                    webReq = (HttpWebRequest)WebRequest.Create(new Uri(Conf.Instance().END_POINT + methodName));
                }

               
                webReq.Method = "POST";
                webReq.ContentType = "text/json";
                webReq.Headers.Add(HttpRequestHeader.Authorization, authorization);
                webReq.ServicePoint.Expect100Continue = false;

                //webReq.Expect = "100-Continue";
                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        int errorcode = (int)response.StatusCode;
                        ret = Youtu.statusText(errorcode);
                    }
                    else
                    {
                        // no http status code available
                    }
                }
                else
                {
                    // no http status code available
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ret;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受   
        }

    }
}
