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
    public class Assetment_detailService : BaseService<Assetment_detail>, IAssetment_detailService
    {
        #region 属性及其构造函数   
        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IAssetment_detailRepository Assetment_detailRepository;

        public Assetment_detailService(IAssetment_detailRepository Assetment_detailRepository) : base(Assetment_detailRepository)
        {
            this.Assetment_detailRepository = Assetment_detailRepository;
        }
        #endregion

    }
}
