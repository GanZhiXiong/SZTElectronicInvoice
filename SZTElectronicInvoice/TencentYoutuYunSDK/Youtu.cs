using System;
using System.Collections.Generic;
using System.Text;
using TencentYoutuYun.SDK.Csharp.Common;

namespace TencentYoutuYun.SDK.Csharp
{
    public class Youtu
    {
        // 30 days
        const double EXPIRED_SECONDS = 2592000;
        const int HTTP_BAD_REQUEST = 400;
        const int HTTP_SERVER_ERROR = 500;

        /// <summary>
        /// return the status message 
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string statusText(int status)
        {
            string statusText = "UNKOWN;status=" + status;

            switch (status)
            {
                case 0:
                statusText = "CONNECT_FAIL";
                break;
                case 200:
                    statusText = "HTTP OK";
                    break;
                case 400:
                    statusText = "BAD_REQUEST";
                    break;
                case 401:
                    statusText = "UNAUTHORIZED";
                    break;
                case 403:
                    statusText = "FORBIDDEN";
                    break;
                case 404:
                    statusText = "NOTFOUND";
                    break;
                case 411:
                    statusText = "REQ_NOLENGTH";
                    break;
                case 423:
                    statusText = "SERVER_NOTFOUND";
                    break;
                case 424:
                    statusText = "METHOD_NOTFOUND";
                    break;
                case 425:
                    statusText = "REQUEST_OVERFLOW";
                    break;
                case 500:
                    statusText = "INTERNAL_SERVER_ERROR";
                    break;
                case 503:
                    statusText = "SERVICE_UNAVAILABLE";
                    break;
                case 504:
                    statusText = "GATEWAY_TIME_OUT";
                    break;
            }
            return statusText;
        }

        #region 接口调用

        #region 人脸检测与分析

        /// <summary>
        /// 人脸检测 detectface
        /// </summary>
        /// <param name="image_path">的路径</param>
        /// <param name="isbigface">是否大脸模式 ０表示检测所有人脸， 1表示只检测照片最大人脸　适合单人照模式</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string detectface(string image_path, int isbigface=1)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/detectface";
            StringBuilder postData = new StringBuilder();
            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\",\"mode\":{2}";
            pars = string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path), isbigface);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }


        /// <summary>
        /// 人脸检测 detectfaceurl
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <param name="isbigface">是否大脸模式 ０表示检测所有人脸， 1表示只检测照片最大人脸　适合单人照模式</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string detectfaceurl(string url,int isbigface = 1)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/detectface";
            StringBuilder postData = new StringBuilder();
            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\",\"mode\":{2}";
            pars = string.Format(pars, Conf.Instance().APPID, url, isbigface);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 五官定位 faceshape
        /// </summary>
        /// <param name="image_path">的路径</param>
        /// <param name="isbigface">是否大脸模式 ０表示检测所有人脸， 1表示只检测照片最大人脸　适合单人照模式</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string faceshape(string image_path,int isbigface=1)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/faceshape";
            StringBuilder postData = new StringBuilder(); 
            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\",\"mode\":{2}";
            pars =string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path), isbigface);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 五官定位 faceshapeurl
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <param name="isbigface">是否大脸模式 ０表示检测所有人脸， 1表示只检测照片最大人脸　适合单人照模式</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string faceshapeurl(string url,int isbigface = 1)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/faceshape";
            StringBuilder postData = new StringBuilder();
            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\",\"mode\":{2}";
            pars = string.Format(pars, Conf.Instance().APPID, url, isbigface);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }


        #endregion

        #region 人脸识别

        /// <summary>
        /// 人脸对比 facecompare
        /// </summary>
        /// <param name="imageA">待比对的A图片数据</param>
        /// <param name="imageB">待比对的B图片数据</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string facecompare(string imageA, string imageB)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/facecompare";
            StringBuilder postData = new StringBuilder(); 
            string pars = "\"app_id\":\"{0}\",\"imageA\":\"{1}\",\"imageB\":\"{2}\"";
            pars =string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(imageA), Utility.ImgBase64(imageB));
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 人脸对比 facecompareurl
        /// </summary>
        /// <param name="urlA">图片的urlA</param>
        /// <param name="urlB">图片的urlB</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string facecompareurl(string urlA, string urlB)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/facecompare";
            StringBuilder postData = new StringBuilder();
            string pars = "\"app_id\":\"{0}\",\"urlA\":\"{1}\",\"urlB\":\"{2}\"";
            pars = string.Format(pars, Conf.Instance().APPID, urlA, urlB);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 人脸验证 faceverify
        /// </summary>
        /// <param name="image_path">待验证图片路径</param>
        /// <param name="person_id">待验证的人脸id</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string faceverify(string image_path, string person_id)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/faceverify";
            StringBuilder postData = new StringBuilder(); 
            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\",\"person_id\":\"{2}\"";
            pars =string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path), person_id);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 人脸验证 faceverifyurl
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <param name="person_id">待验证的人脸id</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string faceverifyurl(string url, string person_id)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/faceverify";
            StringBuilder postData = new StringBuilder();
            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\",\"person_id\":\"{2}\"";
            pars = string.Format(pars, Conf.Instance().APPID, url, person_id);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }


        /// <summary>
        /// 人脸识别 faceidentify
        /// </summary>
        /// <param name="image_path">待识别图片路径</param>
        /// <param name="group_id">识别的组id</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string faceidentify(string image_path, string group_id)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/faceidentify";
            StringBuilder postData = new StringBuilder(); 
            string pars = "\"app_id\":\"{0}\",\"group_id\":\"{1}\",\"image\":\"{2}\"";
            pars =string.Format(pars, Conf.Instance().APPID, group_id, Utility.ImgBase64(image_path));
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 人脸识别 faceidentifyurl
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <param name="group_id">识别的组id</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string faceidentifyurl(string url, string group_id)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/faceidentify";
            StringBuilder postData = new StringBuilder();
            string pars = "\"app_id\":\"{0}\",\"group_id\":\"{1}\",\"url\":\"{2}\"";
            pars = string.Format(pars, Conf.Instance().APPID, group_id, url);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 多人脸检索 multifaceidentify
        /// </summary>
        /// <param name="image_path">待识别图片路径</param>
        /// <param name="group_id">识别的组id</param>
        /// <param name="group_ids">个体存放的组id，可以指定多个组id</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string multifaceidentify(string image_path, string group_id, List<string> group_ids, int topn = 5, int min_size = 40)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/multifaceidentify";
            StringBuilder postData = new StringBuilder();
            string pars = "\"app_id\":\"{0}\",\"group_id\":\"{1}\",\"group_ids\":{2},\"image\":\"{3}\",\"topn\":{4},\"min_size\":{5}";
            pars = string.Format(pars, Conf.Instance().APPID, group_id, JsonHelp<string[]>.ToJsonString(group_ids.ToArray()), Utility.ImgBase64(image_path), topn, min_size);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 多人脸检索 multifaceidentifyurl
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <param name="group_id">识别的组id</param>
        /// <param name="group_ids">个体存放的组id，可以指定多个组id</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string multifaceidentifyurl(string url, string group_id, List<string> group_ids, int topn = 5, int min_size = 40)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/faceidentify";
            StringBuilder postData = new StringBuilder();
            string pars = "\"app_id\":\"{0}\",\"group_id\":\"{1}\",\"group_ids\":{2},\"url\":\"{3}\",\"topn\":{4},\"min_size\":{5}";
            pars = string.Format(pars, Conf.Instance().APPID, group_id, JsonHelp<string[]>.ToJsonString(group_ids.ToArray()), url, topn, min_size);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            Console.WriteLine(postData);
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        #endregion

        #region 个体(Person)管理

        /// <summary>
        /// 个体创建 newperson
        /// </summary>
        /// <param name="image_path">包含个体人脸的图片数据</param>
        /// <param name="person_id">新建的个体id，用户指定，需要保证app_id下的唯一性</param>
        /// <param name="person_name">姓名</param>
        /// <param name="group_ids">新建的个体存放的组id，可以指定多个组id，用户指定（组默认创建）</param>
        /// <param name="person_tag">备注信息，用户自解释字段</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string newperson(string image_path, string person_id, string person_name, List<string> group_ids, string person_tag)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/newperson";
            StringBuilder postData = new StringBuilder(); 
            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\",\"group_ids\":{2},\"person_id\":\"{3}\",\"person_name\":\"{4}\",\"tag\":\"{5}\"";
            pars =string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path), JsonHelp<string[]>.ToJsonString(group_ids.ToArray()), person_id, person_name, person_tag);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 个体创建 newpersonurl
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <param name="person_id">新建的个体id，用户指定，需要保证app_id下的唯一性</param>
        /// <param name="person_name">姓名</param>
        /// <param name="group_ids">新建的个体存放的组id，可以指定多个组id，用户指定（组默认创建）</param>
        /// <param name="person_tag">备注信息，用户自解释字段</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string newpersonurl(string url, string person_id, string person_name, List<string> group_ids, string person_tag)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/newperson";
            StringBuilder postData = new StringBuilder();
            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\",\"group_ids\":{2},\"person_id\":\"{3}\",\"person_name\":\"{4}\",\"tag\":\"{5}\"";
            pars = string.Format(pars, Conf.Instance().APPID, url, JsonHelp<string[]>.ToJsonString(group_ids.ToArray()), person_id, person_name, person_tag);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 删除个体 delperson
        /// </summary>
        /// <param name="person_id">待删除的个体id</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string delperson(string person_id)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/delperson";
            StringBuilder postData = new StringBuilder(); 
            string pars = "\"app_id\":\"{0}\",\"person_id\":\"{1}\"";
            pars =string.Format(pars, Conf.Instance().APPID, person_id);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 增加人脸 addface
        /// </summary>
        /// <param name="person_id">新增人脸的个体身份id</param>
        /// <param name="image_path_arr">待增加的包含人脸的图片数据，可加入多张（包体大小<2m）</param>
        /// <param name="facetag">人脸备注信息，用户自解释字段</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string addface(string person_id, List<string> image_path_arr, string facetag="")
        {
            List<string> faceImgBase64List = new List<string>();

            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/addface";
            StringBuilder postData = new StringBuilder(); 

            image_path_arr.ForEach(path =>
            {
                faceImgBase64List.Add(Utility.ImgBase64(path));
            });

            string pars = "\"app_id\":\"{0}\",\"images\":{1},\"person_id\":\"{2}\",\"tag\":\"{3}\"";

            pars =string.Format(pars, Conf.Instance().APPID, JsonHelp<string[]>.ToJsonString(faceImgBase64List.ToArray()), person_id, facetag);

            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));

            return result;
        }

        /// <summary>
        /// 增加人脸 addface
        /// </summary>
        /// <param name="person_id">新增人脸的个体身份id</param>
        /// <param name="url_arr">待增加的包含人脸的url，可加入多个（包体大小<2m）</param>
        /// <param name="facetag">人脸备注信息，用户自解释字段</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string addfaceurl(string person_id, List<string> url_arr, string facetag = "")
        {
            List<string> faceImgBase64List = new List<string>();

            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/addface";
            StringBuilder postData = new StringBuilder();
            string pars = "\"app_id\":\"{0}\",\"urls\":{1},\"person_id\":\"{2}\",\"tag\":\"{3}\"";

            pars = string.Format(pars, Conf.Instance().APPID, JsonHelp<string[]>.ToJsonString(url_arr.ToArray()), person_id, facetag);

            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));

            return result;
        }

        /// <summary>
        /// 删除人脸 delface
        /// </summary>
        /// <param name="person_id">待删除人脸的个体身份id</param>
        /// <param name="face_ids">删除人脸id的列表</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string delface(string person_id, List<string> face_ids)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/delface";
            StringBuilder postData = new StringBuilder(); 
            string pars = "\"app_id\":\"{0}\",\"person_id\":\"{1}\",\"face_ids\":{2}";
            pars =string.Format(pars, Conf.Instance().APPID, person_id, JsonHelp<string[]>.ToJsonString(face_ids.ToArray()));
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));

            return result;
        }
        /// <summary>
        /// 设置信息 setinfo
        /// </summary>
        /// <param name="person_id">待设置的个体身份id</param>
        /// <param name="person_name">新设置的个体名字</param>
        /// <param name="person_tag">新设置的人备注信息</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string setinfo(string person_id, string person_name, string person_tag)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/setinfo";
            StringBuilder postData = new StringBuilder(); 
            string pars = "\"app_id\":\"{0}\",\"person_name\":\"{1}\",\"person_id\":\"{2}\",\"tag\":\"{3}\"";
            pars =string.Format(pars, Conf.Instance().APPID, person_name, person_id, person_tag);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }
        /// <summary>
        /// 获取信息 getinfo
        /// </summary>
        /// <param name="person_id">待查询的个体身份id</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string getinfo(string person_id)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/getinfo";
            StringBuilder postData = new StringBuilder(); 
            string pars = "\"app_id\":\"{0}\",\"person_id\":\"{1}\"";
            pars =string.Format(pars, Conf.Instance().APPID, person_id);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        #endregion

        #region 信息查询
        /// <summary>
        /// 获取组列表 getgroupids
        /// </summary>
        /// <returns>返回的组列表查询结果，JSON字符串，字段参见API文档</returns>
        public static string getgroupids()
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/getgroupids";
            StringBuilder postData = new StringBuilder(); 
            string pars = "\"app_id\":\"{0}\"";
            pars =string.Format(pars, Conf.Instance().APPID);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }
        /// <summary>
        /// 获取人列表 getpersonids
        /// </summary>
        /// <param name="group_id">待查询的组id</param>
        /// <returns>返回的个体列表查询结果，JSON字符串，字段参见API文档</returns>
        public static string getpersonids(string group_id)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/getpersonids";
            StringBuilder postData = new StringBuilder(); 
            string pars = "\"app_id\":\"{0}\",\"group_id\":\"{1}\"";
            pars =string.Format(pars, Conf.Instance().APPID, group_id);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }
        /// <summary>
        /// 获取人脸列表 getfaceids
        /// </summary>
        /// <param name="person_id">待查询的个体id</param>
        /// <returns>返回的人脸列表查询结果，JSON字符串，字段参见API文档</returns>
        public static string getfaceids(string person_id)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/getfaceids";
            StringBuilder postData = new StringBuilder(); 
            string pars = "\"app_id\":\"{0}\",\"person_id\":\"{1}\"";
            pars =string.Format(pars, Conf.Instance().APPID, person_id);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }
        /// <summary>
        /// 获取人脸信息 getfaceinfo
        /// </summary>
        /// <param name="face_id">待查询的人脸id</param>
        /// <returns>返回的人脸信息查询结果，JSON字符串，字段参见API文档</returns>
        public static string getfaceinfo(string face_id)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/api/getfaceinfo";
            StringBuilder postData = new StringBuilder(); 
            string pars = "\"app_id\":\"{0}\",\"face_id\":\"{1}\"";
            pars =string.Format(pars, Conf.Instance().APPID, face_id);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        #endregion

        #region 图像识别服务

        /// <summary>
        /// 判断一个图像的模糊程度
        /// </summary>
        /// <param name="image_path">图片路径</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string fuzzydetect(string image_path)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/imageapi/fuzzydetect";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path));
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 判断一个图像的模糊程度
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string fuzzydetecturl(string url)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/imageapi/fuzzydetect";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, url);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 识别一个图像是否为美食图像
        /// </summary>
        /// <param name="image_path">图片路径</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string fooddetect(string image_path)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/imageapi/fooddetect";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path));
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 识别一个图像是否为美食图像
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string fooddetecturl(string url)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/imageapi/fooddetect";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, url);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 识别一个图像的标签信息,对图像分类
        /// </summary>
        /// <param name="image_path">图片路径</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string imagetag(string image_path)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/imageapi/imagetag";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path));
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 识别一个图像的标签信息,对图像分类
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string imagetagurl(string url)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/imageapi/imagetag";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, url);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 识别一个图像是否为色情图像
        /// </summary>
        /// <param name="image_path">图片路径</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string imageporn(string image_path)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/imageapi/imageporn";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path));
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 识别一个图像是否为色情图像
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string imagepornurl(string url)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/imageapi/imageporn";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, url);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 识别一个图像是否为暴恐图像
        /// </summary>
        /// <param name="image_path">图片路径</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string imageterrorism(string image_path)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/imageapi/imageterrorism";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path));
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 识别一个图像是否为暴恐图像
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string imageterrorismurl(string url)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/imageapi/imageterrorism";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, url);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 自动地检测图片车身以及识别车辆属性
        /// </summary>
        /// <param name="image_path">图片路径</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string carcalssify(string image_path)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/carapi/carclassify";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path));
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 自动地检测图片车身以及识别车辆属性
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string carcalssifyurl(string url)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/carapi/carclassify";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, url);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        #endregion

        #region OCR

        /// <summary>
        /// 根据用户上传的包含身份证正反面照片，识别并且获取证件姓名、性别、民族、出生日期、地址、身份证号、证件有效期、发证机关等详细的身份证信息，并且可以返回精确剪裁对齐后的身份证正反面图片
        /// </summary>
        /// <param name="image_path">图片路径</param>
        /// <param name="card_type">身份证图片类型，0-正面，1-反面</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string idcardocr(string image_path,int card_type)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/ocrapi/idcardocr";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\",\"card_type\":{2}";
            pars = string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path),card_type);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 根据用户上传的包含身份证正反面照片，识别并且获取证件姓名、性别、民族、出生日期、地址、身份证号、证件有效期、发证机关等详细的身份证信息，并且可以返回精确剪裁对齐后的身份证正反面图片
        /// </summary>
        /// <param name="url">图片的url</param>
        ///<param name="card_type">身份证图片类型，0-正面，1-反面</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string idcardocrurl(string url,int card_type)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/ocrapi/idcardocr";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\",\"card_type\":{2}";
            pars = string.Format(pars, Conf.Instance().APPID,url,card_type);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 名片OCR识别
        /// </summary>
        /// <param name="url">图片的url</param>
        ///<param name="retimage ">是否需要返回处理结果图,true 返回，false 不返回</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string namecardocrurl(string url, bool retimage)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/ocrapi/namecardocr";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\",\"retimage\":\"{2}\"";
            pars = string.Format(pars, Conf.Instance().APPID, url, retimage);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 根据用户上传的行驶证，驾驶证照片，识别并且获取相应的详细信息
        /// </summary>
        /// <param name="image_path">图片路径</param>
        /// <param name="type">识别类型，0-行驶证，1-驾驶证</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string driverlicenseocr(string image_path, int type)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/ocrapi/driverlicenseocr";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\",\"type\":{2}";
            pars = string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path), type);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 根据用户上传的行驶证，驾驶证照片，识别并且获取相应的详细信息
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <param name="type">图片类型，0-行驶证，1-驾驶证</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string driverlicenseocrurl(string url, int type)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/ocrapi/driverlicenseocr";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\",\"type\":{2}";
            pars = string.Format(pars, Conf.Instance().APPID, url, type);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 根据用户上传的名片照片，识别并且获取相应的详细信息
        /// </summary>
        /// <param name="image_path">图片路径</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string bcocr(string image_path)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/ocrapi/bcocr";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path));
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 根据用户上传的名片照片，识别并且获取相应的详细信息
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string bcocrurl(string url)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/ocrapi/bcocr";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, url);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 根据用户上传的照片，识别并且获取图片中的文字信息
        /// </summary>
        /// <param name="image_path">图片路径</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string generalocr(string image_path)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/ocrapi/generalocr";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path));
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 根据用户上传的照片，识别并且获取图片中的文字信息
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string generalocrurl(string url)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/ocrapi/generalocr";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, url);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 银行卡OCR识别，根据用户上传的银行卡图像，返回识别出的银行卡字段信息。
        /// </summary>
        /// <param name="image_path">图片路径</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string creditcardocr(string image_path)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/ocrapi/creditcardocr";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path));
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 银行卡OCR识别，根据用户上传的银行卡图像，返回识别出的银行卡字段信息。
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string creditcardocrurl(string url)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/ocrapi/creditcardocr";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, url);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 营业执照OCR 识别，根据用户上传的营业执照图像，返回识别出的注册号、公司名称、地址字段信息
        /// </summary>
        /// <param name="image_path">图片路径</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string bizlicenseocr(string image_path)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/ocrapi/bizlicenseocr";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path));
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 营业执照OCR 识别，根据用户上传的营业执照图像，返回识别出的注册号、公司名称、地址字段信息
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string bizlicenseocrurl(string url)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/ocrapi/bizlicenseocr";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, url);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 车牌OCR识别，根据用户上传的图像，返回识别出的图片中的车牌号。
        /// </summary>
        /// <param name="image_path">图片路径</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string plateocr(string image_path)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/ocrapi/plateocr";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"image\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, Utility.ImgBase64(image_path));
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        /// <summary>
        /// 车牌OCR识别，根据用户上传的图像，返回识别出的图片中的车牌号。
        /// </summary>
        /// <param name="url">图片的url</param>
        /// <returns>返回的结果，JSON字符串，字段参见API文档</returns>
        public static string plateocrurl(string url)
        {
            string expired = Utility.UnixTime(EXPIRED_SECONDS);
            string methodName = "youtu/ocrapi/plateocr";
            StringBuilder postData = new StringBuilder();

            string pars = "\"app_id\":\"{0}\",\"url\":\"{1}\"";
            pars = string.Format(pars, Conf.Instance().APPID, url);
            postData.Append("{");
            postData.Append(pars);
            postData.Append("}");
            string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));
            return result;
        }

        #endregion

        #endregion

    }
}

