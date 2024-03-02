using Masuit.Tools;
using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessDTO.NGBusiness;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.Base;
using SqlSugar;

namespace NGProjectAdmin.Repository.BusinessRepository.NGBusiness
{
    /// <summary>
    /// File_group数据访问层实现
    /// </summary>   
    public class Assets_ChecklRepository : NGAdminBaseRepository<assets_check>, IAssets_ChecklRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public Assets_ChecklRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<Assets_CheckDTO>> GetAssetCheckListAsync(QueryCondition queryCondition, RefAsync<int> totalCount)
        {
            var where = QueryCondition.BuildExpression<assets_check>(queryCondition.QueryItems);

            var list = await NGDbContext.
               Queryable<assets_check>().
               LeftJoin<Assets_info>((a, c) => c.Id == a.assetsId && a.IsDel == 0 && c.IsDel == 0).
               WhereIF(true, where).
               Where(a => a.IsDel == 0).
               OrderByIF(!String.IsNullOrEmpty(queryCondition.Sort), queryCondition.Sort).
               Select((a,c) => new Assets_CheckDTO() { assetsId=a.assetsId, assetsName= c.assetsName, Id=a.Id, checkName= a.checkName, checkPhone=a.checkPhone, checkProblem=a.checkProblem, checkTime=a.checkTime, checkType=a.checkType, CreateTime=a.CreateTime, Creator=a.Creator, fileGroupsId=a.fileGroupsId, IsDel=a.IsDel, Modifier=a.Modifier, ModifyTime=a.ModifyTime, problemFix=a.problemFix, Remark=a.Remark, VersionId=a.VersionId}).
               ToPageListAsync(queryCondition.PageIndex, queryCondition.PageSize, totalCount);

       
            return list;
        }
    }
}
