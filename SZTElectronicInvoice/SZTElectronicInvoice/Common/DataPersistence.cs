using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SZTElectronicInvoice
{
    public class DataPersistence
    {
        public static Object txtLock = new object();

        public static void SaveSerializeObject(string path, object obj)
        {
            lock (txtLock)
            {
                //序列化
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
                fs.Close();
            }
        }

        public static object ReadSerializeObject(string path)
        {
            //            JavaScriptSerializer j = new JavaScriptSerializer();
            //            return JsonConvert.DeserializeObject(File.ReadAllText(path));
            if (!File.Exists(path))
            {
                return null;
            }

            object obj = null;
            FileStream fs = null;
            try
            {
                //反序列化
                fs = new FileStream(path, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Binder = new UBinder();
                obj = bf.Deserialize(fs);
            }
            catch (Exception e)
            {
                //                return null;
            }
            fs.Close();
            //}
            return obj;
        }

        public class UBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                Assembly ass = Assembly.GetExecutingAssembly();
                return ass.GetType(typeName);
            }
        }
    }
}
