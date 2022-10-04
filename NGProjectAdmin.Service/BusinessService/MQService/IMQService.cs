
using System;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.MQService
{
    /// <summary>
    /// ActiveMQ服务接口
    /// </summary>
    public interface IMQService
    {
        /// <summary>
        /// 发送Topic
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="topicName">topic别名</param>
        /// <returns></returns>
        void SendTopic(String message, String topicName = null);

        /// <summary>
        /// 发送Topic
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="topicName">topic别名</param>
        /// <returns></returns>
        Task SendTopicAsync(String message, String topicName = null);

        /// <summary>
        /// 发送Queue
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="queueName">queue别名</param>
        /// <returns></returns>
        void SendQueue(String message, String queueName = null);

        /// <summary>
        /// 发送Queue
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="queueName">queue别名</param>
        /// <returns></returns>
        Task SendQueueAsync(String message, String queueName = null);
    }
}
