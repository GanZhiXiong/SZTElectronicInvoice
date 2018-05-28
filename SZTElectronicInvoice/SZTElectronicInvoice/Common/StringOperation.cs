using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class StringOperation
{
    /// <summary>
    /// 去掉数字后面的0
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string ClearNumberZero(string num)
    {
        if (num == null)
        {
            return "";
        }
        return num.Contains(".") ? num.TrimEnd('0', '.') : num;
    }

    /// <summary>
    /// 得到指定两个字符串之间的字符串
    /// </summary>
    /// <param name="str"></param>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static string GetBetweenStr(string str, string first, string second)
    {
        Regex rg = new Regex("(?<=(" + first + "))[.\\s\\S]*?(?=(" + second + "))", RegexOptions.Multiline | RegexOptions.Singleline);
        return rg.Match(str).Value;
    }

    /// <summary>
    /// 得到正确文件名格式
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string GetRightFileName(string str)
    {
        string reg = @"\:" + @"|\;" + @"|\/" + @"|\\" + @"|\|" + @"|\," + @"|\*" + @"|\?" + @"|\""" + @"|\<" + @"|\>";//特殊字符
        Regex r = new Regex(reg);
        return r.Replace(str, "");//将特殊字符替换为"" 
    }

    /// <summary>
    /// 获取两个字符串中的内容
    /// </summary>
    /// <param name="strHtm">HTML源代码</param>
    /// <param name="strtop">top标签</param>
    /// <param name="strEnd">end标签</param>
    /// <returns>查询结果文本值</returns>
    public static string GetMiddleText(string strContent, string strStart, string strEnd)
    {
        string texts = "";
        string strRegex = "(?<=" + strStart + ").*?(?=" + strEnd + ")";
        texts = Regex.Match(strContent, strRegex, RegexOptions.Singleline).ToString();
        return texts;
        /*   Regex rg = new Regex("(?<=(" + strStart + "))[.\\s\\S]*?(?=(" + strEnd + "))", RegexOptions.Multiline | RegexOptions.Singleline);
           return rg.Match(strContent).Value;*/
    }

    public static string WordCapWords(string str)
    {
        return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
    }

    public static string SentenceCapWords(string str)
    {
        str = str.ToLower();
        if (str.Length > 1)
        {
            return str[0].ToString().ToUpper() + str.Substring(1);
        }
        else
        {
            return str.ToUpper();
        }
    }
}
