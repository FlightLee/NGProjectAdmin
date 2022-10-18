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
    /// 合同保证金管理
    /// </summary>
    [SugarTable("contract_default")]
    public class Contract_default: BaseEntity
    {
        /// <summary>
        /// 保证金应收
        /// </summary>
        public double DefaultMoney { get; set; }

        /// <summary>
        /// 保证金实收
        /// </summary>
        public string Money { get; set; }

        /// <summary>
        /// 保证金收款日期
        /// </summary>
        public DateTime DefaultDate { get; set; }

        /// <summary>
        /// 保证金银行Id
        /// </summary>
        public int BankId { get; set; }

        /// <summary>
        /// 保证金银行名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 保证金银行账号
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 保证金余额
        /// </summary>
        public double DefaultBalance { get; set; }

        /// <summary>
        /// 保证金实退
        /// </summary>
        public double DefaultReturnMoney { get; set; }

        /// <summary>
        /// 保证金退款日期
        /// </summary>
        public DateTime DefaultReturnDate { get; set; }

        /// <summary>
        /// 保证金退款状态
        /// </summary>
        public int DefaultRerunState { get; set; }

        /// <summary>
        /// 违约金额
        /// </summary>
        public double BreakMoney { get; set; }

        /// <summary>
        /// 违约金实收
        /// </summary>
        public double BreakMoneyActual { get; set; }

        /// <summary>
        /// 保证金文件Id
        /// </summary>
        public int DefaultFileId { get; set; }

        /// <summary>
        /// 评估文件
        /// </summary>
        [Navigate(NavigateType.OneToOne, nameof(DefaultFileId), nameof(File_group.Id))]
        public File_group? DefaultFiles { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
