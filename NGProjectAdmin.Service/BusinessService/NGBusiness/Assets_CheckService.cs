using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Entity.BusinessDTO.NGBusiness;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.Base;
using NGProjectAdmin.Repository.BusinessRepository.NGBusiness;
using NGProjectAdmin.Service.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.NGBusiness
{
    public class Assets_CheckService : NGAdminBaseService<assets_check>, IAssets_CheckService
    {
        #region 属性及其构造函数   
        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IAssets_ChecklRepository assets_ChecklRepository;

        public Assets_CheckService(IAssets_ChecklRepository Assets_ChecklRepository) : base(Assets_ChecklRepository)
        {
            this.assets_ChecklRepository = Assets_ChecklRepository;
        }
        #endregion
        public async Task<QueryResult<Assets_CheckDTO>> GetAssetCheckListAsync(QueryCondition queryCondition)
        {
            var queryResult = new QueryResult<Assets_CheckDTO>();
            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            RefAsync<int> totalCount = 0;
            queryResult.List = await this.assets_ChecklRepository.GetAssetCheckListAsync(queryCondition, totalCount);
            queryResult.TotalCount = totalCount;

            return queryResult;
        }

   
        }
    }
