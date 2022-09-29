using Confluent.Kafka;
using Newtonsoft.Json;
using NGProjectAdmin.Common.Global;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// Kafka工具类
    /// </summary>
    public class NGKafkaContext
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="topicName">主题</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public static async Task PublishAsync<TMessage>(string topicName, TMessage message) where TMessage : class
        {
            var config = new ProducerConfig
            {
                BootstrapServers = NGAdminGlobalContext.KafkaConfig.BootstrapServers
            };

            using var producer = new ProducerBuilder<string, string>(config).Build();

            await producer.ProduceAsync(topicName, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = HttpUtility.UrlEncode(JsonConvert.SerializeObject(message), Encoding.UTF8)
            });
        }

        /// <summary>
        /// 订阅kafka
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="topics">主题</param>
        /// <param name="messageFunc">回调函数</param>
        /// <param name="cancellationToken">取消口令</param>
        /// <returns></returns>
        public static async Task SubscribeAsync<TMessage>(IEnumerable<string> topics, Action<TMessage> messageFunc, CancellationToken cancellationToken) where TMessage : class
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = NGAdminGlobalContext.KafkaConfig.BootstrapServers,
                GroupId = NGAdminGlobalContext.KafkaConfig.GroupId,
                EnableAutoCommit = false,
                StatisticsIntervalMs = NGAdminGlobalContext.KafkaConfig.StatisticsIntervalMs,
                SessionTimeoutMs = NGAdminGlobalContext.KafkaConfig.SessionTimeoutMs,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config)
                       .SetErrorHandler((_, e) =>
                       {
                           Console.WriteLine($"Error: {e.Reason}");
                       })
                       .SetStatisticsHandler((_, json) =>
                       {
                           Console.WriteLine($" - {DateTime.Now:yyyy-MM-dd HH:mm:ss} > 消息监听中..");
                       })
                       .SetPartitionsAssignedHandler((c, partitions) =>
                       {
                           string partitionsStr = string.Join(", ", partitions);
                           Console.WriteLine($" - 分配的 kafka 分区: {partitionsStr}");
                       })
                       .SetPartitionsRevokedHandler((c, partitions) =>
                       {
                           string partitionsStr = string.Join(", ", partitions);
                           Console.WriteLine($" - 回收了 kafka 的分区: {partitionsStr}");
                       })
                       .Build();
            consumer.Subscribe(topics);
            try
            {
                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        Console.WriteLine($"Consumed message '{consumeResult.Message?.Value}' at: '{consumeResult?.TopicPartitionOffset}'.");
                        if (consumeResult.IsPartitionEOF)
                        {
                            Console.WriteLine($" - {DateTime.Now:yyyy-MM-dd HH:mm:ss} 已经到底了：{consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");
                            continue;
                        }
                        TMessage messageResult = null;
                        try
                        {
                            messageResult = JsonConvert.DeserializeObject<TMessage>(consumeResult.Message.Value);
                        }
                        catch (Exception ex)
                        {
                            var errorMessage = $" - {DateTime.Now:yyyy-MM-dd HH:mm:ss}【Exception 消息反序列化失败，Value：{consumeResult.Message.Value}】 ：{ex.StackTrace?.ToString()}";
                            Console.WriteLine(errorMessage);
                            messageResult = null;
                        }
                        if (messageResult != null/* && consumeResult.Offset % commitPeriod == 0*/)
                        {
                            messageFunc(messageResult);
                            try
                            {
                                consumer.Commit(consumeResult);
                            }
                            catch (KafkaException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Consume error: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Closing consumer.");
                consumer.Close();
            }

            await Task.CompletedTask;
        }

        //public static void SubscribeAsync()
        //{
        //    var cts = new CancellationTokenSource();
        //    Console.CancelKeyPress += (_, e) =>
        //    {
        //        e.Cancel = true;
        //        cts.Cancel();
        //    };

        //    await kafkaService.SubscribeAsync<XxxEventData>(topics, async (eventData) =>
        //    {
        //        // Your logic

        //        Console.WriteLine($" - {eventData.EventTime:yyyy-MM-dd HH:mm:ss} 【{eventData.TopicName}】- > 已处理");
        //    }, cts.Token);
        //}
    }
}
