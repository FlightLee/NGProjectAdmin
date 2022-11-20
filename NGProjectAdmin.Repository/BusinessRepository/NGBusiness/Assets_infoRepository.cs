using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Nest;
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

        public async Task<Assets_infoDTO> GetAssetByIdAsync(string assetId)
        {
            var asset = await NGDbContext.Queryable<Assets_info>()
               .LeftJoin<Contract_baseinfo>((a, c) => a.Id == c.AssetsId)
               .LeftJoin<Assetment_detail>((a, c, d) => a.AssetsMentGroupId == d.AssetId)
               .LeftJoin<Assetment_group>((a, c, d,e) => a.AssetsMentGroupId == e.Id)               
                .Select((a, c, d,e) => new Assets_infoDTO()
                {                    
                    propertyFileGroupId = a.propertyFileGroupId,
                    LandPropertyInfo = a.LandPropertyInfo,
                    landCode = a.landCode,
                    PropertyCode = a.PropertyCode,
                    AssetsFileGroupId = a.AssetsFileGroupId,
                    AssetsFor = a.AssetsFor,
                    PropertyOwner = a.PropertyOwner,
                    Id = a.Id,
                    AssetsCode = a.AssetsCode,
                    AssetsTypeId = a.AssetsTypeId,
                    AssetsSourceId = a.AssetsSourceId,
                    AssetsState = a.AssetsState,
                    AssetsArea = a.AssetsArea,
                    AssetsAdress = a.AssetsAdress,
                    AssetUseType = a.AssetUseType,
                    contractinfo = new Assets_info_ContractDTO()
                    {
                        remark = c.Remark,
                        contractState = c.ContractState,
                        contractPromiseMoney = c.ContractPromiseMoney,
                        contractLife = c.ContractLife,
                        contractPayment = c.ContractPayment,
                        ContractPdfGroupId = c.ContractPdfGroupId,
                        lesseeId = c.lesseeId,
                        lesseeAdress = c.lesseeAdress,
                        lessorId = c.lessorId,
                        lessor = c.Lessor,
                        lessorPhone = c.lessorPhone,
                        contractDate = c.ContractDate,
                        contractType = c.ContractType,
                        id = c.Id,
                        lessee = c.lessee,
                        lesseePhone = c.lesseePhone,
                        ContracStartDate = c.ContracStartDate,
                        ContractEndDate = c.ContractEndDate,
                        ContractPrice = c.ContractPrice,
                        ContractMoney = c.ContractMoney
                    },
                    assetsMent = new Assets_info_AssetMentDTO()
                    {
                        buildDate =e.BuildDate,
                        assessArea=d.AssessArea,
                        assetPriceOneYear=d.AssetPriceOneYear
                    }
                })
                .FirstAsync(a => a.Id.Equals(assetId));

            return asset;
        }

        public async Task<List<Assets_infoDTO>> GetAssetInfoListAsync(QueryCondition queryCondition)
        {
            var where = QueryCondition.BuildExpression<Assets_info>(queryCondition.QueryItems);

            var list = await NGDbContext.
               Queryable<Assets_info>().LeftJoin<Contract_baseinfo>((a, c) => a.Id == c.AssetsId).
               WhereIF(true, where).
               OrderByIF(!String.IsNullOrEmpty(queryCondition.Sort), queryCondition.Sort).
               Select((a, c) => new Assets_infoDTO() { Id = a.Id, AssetsCode = a.AssetsCode, AssetsTypeId = a.AssetsTypeId, AssetsState = a.AssetsState, AssetsArea = a.AssetsArea, AssetsAdress = a.AssetsAdress, AssetUseType = a.AssetUseType, contractinfo = new Assets_info_ContractDTO() { lessee = c.lessee, lesseePhone = c.lesseePhone, ContracStartDate = c.ContracStartDate, ContractEndDate = c.ContractEndDate, ContractPrice = c.ContractPrice, ContractMoney = c.ContractMoney } }).
               ToListAsync();
            return list;
        }
    }
}
