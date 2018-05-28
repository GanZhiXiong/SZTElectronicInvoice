using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TencentYoutuYun.SDK.Csharp.Common;

namespace TencentYoutuYun.SDK.Csharp
{
    public class Auth
    {
        const string AUTH_URL_FORMAT_ERROR = "-1";
        const string AUTH_SECRET_ID_KEY_ERROR = "-2";

        /// <summary>
        /// HMAC-SHA1 算法签名
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static byte[] HmacSha1Sign(string str, string key)
        {
            byte[] keyBytes = Utility.StrToByteArr(key);
            HMACSHA1 hmac = new HMACSHA1(keyBytes);
            byte[] inputBytes = Utility.StrToByteArr(str);
            return hmac.ComputeHash(inputBytes);
        }
        /// <summary>
        /// 签名串拼接
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="appid"></param>
        /// <param name="secretid"></param>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <returns></returns>
        private static string SetOrignal(string uid, string appid, string secretid, string stime, string etime = "0")
        {
            return string.Format("u={0}&a={1}&k={2}&e={3}&t={4}&r={5}&f={6}", uid, appid, secretid, etime, stime, new Random()
               .Next(0, 1000000000), "");
        }
        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="expired">过期时间</param>
        /// <param name="userid">暂时不用</param>
        /// <returns>签名</returns>
        public static string appSign(string expired, string userid)
        {
            if (string.IsNullOrEmpty(Conf.Instance().SECRET_ID) || string.IsNullOrEmpty(Conf.Instance().SECRET_KEY))
            {
                return AUTH_SECRET_ID_KEY_ERROR;
            }

            string time = Utility.UnixTime();

            string plainText = SetOrignal(Conf.Instance().USER_ID, Conf.Instance().APPID, Conf.Instance().SECRET_ID, time, expired);

            byte[] signByteArrary = Utility.JoinByteArr(HmacSha1Sign(plainText, Conf.Instance().SECRET_KEY), Utility.StrToByteArr(plainText));

            return Convert.ToBase64String(signByteArrary);

        }
    }
}
