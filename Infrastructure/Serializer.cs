using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace IdleMonitor.Infrastructure
{
    /// <summary>
    /// Преобразователь объекта в/из JSON
    /// </summary>
    public class JSONSerializer
    {
        /// <summary>
        /// Десериализует JSON в объект указанного типа
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Сериализует объект в JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj, JsonSerializerSettings settings)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, settings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Сериализует объект в JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    /// <summary>
    /// Преобразователь объекта в/из XML
    /// </summary>
    public class XMLSerializer
    {
        /// <summary>
        /// Десериализует XML в объект указанного типа
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            try
            {
                using (StringReader reader = new StringReader(data))
                {
                    var result = (T)serializer.Deserialize(reader);
                    reader.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Сериализует объект в строку XML
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = Encoding.UTF8
            };

            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings))
                    {
                        XmlSerializer serializer = new XmlSerializer(obj.GetType());
                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                        ns.Add(String.Empty, String.Empty);
                        serializer.Serialize(xmlWriter, obj, ns);
                        memoryStream.Position = 0;
                    }

                    using (StreamReader sr = new StreamReader(memoryStream))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
