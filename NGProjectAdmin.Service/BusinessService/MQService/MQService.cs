
using NGProjectAdmin.Repository.BusinessRepository.MQRepository;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NGProjectAdmin.Service.BusinessService.MQService
{
    /// <summary>
    /// ActiveMQ服务
    /// </summary>
    public class MQService : IMQService
    {
        /// <summary>
        /// ActiveMQ访问层实例
        /// </summary>
        private readonly IMQRepository mqRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mqRepository">管道实例</param>
        public MQService(IMQRepository mqRepository)
        {
            this.mqRepository = mqRepository;
        }

        /// <summary>
        /// 发送Topic
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="topicName">topic别名</param>
        /// <returns></returns>
        public void SendTopic(String message, String topicName = null)
        {
            this.mqRepository.SendTopic(HttpUtility.UrlEncode(message, Encoding.UTF8), topicName);
        }

        /// <summary>
        /// 发送Topic
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="topicName">topic别名</param>
        /// <returns></returns>
        public async Task SendTopicAsync(String message, String topicName = null)
        {
            await this.mqRepository.SendTopicAsync(HttpUtility.UrlEncode(message, Encoding.UTF8), topicName);
        }

        /// <summary>
        /// 发送Queue
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="queueName">queue别名</param>
        /// <returns></returns>
        public void SendQueue(String message, String queueName = null)
        {
            this.mqRepository.SendQueue(HttpUtility.UrlEncode(message, Encoding.UTF8), queueName);
        }

        /// <summary>
        /// 发送Queue
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="queueName">queue别名</param>
        /// <returns></returns>
        public async Task SendQueueAsync(String message, String queueName = null)
        {
            await this.mqRepository.SendQueueAsync(HttpUtility.UrlEncode(message, Encoding.UTF8), queueName);
        }
    }
}
