using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace SZTElectronicInvoice.Common
{
    public class GZXMessageBox
    {
        public static void Show(string text, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            MessageBoxEx.EnableGlass = true;
            string caption = null;
            switch (icon)
            {
                case MessageBoxIcon.Information:
                    caption = "提示";
                    break;
                case MessageBoxIcon.Error:
                    caption = "错误";
                    break;
                case MessageBoxIcon.Warning:
                    caption = "警告";
                    break;
            }
            if (icon != MessageBoxIcon.Information)
            {
                MessageBoxEx.MessageBoxTextColor = Color.Yellow;
            }
            else
            {
                MessageBoxEx.MessageBoxTextColor = ColorTranslator.FromHtml("#007acc");
            }
            MessageBoxEx.Show("<font size=\"19\">" + text + "</font>", caption, MessageBoxButtons.OK, icon);
        }

        public static bool MessageBoxResult(string text)
        {
            //                MessageBoxEx.MessageBoxTextColor = Color.Crimson;
            if (DialogResult.OK == MessageBoxEx.Show("<font size=\"19\" color=\"red\">" + text + "</font>", "警告",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
            {
                //                    MessageBoxEx.MessageBoxTextColor = Color.White;

                return true;
            }
            else
            {
                //                    MessageBoxEx.MessageBoxTextColor = Color.White;

                return false;
            }
        }
    }
}
