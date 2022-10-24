using NGProjectAdmin.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessEntity.NGBusiness
{

    public class Rent_detail:BaseEntity
    {

        /// <summary>
        /// 租金Id
        /// </summary>
        public int RentId { get; set; }

        /// <summary>
        /// 应收开始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 应收结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 应收金额
        /// </summary>
        public double ContractMoney { get; set; }

        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime FeeDate { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public double Money { get; set; }

        /// <summary>
        /// 欠款金额
        /// </summary>
        public double arrears { get; set; }

        /// <summary>
        /// 是否逾期
        /// </summary>
        public int slippageFlage { get; set; }

        /// <summary>
        /// 是否按时缴款
        /// </summary>
        public int FeeFlage { get; set; }

        /// <summary>
        /// 银行Id
        /// </summary>
        public int BlankId { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string BlankName { get; set; }

        /// <summary>
        /// 银行账号
        /// </summary>
        public string BlankAccount { get; set; }

        /// <summary>
        /// 收款附件Id
        /// </summary>
        public int RentFileId { get; set; }

        /// <summary>
        /// 收款附件
        /// </summary>
        [Navigate(NavigateType.OneToOne, nameof(RentFileId), nameof(File_group.Id))]
        public File_group? rentFiles { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
