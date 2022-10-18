using NGProjectAdmin.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessEntity.NGBusiness
{
    /// <summary>
    /// 租金主表
    /// </summary>
    [SugarTable("Rent_group")]
    public class Rent_group: BaseEntity
    {
        /// <summary>
        /// 应收金额
        /// </summary>
        public double ContractMoney { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public double Money { get; set; }

        /// <summary>
        /// 欠款金额
        /// </summary>
        public double Arrears { get; set; }

        [Navigate(NavigateType.OneToMany, nameof(Rent_detail.RentId))]
        public List<Rent_detail> rent_details { get; set; }
    }
}
