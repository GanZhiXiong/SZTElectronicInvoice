using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using DevComponents.DotNetBar;

namespace SZTElectronicInvoice
{
   public class GZXNetworking
    {
        private static CookieContainer _cookies = null;
        //        public string VerificationImageFileName = "pic.jpg";
        private static string ruokuaiusername="ganzhixiong";
        private static string ruokuaipassword = "xinwei1682";
        private static string ruokuaitypeid= "7110";
        private static string ruokuaitimeout= "90";
        private static string ruokuaisoftid= "104131";
        private static string ruokuaisoftkey= "3505c868cf0f4122be7b930848288c92";

        /// <summary>
        /// 获取验证码和Cookie
        /// </summary>
        public static Image GetValidateImage()
        {
            try
            {
                _cookies = new CookieContainer();
                string url = "https://www.shenzhentong.com/ajax/WaterMark.ashx"; //验证码页面
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Accept = "*/*";
                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0";
                request.CookieContainer = new CookieContainer(); //暂存到新实例
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                    return null;

                /*    //识别验证码
                    int startX = 0;
                    int startY = 0;
                    int width = -1;
                    int height = -1;*/

                Stream resStream = response.GetResponseStream();
                Image image = Image.FromStream(resStream);
                /* width = image.Width;
                 height = image.Height;

                 image.Save(VerificationImageFileName);

                 Image img = Image.FromFile(VerificationImageFileName);
                 Image bmp = new Bitmap(img);
                 img.Dispose();
                 picVerificationImage.Image = bmp; //new Bitmap(resStream);*/

                //                textBoxXTimgcode.Text =
                //                    Marshal.PtrToStringAnsi(OCRpart(VerificationImageFileName, -1, startX, startY, width, height))
                //                        .Replace(" ", "")
                //                        .Replace("\r\n", "");

                _cookies = request.CookieContainer; //保存cookies
                                                    //                GlobalManager.Cookie = request.CookieContainer.GetCookieHeader(request.RequestUri);
                                                    //把cookies转换成字符串
                return image;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }

        public static byte[] GetByteImage(Image img)
        {
            byte[] bt = null;
            if (!img.Equals(null))
            {
                using (MemoryStream mostream = new MemoryStream())
                {
                    Bitmap bmp = new Bitmap(img);
                    bmp.Save(mostream, System.Drawing.Imaging.ImageFormat.Bmp);//将图像以指定的格式存入缓存内存流
                    bt = new byte[mostream.Length];
                    mostream.Position = 0;//设置留的初始位置
                    mostream.Read(bt, 0, Convert.ToInt32(bt.Length));
                }
            }
            return bt;
        }

        public static string ORC(Image image, int failureTryCount = 3)
        {
//            try
//            {
                if (failureTryCount == 0)
                {
                    return null;
                }

                //必要的参数
                var param = new Dictionary<object, object>
            {
                {"username",ruokuaiusername},
                {"password",ruokuaipassword},
                {"typeid",ruokuaitypeid},
                {"timeout",ruokuaitimeout},
                {"softid",ruokuaisoftid},
                {"softkey",ruokuaisoftkey}
            };
                //            byte[] data = File.ReadAllBytes("pic.jpg");
                byte[] data = GetByteImage(image);

                //提交服务器
                string httpResult = RuoKuaiHttp.Post("http://api.ruokuai.com/create.xml", param, data);

                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.LoadXml(httpResult);
                }
                catch
                {
                    failureTryCount--;
                    ORC(image, failureTryCount);
                    return null;
                    /*richTextBox1.BeginInvoke(new EventHandler(delegate
                    {
                        richTextBox1.AppendText("返回格式有误\r\n");
                        richTextBox1.Select(richTextBox1.TextLength, richTextBox1.TextLength);
                        richTextBox1.ScrollToCaret();
                    }));*/
                }
                XmlNode idNode = xmlDoc.SelectSingleNode("Root/Id");
                XmlNode resultNode = xmlDoc.SelectSingleNode("Root/Result");
                XmlNode errorNode = xmlDoc.SelectSingleNode("Root/Error");
                string result = string.Empty;
                string topidid = string.Empty;
                if (resultNode != null && idNode != null)
                {
                    topidid = idNode.InnerText;
                    result = resultNode.InnerText;
                    return result;

                    /*   richTextBox1.BeginInvoke(new EventHandler(delegate
                       {
                           richTextBox1.AppendText("题目ID：" + topidid + "\r\n");
                           richTextBox1.AppendText("识别结果：" + result + "\r\n");
                           richTextBox1.Select(richTextBox1.TextLength, richTextBox1.TextLength);
                           richTextBox1.ScrollToCaret();
                       }));*/
                }
                else if (errorNode != null)
                {
                    failureTryCount--;
                    ORC(image, failureTryCount);
                    return null;
                    /*richTextBox1.BeginInvoke(new EventHandler(delegate
                    {
                        richTextBox1.AppendText("识别错误：" + errorNode.InnerText + "\r\n");
                        richTextBox1.Select(richTextBox1.TextLength, richTextBox1.TextLength);
                        richTextBox1.ScrollToCaret();
                    }));*/
                }
                else
                {
                    failureTryCount--;
                    ORC(image, failureTryCount);
                    return null;
                    /*richTextBox1.BeginInvoke(new EventHandler(delegate
                    {
                        richTextBox1.AppendText("未知问题\r\n");
                        richTextBox1.Select(richTextBox1.TextLength, richTextBox1.TextLength);
                        richTextBox1.ScrollToCaret();
                    }));*/
                }
//            }
//            catch (Exception exception)
//            {
//
//                throw exception;
//            }
        }

        /// <summary>
        /// POST 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string PostRequest(string url, string postData)
        {
//            try
//            {
                HttpWebRequest request = null;

                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";

                request.Accept = "*/*;";
                request.UserAgent = "Mozilla/5.0";
                request.ContentType = "application/x-www-form-urlencoded";
                request.AllowAutoRedirect = true;
                request.CookieContainer = _cookies;
                request.KeepAlive = true;

                byte[] postdatabyte = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = postdatabyte.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(postdatabyte, 0, postdatabyte.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //            GlobalManager.Cookie = response.Headers.Get("Set-Cookie");

                string strWebData = string.Empty;
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    strWebData = reader.ReadToEnd();
                }

                return strWebData;
//            }
//            catch (Exception exception)
//            { 
//                throw exception;
//            }
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetRequest(string url)
        {
//            try
//            {
                HttpWebRequest request = null;

                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                request.Accept = "*/*;";
                request.UserAgent = "Mozilla/5.0";
                request.ContentType = "application/x-www-form-urlencoded";
                request.AllowAutoRedirect = true;
                request.CookieContainer = _cookies;
                request.KeepAlive = true;

                /*   string postData = string.Format("tp={0}&yzm={1}&cardnum={2}", tp, yzm, cardnum
               ); //这里按照前面FireBug中查到的POST字符串做相应修改。
               byte[] postdatabyte = Encoding.UTF8.GetBytes(postData);
               request.ContentLength = postdatabyte.Length;

               using (Stream stream = request.GetRequestStream())
               {
                   stream.Write(postdatabyte, 0, postdatabyte.Length);
               }*/

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //            GlobalManager.Cookie = response.Headers.Get("Set-Cookie");

                string strWebData = string.Empty;
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    strWebData = reader.ReadToEnd();
                }


                //            webBrowser1.Navigate(string.Format("https://www.shenzhentong.com/service/fplist_101007009_{0}_{1}.html", textBoxX1CardNum.Text, monthCalendarAdvTransaction.SelectedDate.ToString("yyyyMMdd")));
                return strWebData;
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//                throw e;
//            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="failureTryCount"></param>
        /// <returns></returns>
        public static bool DownloadFile(string url, string path, string fileName, int failureTryCount=3)
        {
            if (failureTryCount==0)
            {
                return false;
            }

            try
            {
                WebClient client = new WebClient();

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                client.DownloadFile(url, path + fileName);
                 
            }
            catch (Exception ex)
            {
                failureTryCount--;
                DownloadFile(url,path,fileName,failureTryCount);
            }

            return true;
        }
    }
}
