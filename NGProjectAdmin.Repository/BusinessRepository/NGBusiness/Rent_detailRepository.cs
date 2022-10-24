using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.NGBusiness
{
    /// <summary>
    /// File_group数据访问层实现
    /// </summary>   
    public class Rent_detailRepository : BaseRepository<Rent_detail>, IRent_detailRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public Rent_detailRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
