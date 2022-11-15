using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessDTO.NGBusiness;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.Base;
using NPOI.SS.Formula.Functions;

namespace NGProjectAdmin.Repository.BusinessRepository.NGBusiness
{
    /// <summary>
    /// File_group数据访问层实现
    /// </summary>   
    public class Assets_infoRepository : BaseRepository<Assets_info>, IAssets_infoRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public Assets_infoRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<Assets_infoDTO>> GetAssetInfoListAsync(QueryCondition queryCondition)
        {
            var where = QueryCondition.BuildExpression<Assets_info>(queryCondition.QueryItems);

            var list = await NGDbContext.
               Queryable<Assets_info>().LeftJoin<Contract_baseinfo>((a,c)=>a.AssetsCode==c.AssetsId).
               WhereIF(true, where).
               OrderByIF(!String.IsNullOrEmpty(queryCondition.Sort), queryCondition.Sort).
               Select((a, c) =>new Assets_infoDTO() {Id=a.Id, AssetsCode=a.AssetsCode, AssetsTypeId=a.AssetsTypeId, AssetsState=a.AssetsState, AssetsArea=a.AssetsArea,AssetsAdress=a.AssetsAdress,AssetUseType=a.AssetUseType,contractinfo=new Assets_info_ContractDTO() { lessee=c.lessee,lesseePhone=c.lesseePhone,ContracStartDate=c.ContracStartDate,ContractEndDate=c.ContractEndDate,ContractPrice=c.ContractPrice,ContractMoney=c.ContractMoney   }  }).
               ToListAsync();            
            return list;
        }
    }
}
