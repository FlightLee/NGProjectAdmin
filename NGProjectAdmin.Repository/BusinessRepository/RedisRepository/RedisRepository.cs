using NGProjectAdmin.Common.Utility;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NGProjectAdmin.Repository.BusinessRepository.RedisRepository
{
    /// <summary>
    /// Redis访问层实现
    /// </summary>
    public class RedisRepository : IRedisRepository
    {
        /// <summary>
        /// 依据Key获取字符串
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>字符串</returns>
        public string Get(string key)
        {
            return NGRedisContext.Get(key);
        }

        /// <summary>
        /// 依据Key获取字符串
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>字符串</returns>
        public async Task<string> GetAsync(string key)
        {
            return await NGRedisContext.GetAsync(key);
        }

        /// <summary>
        /// 缓存对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="obj">对象</param>
        /// <param name="expireSeconds">时效（秒）</param>
        public void Set(string key, object obj, int expireSeconds = 0)
        {
            NGRedisContext.Set(key, obj, expireSeconds);
        }

        /// <summary>
        /// 缓存对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="obj">对象</param>
        /// <param name="expireSeconds">时效（秒）</param>
        /// <returns></returns>
        public async Task SetAsync(string key, object obj, int expireSeconds = 0)
        {
            await NGRedisContext.SetAsync(key, obj, expireSeconds);
        }

        /// <summary>
        /// 获取泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>泛型对象</returns>
        public T Get<T>(string key) where T : new()
        {
            return NGRedisContext.Get<T>(key);
        }

        /// <summary>
        /// 获取泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>泛型对象</returns>
        public async Task<T> GetAsync<T>(string key) where T : new()
        {
            return await NGRedisContext.GetAsync<T>(key);
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns>字符串</returns>
        public string GetHash(string key, string field)
        {
            return NGRedisContext.GetHash(key, field);
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns>字符串</returns>
        public async Task<String> GetHashAsync(string key, string field)
        {
            return await NGRedisContext.GetHashAsync(key, field);
        }

        /// <summary>
        /// 缓存哈希表的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>标志</returns>
        public bool SetHash(string key, string field, string value)
        {
            return NGRedisContext.SetHash(key, field, value);
        }

        /// <summary>
        /// 缓存哈希表的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>标志</returns>
        public async Task<bool> SetHashAsync(string key, string field, string value)
        {
            return await NGRedisContext.SetHashAsync(key, field, value);
        }

        /// <summary>
        /// 获取哈希表所有字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>字典</returns>
        public Dictionary<string, string> GetHashAll(string key)
        {
            return NGRedisContext.GetHashAll(key);
        }

        /// <summary>
        /// 获取哈希表所有字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>字典</returns>
        public async Task<Dictionary<string, string>> GetHashAllAsync(string key)
        {
            return await NGRedisContext.GetHashAllAsync(key);
        }

        /// <summary>
        /// 删除哈希表字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public long DeleteHash(string key, string[] field)
        {
            return NGRedisContext.DeleteHash(key, field);
        }

        /// <summary>
        /// 删除哈希表字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public async Task<long> DeleteHashAsync(string key, string[] field)
        {
            return await NGRedisContext.DeleteHashAsync(key, field);
        }

        /// <summary>
        /// 获取哈希表泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <param name="field">字段</param>
        /// <returns>泛型对象</returns>
        public T GetHash<T>(string key, string field) where T : new()
        {
            return NGRedisContext.GetHash<T>(key, field);
        }

        /// <summary>
        /// 获取哈希表泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <param name="field">字段</param>
        /// <returns>泛型对象</returns>
        public async Task<T> GetHashAsync<T>(string key, string field) where T : new()
        {
            return await NGRedisContext.GetHashAsync<T>(key, field);
        }

        /// <summary>
        /// 获取哈希表所有泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>字典</returns>
        public Dictionary<string, T> GetHashAll<T>(string key) where T : new()
        {
            return NGRedisContext.GetHashAll<T>(key);
        }

        /// <summary>
        /// 获取哈希表所有泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>字典</returns>
        public async Task<Dictionary<string, T>> GetHashAllAsync<T>(string key) where T : new()
        {
            return await NGRedisContext.GetHashAllAsync<T>(key);
        }

        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns></returns>
        public long Delete(String[] keys)
        {
            return NGRedisContext.Delete(keys);
        }

        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns></returns>
        public async Task<long> DeleteAsync(String[] keys)
        {
            return await NGRedisContext.DeleteAsync(keys);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return NGRedisContext.Exists(key);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns></returns>
        public long Exists(string[] keys)
        {
            return NGRedisContext.Exists(keys);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string key)
        {
            return await NGRedisContext.ExistsAsync(key);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns></returns>
        public async Task<long> ExistsAsync(string[] keys)
        {
            return await NGRedisContext.ExistsAsync(keys);
        }


        /// <summary>
        /// 查看哈希表 key 中，指定的字段是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="field">字段</param>
        /// <returns>bool</returns>
        public bool HExists(string key, string field)
        {
            return NGRedisContext.HExists(key, field);
        }

        /// <summary>
        /// 查看哈希表 key 中，指定的字段是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="field">字段</param>
        /// <returns>bool</returns>
        public async Task<bool> HExistsAsync(string key, string field)
        {
            return await NGRedisContext.HExistsAsync(key, field);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="expire">时间间隔</param>
        /// <returns>标志</returns>
        public bool Expire(string key, TimeSpan expire)
        {
            return NGRedisContext.Expire(key, expire);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="seconds">秒数</param>
        /// <returns>标志 </returns>
        public bool Expire(string key, int seconds)
        {
            return NGRedisContext.Expire(key, seconds);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="expire">时间间隔</param>
        /// <returns>标志</returns>
        public async Task<bool> ExpireAsync(string key, TimeSpan expire)
        {
            return await NGRedisContext.ExpireAsync(key, expire);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="seconds">秒数</param>
        /// <returns>标志</returns>
        public async Task<bool> ExpireAsync(string key, int seconds)
        {
            return await NGRedisContext.ExpireAsync(key, seconds);
        }

        /// <summary>
        /// 分布式锁
        /// </summary>
        /// <param name="lockKey">锁名称，不可重复</param>
        /// <param name="action">委托事件</param>
        /// <returns>bool</returns>
        public bool LockTake(String lockKey, Action action)
        {
            return NGRedisContext.LockTake(lockKey, action);
        }

        /// <summary>
        /// 模糊匹配
        /// </summary>
        /// <param name="pattern">匹配表达式</param>
        /// <returns>RedisKey列表</returns>
        public List<RedisKey> PatternSearch(String pattern)
        {
            return NGRedisContext.PatternSearch(pattern);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="channel">通道</param>
        /// <param name="message">消息</param>
        public void PublishMessage(String channel, String message)
        {
            NGRedisContext.PublishMessage(channel, message);
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="channel">通道</param>
        /// <param name="callBack">回调函数:通道,消息</param>
        public void SubscribeMessage(String channel, Action<String> callBack)
        {
            NGRedisContext.SubscribeMessage(channel, callBack);
        }
    }
}
