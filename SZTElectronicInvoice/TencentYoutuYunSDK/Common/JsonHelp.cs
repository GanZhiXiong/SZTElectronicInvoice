using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TencentYoutuYun.SDK.Csharp.Common
{
    public class JsonHelp<T>
    {
        /// <summary>
        /// 将类型实例转换为Json字符串
        /// </summary>
        /// <param name="t">类型实例</param>
        /// <returns></returns>
        public static string ToJsonString(T t)
        {
            IsoDateTimeConverter a = new IsoDateTimeConverter();
            string result = JsonConvert.SerializeObject(t, a);
            return result;
        }

        /// <summary>
        /// 将Json字符串转换为类型实例
        /// </summary>
        /// <param name="jsonstring">字符串</param>
        /// <returns>类型</returns>
        public static T FromJsonString(string jsonstring)
        {
            T t = JsonConvert.DeserializeObject<T>(jsonstring);
            return t;
        }

        /// <summary>
        /// 将Json字符串转换为类型实例
        /// </summary>
        /// <param name="jsonstring">字符串</param>
        /// <returns>类型</returns>
        public static T FromJsonStringToDataTable(string jsonstring)
        {
            var regex = new Regex(@",?""[_\w]+"":null");
            var nullless = regex.Replace(jsonstring, string.Empty);
            T t = JsonConvert.DeserializeObject<T>(nullless);
            return t;
        }


        /// <summary>
        /// 将List转换为字符串
        /// </summary>
        /// <param name="tlist">数据集</param>
        /// <returns>字符串</returns>
        public static string ToJsonString(List<T> tlist)
        {
            IsoDateTimeConverter a = new IsoDateTimeConverter();
            string result = JsonConvert.SerializeObject(tlist, Formatting.Indented, a);
            return result;
        }

        /// <summary>
        /// 将Json字符串转换为类型List集合
        /// </summary>
        /// <param name="jsonstring">字符串</param>
        /// <returns>类型集合</returns>
        public static List<T> GetList(string jsonstring)
        {           
            List<T> list = JsonConvert.DeserializeObject<List<T>>(jsonstring);
            return list;
        }


        /// <summary>  
        /// 对象转换为Json字符串  
        /// </summary>  
        /// <param name="jsonObject">对象</param>  
        /// <returns>Json字符串</returns>  
        public static string ToJson(object jsonObject)
        {
            string jsonString = "{";
            PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
                string value = string.Empty;
                if (objectValue is DateTime || objectValue is Guid || objectValue is TimeSpan)
                {
                    value = "'" + objectValue.ToString() + "'";
                }
                else if (objectValue is string)
                {
                    value = "'" + ToJson(objectValue.ToString()) + "'";
                }
                else if (objectValue is IEnumerable)
                {
                    value = ToJson((IEnumerable)objectValue);
                }
                else
                {
                    value = ToJson(objectValue.ToString());
                }
                jsonString += "\"" + ToJson(propertyInfo[i].Name) + "\":" + value + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "}";
        }

        /// <summary>  
        /// 对象集合转换Json  
        /// </summary>  
        /// <param name="array">集合对象</param>  
        /// <returns>Json字符串</returns>  
        public static string ToJson(IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString += ToJson(item) + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "]";
        }

        /// <summary>  
        /// 普通集合转换Json  
        /// </summary>  
        /// <param name="array">集合对象</param>  
        /// <returns>Json字符串</returns>  
        public static string ToArrayString(IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString = ToJson(item.ToString()) + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "]";
        }

        /// <summary>  
        /// Datatable转换为Json  
        /// </summary>  
        /// <param name="table">Datatable对象</param>  
        /// <returns>Json字符串</returns>  
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }

        /// <summary> 
        /// DataTable转成Json  
        /// </summary> 
        /// <param name="jsonName"></param> 
        /// <param name="dt"></param> 
        /// <returns></returns> 
        public static string ToJson(DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = dt.TableName;
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        /// <summary>  
        /// DataReader转换为Json  
        /// </summary>  
        /// <param name="dataReader">DataReader对象</param>  
        /// <returns>Json字符串</returns>  
        public static string ToJson(DbDataReader dataReader)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            while (dataReader.Read())
            {
                jsonString.Append("{");
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Type type = dataReader.GetFieldType(i);
                    string strKey = dataReader.GetName(i);
                    string strValue = dataReader[i].ToString();
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (i < dataReader.FieldCount - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            dataReader.Close();
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }

        /// <summary>  
        /// DataSet转换为Json  
        /// </summary>  
        /// <param name="dataSet">DataSet对象</param>  
        /// <returns>Json字符串</returns>  
        public static string ToJson(DataSet dataSet)
        {
            string jsonString = "{";
            foreach (DataTable table in dataSet.Tables)
            {
                jsonString += "\"" + table.TableName + "\":" + ToJson(table) + ",";
            }
            jsonString = jsonString.TrimEnd(',');
            return jsonString + "}";
        }

        public String string2Json(String s)
        {
            StringBuilder sb = new StringBuilder(s.Length + 20);

            sb.Append("/\"");
            for (int i = 0; i < s.Length; i++)
            {
                string c = s[i].ToString();
                switch (c)
                {
                    case "/\"":
                        sb.Append("///\"");
                        break;
                    case "//":
                        sb.Append("////");
                        break;
                    case "/":
                        sb.Append("///");
                        break;
                    case "/b":
                        sb.Append("//b");
                        break;
                    case "/f":
                        sb.Append("//f");
                        break;
                    case "/n":
                        sb.Append("//n");
                        break;
                    case "/r":
                        sb.Append("//r");
                        break;
                    case "/t":
                        sb.Append("//t");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            sb.Append("/\"");
            return sb.ToString();
        }
        /// <summary> 
        /// 格式化字符型、日期型、布尔型 
        /// </summary> 
        /// <param name="str"></param> 
        /// <param name="type"></param> 
        /// <returns></returns> 
        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                //str = String2Json(str); 
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            return str;
        }

    }
}
