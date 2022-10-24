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
    public class Invoice_groupService : BaseService<Invoice_group>, IInvoice_groupService
    {
        #region 属性及其构造函数   
        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IInvoice_groupRepository Invoice_groupRepository;

        public Invoice_groupService(IInvoice_groupRepository Invoice_groupRepository) : base(Invoice_groupRepository)
        {
            this.Invoice_groupRepository = Invoice_groupRepository;
        }
        #endregion

    }
}
