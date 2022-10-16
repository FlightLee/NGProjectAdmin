using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessEntity.Contract
{
    /// <summary>
    /// 合同基础信息
    /// </summary>
    [SugarTable("contract_baseinfo")]
    public class Contract_baseinfo
    {
        /// <summary>
        /// 合同编号
        /// </summary>
        public int ContractCode { get; set; }

        /// <summary>
        /// 合同签订日期
        /// </summary>
        public DateTime ContractDate { get; set; }

        /// <summary>
        /// 合同类型 0公开租牌合同1协议租赁合同2出借协议
        /// </summary>
        public int ContractType { get; set; }

        /// <summary>
        /// 出租方（甲方）
        /// </summary>
        public string? Lessor { get; set; }

        /// <summary>
        /// 甲方地址
        /// </summary>
        public string? LessorAdress { get; set; }

        /// <summary>
        /// 承租方（乙方）
        /// </summary>
        public string? lessee { get; set; }

        /// <summary>
        /// 乙方身份证号码（统一社会信用代码）
        /// </summary>
        public string? lesseeId { get; set; }

        /// <summary>
        /// 乙方联系方式
        /// </summary>
        public string? lesseePhone { get; set; }

        /// <summary>
        /// 乙方地址
        /// </summary>
        public string? lesseeAdress { get; set; }

        /// <summary>
        /// 担保方
        /// </summary>
        public string? Warrant { get; set; }

        /// <summary>
        /// 担保方身份证号码（统一社会信用代码）
        /// </summary>
        public int warrantId { get; set; }

        /// <summary>
        /// 担保方联系方式
        /// </summary>
        public string? warrantPhone { get; set; }

        /// <summary>
        /// 担保方地址
        /// </summary>
        public string? warrantAdress { get; set; }

        /// <summary>
        /// 合同总租期（年）
        /// </summary>
        public int ContractLife { get; set; }

        /// <summary>
        /// 租期起始日期
        /// </summary>
        public DateTime ContracStartDate { get; set; }

        /// <summary>
        /// 租期终止日期
        /// </summary>
        public DateTime ContractEndDate { get; set; }

        /// <summary>
        /// 合同年租金
        /// </summary>
        public double ContractPrice { get; set; }

        /// <summary>
        /// 合同总金额
        /// </summary>
        public double ContractMoney { get; set; }

        /// <summary>
        /// 合同保证金
        /// </summary>
        public double ContractPromiseMoney { get; set; }

        /// <summary>
        /// 0半年一交1一年一交
        /// </summary>
        public int ContractPayment { get; set; }

        /// <summary>
        /// 合同附件表Id
        /// </summary>
        public int ContractPdfGroupId { get; set; }

        /// <summary>
        /// 合同信息备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 资产信息ID
        /// </summary>
        public int AssetsId { get; set; }

        /// <summary>
        /// 租金收款组Id
        /// </summary>
        public int RentGroupId { get; set; }

        /// <summary>
        /// 开票登记组Id
        /// </summary>
        public int InvoiceGroupId { get; set; }

        /// <summary>
        /// 保证金违约金信息表Id
        /// </summary>
        public int ContractDefaultId { get; set; }

        /// <summary>
        /// 财务信息Id
        /// </summary>
        public int FinanceId { get; set; }


        /// <summary>
        /// 0合同完结1合同逾期2不存在逾期
        /// </summary>
        public int ContractState { get; set; }
    }
}
