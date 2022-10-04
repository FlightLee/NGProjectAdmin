
using NGProjectAdmin.Repository.BusinessRepository.RedisRepository;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.RedisService
{
    /// <summary>
    /// Redis服务
    /// </summary>
    public class RedisService : IRedisService
    {
        /// <summary>
        /// Redis访问层实例
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="redisRepository">管道实例</param>
        public RedisService(IRedisRepository redisRepository)
        {
            this.redisRepository = redisRepository;
        }

        /// <summary>
        /// 依据Key获取字符串
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>字符串</returns>
        public string Get(string key)
        {
            return this.redisRepository.Get(key);
        }

        /// <summary>
        /// 依据Key获取字符串
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>字符串</returns>
        public async Task<string> GetAsync(string key)
        {
            return await this.redisRepository.GetAsync(key);
        }

        /// <summary>
        /// 缓存对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="obj">对象</param>
        /// <param name="expireSeconds">时效（秒）</param>
        public void Set(string key, object obj, int expireSeconds = 0)
        {
            this.redisRepository.Set(key, obj, expireSeconds);
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
            await this.redisRepository.SetAsync(key, obj, expireSeconds);
        }

        /// <summary>
        /// 获取泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>泛型对象</returns>
        public T Get<T>(string key) where T : new()
        {
            return this.redisRepository.Get<T>(key);
        }

        /// <summary>
        /// 获取泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>泛型对象</returns>
        public async Task<T> GetAsync<T>(string key) where T : new()
        {
            return await this.redisRepository.GetAsync<T>(key);
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns>字符串</returns>
        public string GetHash(string key, string field)
        {
            return this.redisRepository.GetHash(key, field);
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns>字符串</returns>
        public async Task<String> GetHashAsync(string key, string field)
        {
            return await this.redisRepository.GetHashAsync(key, field);
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
            return this.redisRepository.SetHash(key, field, value);
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
            return await this.redisRepository.SetHashAsync(key, field, value);
        }

        /// <summary>
        /// 获取哈希表所有字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>字典</returns>
        public Dictionary<string, string> GetHashAll(string key)
        {
            return this.redisRepository.GetHashAll(key);
        }

        /// <summary>
        /// 获取哈希表所有字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>字典</returns>
        public async Task<Dictionary<string, string>> GetHashAllAsync(string key)
        {
            return await this.redisRepository.GetHashAllAsync(key);
        }

        /// <summary>
        /// 删除哈希表字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public long DeleteHash(string key, string[] field)
        {
            return this.redisRepository.DeleteHash(key, field);
        }

        /// <summary>
        /// 删除哈希表字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public async Task<long> DeleteHashAsync(string key, string[] field)
        {
            return await this.redisRepository.DeleteHashAsync(key, field);
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
            return this.redisRepository.GetHash<T>(key, field);
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
            return await this.redisRepository.GetHashAsync<T>(key, field);
        }

        /// <summary>
        /// 获取哈希表所有泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>字典</returns>
        public Dictionary<string, T> GetHashAll<T>(string key) where T : new()
        {
            return this.redisRepository.GetHashAll<T>(key);
        }

        /// <summary>
        /// 获取哈希表所有泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>字典</returns>
        public async Task<Dictionary<string, T>> GetHashAllAsync<T>(string key) where T : new()
        {
            return await this.redisRepository.GetHashAllAsync<T>(key);
        }

        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns></returns>
        public long Delete(String[] keys)
        {
            return this.redisRepository.Delete(keys);
        }

        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns></returns>
        public async Task<long> DeleteAsync(String[] keys)
        {
            return await this.redisRepository.DeleteAsync(keys);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return this.redisRepository.Exists(key);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns></returns>
        public long Exists(string[] keys)
        {
            return this.redisRepository.Exists(keys);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string key)
        {
            return await this.redisRepository.ExistsAsync(key);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns></returns>
        public async Task<long> ExistsAsync(string[] keys)
        {
            return await this.redisRepository.ExistsAsync(keys);
        }

        /// <summary>
        /// 查看哈希表 key 中，指定的字段是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="field">字段</param>
        /// <returns>bool</returns>
        public bool HExists(string key, string field)
        {
            return this.redisRepository.HExists(key, field);
        }

        /// <summary>
        /// 查看哈希表 key 中，指定的字段是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="field">字段</param>
        /// <returns>bool</returns>
        public async Task<bool> HExistsAsync(string key, string field)
        {
            return await this.redisRepository.HExistsAsync(key, field);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="expire">时间间隔</param>
        /// <returns>标志</returns>
        public bool Expire(string key, TimeSpan expire)
        {
            return this.redisRepository.Expire(key, expire);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="seconds">秒数</param>
        /// <returns>标志 </returns>
        public bool Expire(string key, int seconds)
        {
            return this.redisRepository.Expire(key, seconds);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="expire">时间间隔</param>
        /// <returns>标志</returns>
        public async Task<bool> ExpireAsync(string key, TimeSpan expire)
        {
            return await this.redisRepository.ExpireAsync(key, expire);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="seconds">秒数</param>
        /// <returns>标志</returns>
        public async Task<bool> ExpireAsync(string key, int seconds)
        {
            return await this.redisRepository.ExpireAsync(key, seconds);
        }

        /// <summary>
        /// 分布式锁
        /// </summary>
        /// <param name="lockKey">锁名称，不可重复</param>
        /// <param name="action">委托事件</param>
        /// <returns>bool</returns>
        public bool LockTake(String lockKey, Action action)
        {
            return this.redisRepository.LockTake(lockKey, action);
        }

        /// <summary>
        /// 模糊匹配
        /// </summary>
        /// <param name="pattern">匹配表达式</param>
        /// <returns>RedisKey列表</returns>
        public List<RedisKey> PatternSearch(String pattern)
        {
            return this.redisRepository.PatternSearch(pattern);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="channel">通道</param>
        /// <param name="message">消息</param>
        public void PublishMessage(String channel, String message)
        {
            this.redisRepository.PublishMessage(channel, message);
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="channel">通道</param>
        /// <param name="callBack">回调函数:通道,消息</param>
        public void SubscribeMessage(String channel, Action<String> callBack)
        {
            this.redisRepository.SubscribeMessage(channel, callBack);
        }
    }
}
