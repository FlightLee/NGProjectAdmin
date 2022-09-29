using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// Xml文件操作工具类
    /// </summary>
    public class NGXmlDocumentUtil
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="path">文件路径</param>
        /// <param name="t">泛型对象</param>
        public static void Serialize<T>(String path, T t)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                //序列化
                serializer.Serialize(fs, t);
            }
        }


        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="path">文件路径</param>
        /// <param name="list">泛型集合</param>
        public static void SerializeList<T>(String path, List<T> list)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                //序列化
                serializer.Serialize(fs, list);
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="path">文件路径</param>
        /// <returns>泛型对象</returns>
        public static T Deserialize<T>(String path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                //反序列化
                return (T)serializer.Deserialize(fs);
            }
        }

        /// <summary>
        /// 反序列化集合
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="path">文件路径</param>
        /// <returns>对象集合</returns>
        public static List<T> DeserializeList<T>(String path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                //反序列化
                return (List<T>)serializer.Deserialize(fs);
            }
        }

        /// <summary>
        /// 获取节点集合
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="section">指定节点，以“:”分割</param>
        /// <returns>节点集合</returns>
        public static XmlNodeList GetChildNodes(String path, String section)
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlReaderSettings settings = new XmlReaderSettings();
            //忽略注释
            settings.IgnoreComments = true;
            //忽略空白
            settings.IgnoreWhitespace = true;

            //加载配置文件
            XmlReader reader = XmlReader.Create(path, settings);
            xmlDocument.Load(reader);

            XmlNode xn = null;
            var arr = section.Split(':');
            for (var i = 0; i < arr.Length; i++)
            {
                if (i.Equals(0))
                {
                    xn = xmlDocument.SelectSingleNode(arr[i]);
                }
                else
                {
                    xn = xn.SelectSingleNode(arr[i]);
                }
            }
            XmlNodeList xns = xn.ChildNodes;

            reader.Close();

            return xns;
        }
    }
}
