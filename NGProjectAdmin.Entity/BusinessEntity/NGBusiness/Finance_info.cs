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
    /// 财务信息汇总
    /// </summary>
    [SugarTable("finance_info")]
    public class Finance_info: BaseEntity
    {
        /// <summary>
        /// 已收款金额
        /// </summary>
        public double GetMoney { get; set; }

        /// <summary>
        /// 未收款金额
        /// </summary>
        public double LeftMoney { get; set; }

        /// <summary>
        /// 收款比例
        /// </summary>
        public double MoneyPercent { get; set; }

        /// <summary>
        /// 已开票金额
        /// </summary>
        public double GetInvoiceMoney { get; set; }

        /// <summary>
        /// 未开票金额
        /// </summary>
        public double LeftInvoiceMoney { get; set; }

        /// <summary>
        /// 开票比例
        /// </summary>
        public double InvoicePercent { get; set; }

    }
}
