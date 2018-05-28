using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentYoutuYun.SDK.Csharp
{
    public class Conf
    {
        const string PKG_VERSION = "1.0.*";
        public string YOUTU_END_POINT { get { return "http://api.youtu.qq.com/"; } }

        // 请到 open.youtu.qq.com查看您对应的appid相关信息并填充
        // 请统一 通过 setAppInfo 设置 

        public string APPID { get; set; }
        public string SECRET_ID { get; set; }
        public string SECRET_KEY { get; set; }

        public string END_POINT { get; set; }

        /// <summary>
        /// 开发者 QQ
        /// </summary>
        public string USER_ID { get; set; }

        private static Conf instance = null;

        private Conf() { }

        public static Conf Instance()
        {
            if (instance == null)
            {
                instance = new Conf();
            }

            return instance;
        }

        /// <summary>
        /// 初始化 应用信息
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secretId"></param>
        /// <param name="secretKey"></param>
        /// <param name="userid"></param>
        public void setAppInfo(string appid, string secretId, string secretKey, string userid,string end_point)
        {
            this.APPID = appid;
            this.SECRET_ID = secretId;
            this.SECRET_KEY = secretKey;
            this.USER_ID = userid;
            this.END_POINT = end_point;
        }

    }
}
