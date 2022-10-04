
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.BusinessRepository.RedisRepository;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.AreaRepository;
using NGProjectAdmin.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.AreaService
{
    /// <summary>
    /// 行政区域服务层实现
    /// </summary>
    public class AreaService : NGAdminBaseService<SysArea>, IAreaService
    {
        #region 属性及构造函数

        /// <summary>
        /// 行政区域仓储实例
        /// </summary>
        private readonly IAreaRepository areaRepository;

        /// <summary>
        /// Redis仓储实例
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="areaRepository"></param>
        /// <param name="redisRepository"></param>
        public AreaService(IAreaRepository areaRepository, IRedisRepository redisRepository) : base(areaRepository)
        {
            this.areaRepository = areaRepository;
            this.redisRepository = redisRepository;
        }

        #endregion

        #region 获取行政区域树

        /// <summary>
        /// 获取行政区域树
        /// </summary>
        /// <returns></returns>
        public async Task<QueryResult<SysAreaDTO>> GetAreaTreeNodes()
        {
            return await Task.Run(async () =>
            {
                var result = new List<SysAreaDTO>();

                var areas = await this.redisRepository.GetAsync<List<SysAreaDTO>>(NGAdminGlobalContext.SystemCacheConfig.AreaCacheName);
                var parentAreas = areas.Where(t => t.ParentAreaCode == "0").OrderBy(t => t.AreaCode).ToList();

                foreach (var item in parentAreas)
                {
                    this.GetAreaChildren(item, areas);
                }

                result.AddRange(parentAreas);

                var queryResult = new QueryResult<SysAreaDTO>();
                queryResult.HttpStatusCode = HttpStatusCode.OK;
                queryResult.Message = new String("OK");
                queryResult.List = result;

                return queryResult;
            });
        }

        #endregion

        #region 加载行政区域缓存

        /// <summary>
        /// 加载行政区域缓存
        /// </summary>
        public async Task LoadSysAreaCache()
        {
            var areas = await this.areaRepository.GetListAsync<SysAreaDTO>();
            await this.redisRepository.SetAsync(NGAdminGlobalContext.SystemCacheConfig.AreaCacheName, areas, -1);

            NGLoggerContext.Info("系统行政区域缓存加载完成");
        }

        #endregion

        #region 清理行政区域缓存

        /// <summary>
        /// 清理行政区域缓存
        /// </summary>
        public async Task ClearSysAreaCache()
        {
            await this.redisRepository.DeleteAsync(new String[] { NGAdminGlobalContext.SystemCacheConfig.AreaCacheName });

            NGLoggerContext.Info("系统行政区域缓存清理完成");
        }

        #endregion

        #region 服务层私有方法

        /// <summary>
        /// 递归树
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="areas">区域列表</param>
        private void GetAreaChildren(SysAreaDTO root, List<SysAreaDTO> areas)
        {
            var list = areas.Where(t => t.ParentAreaCode == root.AreaCode).ToList();
            if (list.Count > 0)
            {
                root.Children = new List<SysAreaDTO>();
                root.Children.AddRange(list.OrderBy(t => t.AreaCode).ToList());
                foreach (var item in list)
                {
                    this.GetAreaChildren(item, areas);
                }
            }
        }

        #endregion
    }
}
