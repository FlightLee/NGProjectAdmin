
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// Json文件操作工具类
    /// </summary>
    public class NGJsonDocumentUtil
    {
        /// <summary>
        /// 读取节点对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="section">节点名称</param>
        /// <param name="path">文件路径</param>
        /// <returns>泛型对象</returns>
        public T Read<T>(String section, String path)
        {
            try
            {
                using (var file = new StreamReader(path))
                {
                    using (var reader = new JsonTextReader(file))
                    {
                        var jObj = (JObject)JToken.ReadFrom(reader);
                        if (!String.IsNullOrWhiteSpace(section))
                        {
                            var secObj = jObj[section];
                            if (secObj != null)
                            {
                                return JsonConvert.DeserializeObject<T>(secObj.ToString());
                            }
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default(T);
        }

        /// <summary>
        /// 读取节点返回集合
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="section">节点名称</param>
        /// <param name="path">文件路径</param>
        /// <returns>泛型集合</returns>
        public List<T> ReadList<T>(String section, String path)
        {
            try
            {
                using (var file = new StreamReader(path))
                {
                    using (var reader = new JsonTextReader(file))
                    {
                        var jObj = (JObject)JToken.ReadFrom(reader);
                        if (!String.IsNullOrWhiteSpace(section))
                        {
                            var secObj = jObj[section];
                            if (secObj != null)
                            {
                                return JsonConvert.DeserializeObject<List<T>>(secObj.ToString());
                            }
                        }
                        else
                        {
                            return default(List<T>);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default(List<T>);
        }

        /// <summary>
        /// 写入指定节点
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="section">节点名称</param>
        /// <param name="t">泛型对象</param>
        /// <param name="path">文件路径</param>
        public void Write<T>(String section, T t, String path)
        {
            try
            {
                JObject jObj;
                using (StreamReader file = new StreamReader(path))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        jObj = (JObject)JToken.ReadFrom(reader);
                        if (!String.IsNullOrWhiteSpace(section))
                        {
                            var json = JsonConvert.SerializeObject(t);
                            jObj[section] = JObject.Parse(json);
                        }
                    }
                }

                using (var writer = new StreamWriter(path))
                {
                    using (var jsonWriter = new JsonTextWriter(writer))
                    {
                        jObj.WriteTo(jsonWriter);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 删除指定节点
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="path">文件路径</param>
        public void Remove(String section, String path)
        {
            try
            {
                JObject jObj;
                using (StreamReader file = new StreamReader(path))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        jObj = (JObject)JToken.ReadFrom(reader);
                        if (!String.IsNullOrWhiteSpace(section))
                        {
                            var secObj = jObj[section];
                            if (secObj != null)
                            {
                                jObj.Remove(section);
                            }
                        }
                    }
                }

                using (var writer = new StreamWriter(path))
                {
                    using (var jsonWriter = new JsonTextWriter(writer))
                    {
                        jObj.WriteTo(jsonWriter);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
