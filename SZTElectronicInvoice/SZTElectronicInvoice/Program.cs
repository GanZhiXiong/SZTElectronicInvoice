using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using SZTElectronicInvoice.Common;

namespace SZTElectronicInvoice
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //            MessageBoxEx.UseSystemLocalizedString = true;
            //            MessageBoxEx.EnableGlass = true;
            DevComponents.DotNetBar.LocalizationKeys.LocalizeString +=
                new DevComponents.DotNetBar.DotNetBarManager.LocalizeStringEventHandler(LocalizationKeys_LocalizeString);

            //GZXMessageBox.MessageBoxResult("失败，点击【确定】重试下载失败的发票，");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        #region Donetbar控件英文翻译为中文
        static void LocalizationKeys_LocalizeString(object sender, DevComponents.DotNetBar.LocalizeEventArgs e)
        {
            if (e.Key == DevComponents.DotNetBar.LocalizationKeys.MessageBoxCancelButton)
            {
                e.LocalizedValue = "取消";
                e.Handled = true;
            }
            if (e.Key == DevComponents.DotNetBar.LocalizationKeys.MessageBoxNoButton)
            {
                e.LocalizedValue = "取消";
                e.Handled = true;
            }
            if (e.Key == DevComponents.DotNetBar.LocalizationKeys.MessageBoxOkButton)
            {
                e.LocalizedValue = "确定";
                e.Handled = true;
            }
            if (e.Key == DevComponents.DotNetBar.LocalizationKeys.MessageBoxYesButton)
            {
                e.LocalizedValue = "确定";
                e.Handled = true;
            }
            if (e.Key == DevComponents.DotNetBar.LocalizationKeys.MonthCalendarClearButtonText)
            {
                e.LocalizedValue = "清除";
                e.Handled = true;
            }
            if (e.Key == DevComponents.DotNetBar.LocalizationKeys.MonthCalendarTodayButtonText)
            {
                e.LocalizedValue = "今天";
                e.Handled = true;
            }
            if (e.Key == DevComponents.DotNetBar.LocalizationKeys.TimeSelectorHourLabel)
            {
                e.LocalizedValue = "时";
                e.Handled = true;
            }
            if (e.Key == DevComponents.DotNetBar.LocalizationKeys.TimeSelectorMinuteLabel)
            {
                e.LocalizedValue = "分";
                e.Handled = true;
            }
            if (e.Key == DevComponents.DotNetBar.LocalizationKeys.TimeSelectorClearButton)
            {
                e.LocalizedValue = "清除";
                e.Handled = true;
            }
            if (e.Key == DevComponents.DotNetBar.LocalizationKeys.TimeSelectorOkButton)
            {
                e.LocalizedValue = "确定";
                e.Handled = true;
            }
        }

        #endregion

    }
}
