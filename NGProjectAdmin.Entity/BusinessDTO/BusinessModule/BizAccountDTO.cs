using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessDTO.BusinessModule
{
    /// <summary>
    /// 同步账号DTO
    /// </summary>
    public class BizAccountDTO : BizAccount
    {
        /// <summary>
        /// 口令
        /// </summary>
        public String Token;
    }
}
