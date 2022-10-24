using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.Base;
using NGProjectAdmin.Repository.BusinessRepository.NGBusiness;
using NGProjectAdmin.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.NGBusiness
{
    public class Assets_detailService : BaseService<Assets_detail>, IAssets_detailService
    {
        #region 属性及其构造函数   
        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IAssets_detailRepository Assets_detailRepository;

        public Assets_detailService(IAssets_detailRepository Assets_detailRepository) : base(Assets_detailRepository)
        {
            this.Assets_detailRepository = Assets_detailRepository;
        }
        #endregion

    }
}
