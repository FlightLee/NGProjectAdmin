using RabbitMQ.Client;
using NGProjectAdmin.Common.Global;
using System;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// RabbitMQ工具类
    /// </summary>
    public class NGRabbitMQContext
    {
        /// <summary>
        /// 私有化构造函数
        /// 用于单例模式
        /// </summary>
        private NGRabbitMQContext() { }

        /// <summary>
        /// Lazy对象
        /// </summary>
        private static readonly Lazy<IConnection> LazyConnection = new Lazy<IConnection>(() =>
        {
            ConnectionFactory factory = null;
            IConnection connection = null;

            #region 初始工厂类

            if (NGAdminGlobalContext.RabbitMQConfig.HostName.Contains(","))
            {
                //创建连接对象工厂
                factory = new ConnectionFactory()
                {
                    UserName = NGAdminGlobalContext.RabbitMQConfig.UserName,
                    Password = NGAdminGlobalContext.RabbitMQConfig.Password,
                    AutomaticRecoveryEnabled = true,//如果connection挂掉是否重新连接
                    TopologyRecoveryEnabled = true//连接恢复后，连接的交换机，队列等是否一同恢复
                };
                //创建连接对象
                connection = factory.CreateConnection(NGAdminGlobalContext.RabbitMQConfig.HostName.Split(','));
            }
            else
            {
                //创建连接对象工厂
                factory = new ConnectionFactory()
                {
                    UserName = NGAdminGlobalContext.RabbitMQConfig.UserName,
                    Password = NGAdminGlobalContext.RabbitMQConfig.Password,
                    HostName = NGAdminGlobalContext.RabbitMQConfig.HostName,
                    Port = NGAdminGlobalContext.RabbitMQConfig.Port
                };
                //创建连接对象
                connection = factory.CreateConnection();
            }

            #endregion

            return connection;
        });

        /// <summary>
        /// 单例对象
        /// </summary>
        public static IConnection ConnectionInstance { get { return LazyConnection.Value; } }

        /// <summary>
        /// 是否已创建
        /// </summary>
        public static bool IsConnectionInstanceCreated { get { return LazyConnection.IsValueCreated; } }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息体</param>
        public static void SendMessage(String message)
        {
            IConnection connection = ConnectionInstance;
            //定义通道
            var channel = connection.CreateModel();
            //定义交换机
            channel.ExchangeDeclare(NGAdminGlobalContext.RabbitMQConfig.ExchangeName, ExchangeType.Fanout, true, false);
            //定义队列
            channel.QueueDeclare(NGAdminGlobalContext.RabbitMQConfig.QueueName, false, false, false, null);
            //将队列绑定到交换机
            channel.QueueBind(NGAdminGlobalContext.RabbitMQConfig.QueueName, NGAdminGlobalContext.RabbitMQConfig.ExchangeName, "", null);
            //发布消息
            channel.BasicPublish(NGAdminGlobalContext.RabbitMQConfig.ExchangeName, "", null, Encoding.Default.GetBytes(message));
        }

        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="message">消息体</param>
        /// <returns></returns>
        public static async Task SendMessageAsynce(String message)
        {
            await Task.Run(() => SendMessage(message));
        }
    }
}
