
using NGProjectAdmin.Common.Enum;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using System;
using System.Threading.Tasks;

namespace NGProjectAdmin.Repository.BusinessRepository.MQRepository
{
    /// <summary>
    /// ActiveMQ访问层实现
    /// </summary>
    public class MQRepository : IMQRepository
    {
        /// <summary>
        /// 发送Topic
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="topicName">topic别名</param>
        /// <returns></returns>
        public void SendTopic(String message, String topicName = null)
        {
            if (NGAdminGlobalContext.MomConfig.MomType == MomType.ActiveMQ)
            {
                NGActiveMQContext.SendTopic(message, topicName);
            }
            else if (NGAdminGlobalContext.MomConfig.MomType == MomType.RabbitMQ)
            {
                NGRabbitMQContext.SendMessage(message);
            }
        }

        /// <summary>
        /// 发送Topic
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="topicName">topic别名</param>
        /// <returns></returns>
        public async Task SendTopicAsync(String message, String topicName = null)
        {
            if (NGAdminGlobalContext.MomConfig.MomType == MomType.ActiveMQ)
            {
                await NGActiveMQContext.SendTopicAsync(message, topicName);
            }
            else if (NGAdminGlobalContext.MomConfig.MomType == MomType.RabbitMQ)
            {
                await NGRabbitMQContext.SendMessageAsynce(message);
            }
        }

        /// <summary>
        /// 发送Queue
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="queueName">queue别名</param>
        /// <returns></returns>
        public void SendQueue(String message, String queueName = null)
        {
            if (NGAdminGlobalContext.MomConfig.MomType == MomType.ActiveMQ)
            {
                NGActiveMQContext.SendQueue(message, queueName);
            }
            else if (NGAdminGlobalContext.MomConfig.MomType == MomType.RabbitMQ)
            {
                NGRabbitMQContext.SendMessage(message);
            }
        }

        /// <summary>
        /// 发送Queue
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="queueName">queue别名</param>
        /// <returns></returns>
        public async Task SendQueueAsync(String message, String queueName = null)
        {
            if (NGAdminGlobalContext.MomConfig.MomType == MomType.ActiveMQ)
            {
                await NGActiveMQContext.SendQueueAsync(message, queueName);
            }
            else if (NGAdminGlobalContext.MomConfig.MomType == MomType.RabbitMQ)
            {
                await NGRabbitMQContext.SendMessageAsynce(message);
            }
        }
    }
}
