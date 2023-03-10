using Nest;
using NetTaste;
using NGProjectAdmin.Entity.BusinessDTO.NGBusiness;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.Base;
using NGProjectAdmin.Repository.BusinessRepository.NGBusiness;
using NGProjectAdmin.Service.Base;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.NGBusiness
{
    public class Assets_infoService : BaseService<Assets_info>, IAssets_infoService
    {
        #region 属性及其构造函数   
        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IAssets_infoRepository Assets_infoRepository;

        public Assets_infoService(IAssets_infoRepository Assets_infoRepository) : base(Assets_infoRepository)
        {
            this.Assets_infoRepository = Assets_infoRepository;
        }

        public async Task<ActionResult> DeleteAssetAndContract(Assets_infoDTO assetId)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.Assets_infoRepository.DeleteAssetAndContract(assetId);

            return actionResult;
        }

        public async Task<ActionResult> GetAssetByIdAsync(Assets_infoDTO assetId)
        {
            var actionResult = new ActionResult();
          
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.Assets_infoRepository.GetAssetByIdAsync(assetId );

            return actionResult;
        }

        public async Task<QueryResult<Assets_infoDTO>> GetAssetInfoListAsync(QueryCondition queryCondition)
        {
            var queryResult = new QueryResult<Assets_infoDTO>();
            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            RefAsync<int> totalCount = 0;
            queryResult.List = await this.Assets_infoRepository.GetAssetInfoListAsync(queryCondition,  totalCount);
            queryResult.TotalCount = totalCount;

            return queryResult;
        }
        #endregion

    }
}
