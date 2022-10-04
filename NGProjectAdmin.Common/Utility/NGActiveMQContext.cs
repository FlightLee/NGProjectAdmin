using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using NGProjectAdmin.Common.Global;
using System;
using System.Threading.Tasks;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// ActiveMQ工具类
    /// </summary>
    public class NGActiveMQContext
    {
        /// <summary>
        /// 发送Topic
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="topicName">topic别名</param>
        /// <returns></returns>
        public static void SendTopic(String message, String topicName = null)
        {
            SendMessage("topic", message, topicName);
        }

        /// <summary>
        /// 发送Topic
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="topicName">topic别名</param>
        /// <returns></returns>
        public static async Task SendTopicAsync(String message, String topicName = null)
        {
            await SendMessageAsync("topic", message, topicName);
        }

        /// <summary>
        /// 发送Queue
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="queueName">queue别名</param>
        /// <returns></returns>
        public static void SendQueue(String message, String queueName = null)
        {
            SendMessage("queue", message, queueName);
        }
        /// <summary>
        /// 发送Queue
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="queueName">queue别名</param>
        /// <returns></returns>
        public static async Task SendQueueAsync(String message, String queueName = null)
        {
            await SendMessageAsync("queue", message, queueName);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="message">消息</param>
        /// <param name="tqName">别名</param>
        public static void SendMessage(String type, String message, String tqName = null)
        {
            Send(type, message, tqName);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="message">消息</param>
        /// <param name="tqName">别名</param>
        public static async Task SendMessageAsync(String type, String message, String tqName = null)
        {
            await Task.Run(() => { Send(type, message, tqName); });
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="message">消息</param>
        /// <param name="tqName">别名</param>
        private static void Send(String type, String message, String tqName = null)
        {
            IConnectionFactory _factory = new ConnectionFactory(new Uri(NGAdminGlobalContext.ActiveMQConfig.ConnectionString));
            using (IConnection _conn = _factory.CreateConnection())
            {
                using (ISession _session = _conn.CreateSession())
                {
                    if (type.Equals("queue"))
                    {
                        var queueName = NGAdminGlobalContext.ActiveMQConfig.QueueName;
                        if (!String.IsNullOrEmpty(tqName))
                        {
                            queueName += tqName;
                        }
                        using (IMessageProducer producer = _session.CreateProducer(new ActiveMQQueue(queueName)))
                        {
                            ITextMessage request = _session.CreateTextMessage(message);
                            producer.Send(request);
                        }
                    }
                    else if (type.Equals("topic"))
                    {
                        var topicName = NGAdminGlobalContext.ActiveMQConfig.TopicName;
                        if (!String.IsNullOrEmpty(tqName))
                        {
                            topicName += tqName;
                        }
                        using (IMessageProducer producer = _session.CreateProducer(new ActiveMQTopic(topicName)))
                        {
                            ITextMessage request = _session.CreateTextMessage(message);
                            producer.Send(request);
                        }
                    }
                }
            }
        }
    }
}
