using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace My_Notes3
{
    public class MYStorage
    {
        public static void WriteXml<T>(T data, string file)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T)); //
            FileStream stream;
            stream = new FileStream(file, FileMode.Create);
            serializer.Serialize(stream, data);
            stream.Close();
        }

        public static T ReadXml<T>(string file)
        {
            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(sr);
                }
            }
            catch // (Exception)
            {

                return default(T);
            }
        }
    }
}
