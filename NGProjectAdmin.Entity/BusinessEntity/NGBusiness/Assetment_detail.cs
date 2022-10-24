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
    /// 资产评估报告单明细
    /// </summary>
    [SugarTable("assetment_detail")]
    public class Assetment_detail: BaseEntity
    {
        /// <summary>
        /// 资产评估主表Id
        /// </summary>
        public int AssetMentId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int insideId { get; set; }

        /// <summary>
        /// 资产Id
        /// </summary>
        public int AssetId { get; set; }

        /// <summary>
        /// 资产名称
        /// </summary>
        public string AssetName { get; set; }

        /// <summary>
        /// 资产地址
        /// </summary>
        public string AssetAdress { get; set; }

        /// <summary>
        /// 资产面积
        /// </summary>
        public double AssessArea { get; set; }

        /// <summary>
        /// 评估单价/月
        /// </summary>
        public double AssetPriceOneMonth { get; set; }

        /// <summary>
        /// 评估总价/年
        /// </summary>
        public double AssetPriceOneYear { get; set; }

        /// <summary>
        /// 分摊评估费用
        /// </summary>
        public double EverAssessMoney { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
