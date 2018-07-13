using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SZTElectronicInvoice.Model;

namespace SZTElectronicInvoice
{
    public class GlobalManager
    {
        public static string DomainName = "http://113.140.3.157:16922/";

        public static string DownloadPath = System.Windows.Forms.Application.StartupPath + "/ZhiXiongDownload/";
        public static string MainDownloadPath = System.Windows.Forms.Application.StartupPath + "/ZhiXiongDownload/";

        public static BindingList<ElectronicInvoiceInfo> ElectronicInvoiceInfos = new BindingList<ElectronicInvoiceInfo>();
        //        public static string Cookie { get; set; }

        private static UserConfig _userConfig;

        public static UserConfig UserConfig
        {
            get
            {
                if (_userConfig == null)
                {
                    _userConfig = (UserConfig)DataPersistence.ReadSerializeObject("user.dat"); ;
                }

                if (_userConfig == null)
                {
                    _userConfig = new UserConfig();
                }

                return _userConfig;
            }
            set { _userConfig = value; }
        }

        public static void SaveUserConfig()
        {
            DataPersistence.SaveSerializeObject("user.dat", GlobalManager.UserConfig);
        }

        public static UserConfig ReadUserConfig()
        {
            return (UserConfig)DataPersistence.ReadSerializeObject("user.dat");
        }
    }
}
