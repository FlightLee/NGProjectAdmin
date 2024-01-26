using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity
{
    public  class test11
    {

        public string Id { get; set; } = "";
        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractCode { get; set; } = "";

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
        public string warrantId { get; set; } = "";

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
        public string ContractLife { get; set; } = "";

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
        public string ContractPrice { get; set; } = "";

         /// <summary>
        /// 合同总金额
        /// </summary>
        public string ContractMoney { get; set; } = "";

        /// <summary>
        /// 合同保证金
        /// </summary>
        public string ContractPromiseMoney { get; set; } = "";

        /// <summary>
        /// 0半年一交1一年一交
        /// </summary>
        public string ContractPayment { get; set; } = "";


        /// <summary>
        /// 合同信息备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 资产信息ID
        /// </summary>
        public string AssetsId { get; set; }
    }

}
