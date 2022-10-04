using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NGProjectAdmin.Repository.BusinessRepository.RedisRepository
{
    /// <summary>
    /// Redis访问层接口
    /// </summary>
    public interface IRedisRepository
    {
        /// <summary>
        /// 依据Key获取字符串
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>字符串</returns>
        string Get(string key);

        /// <summary>
        /// 依据Key获取字符串
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>字符串</returns>
        Task<string> GetAsync(string key);

        /// <summary>
        /// 缓存对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="obj">对象</param>
        /// <param name="expireSeconds">时效（秒）</param>
        void Set(string key, object obj, int expireSeconds = 0);

        /// <summary>
        /// 缓存对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="obj">对象</param>
        /// <param name="expireSeconds">时效（秒）</param>
        Task SetAsync(string key, object obj, int expireSeconds = 0);

        /// <summary>
        /// 获取泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>泛型对象</returns>
        T Get<T>(string key) where T : new();

        /// <summary>
        /// 获取泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>泛型对象</returns>
        Task<T> GetAsync<T>(string key) where T : new();

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns>字符串</returns>
        string GetHash(string key, string field);

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns>字符串</returns>
        Task<String> GetHashAsync(string key, string field);

        /// <summary>
        /// 缓存哈希表的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>标志</returns>
        bool SetHash(string key, string field, string value);

        /// <summary>
        /// 缓存哈希表的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>标志</returns>
        Task<bool> SetHashAsync(string key, string field, string value);

        /// <summary>
        /// 获取哈希表所有字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>字典</returns>
        Dictionary<string, string> GetHashAll(string key);

        /// <summary>
        /// 获取哈希表所有字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>字典</returns>
        Task<Dictionary<string, string>> GetHashAllAsync(string key);

        /// <summary>
        /// 删除哈希表字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        long DeleteHash(string key, string[] field);

        /// <summary>
        /// 删除哈希表字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        Task<long> DeleteHashAsync(string key, string[] field);

        /// <summary>
        /// 获取哈希表泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <param name="field">字段</param>
        /// <returns>泛型对象</returns>
        T GetHash<T>(string key, string field) where T : new();

        /// <summary>
        /// 获取哈希表泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <param name="field">字段</param>
        /// <returns>泛型对象</returns>
        Task<T> GetHashAsync<T>(string key, string field) where T : new();

        /// <summary>
        /// 获取哈希表所有泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>字典</returns>
        Dictionary<string, T> GetHashAll<T>(string key) where T : new();

        /// <summary>
        /// 获取哈希表所有泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>字典</returns>
        Task<Dictionary<string, T>> GetHashAllAsync<T>(string key) where T : new();

        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns></returns>
        long Delete(String[] keys);

        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns></returns>
        Task<long> DeleteAsync(String[] keys);

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns></returns>
        long Exists(string[] keys);

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns></returns>
        Task<long> ExistsAsync(string[] keys);

        /// <summary>
        /// 查看哈希表 key 中，指定的字段是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="field">字段</param>
        /// <returns>bool</returns>
        bool HExists(string key, string field);

        /// <summary>
        /// 查看哈希表 key 中，指定的字段是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="field">字段</param>
        /// <returns>bool</returns>
        Task<bool> HExistsAsync(string key, string field);

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="expire">时间间隔</param>
        /// <returns>标志</returns>
        bool Expire(string key, TimeSpan expire);

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="seconds">秒数</param>
        /// <returns>标志 </returns>
        bool Expire(string key, int seconds);

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="expire">时间间隔</param>
        /// <returns>标志</returns>
        Task<bool> ExpireAsync(string key, TimeSpan expire);

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="seconds">秒数</param>
        /// <returns>标志</returns>
        Task<bool> ExpireAsync(string key, int seconds);

        /// <summary>
        /// 分布式锁
        /// </summary>
        /// <param name="lockKey">锁名称，不可重复</param>
        /// <param name="action">委托事件</param>
        /// <returns>bool</returns>
        bool LockTake(String lockKey, Action action);

        /// <summary>
        /// 模糊匹配
        /// </summary>
        /// <param name="pattern">匹配表达式</param>
        /// <returns>RedisKey列表</returns>
        List<RedisKey> PatternSearch(String pattern);

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="channel">通道</param>
        /// <param name="message">消息</param>
        void PublishMessage(String channel, String message);

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="channel">通道</param>
        /// <param name="callBack">回调函数:通道,消息</param>
        void SubscribeMessage(String channel, Action<String> callBack);
    }
}
