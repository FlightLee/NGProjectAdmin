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
    /// 合同资产信息明细
    /// </summary>
    [SugarTable("assets_detail")]
    public class Assets_detail:BaseEntity
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int insideId { get; set; }

        /// <summary>
        /// 组Id
        /// </summary>
        public string? GroupId { get; set; }

        /// <summary>
        /// 资产编号Id
        /// </summary>
        public string? AssetsId { get; set; }

        /// <summary>
        /// 资产地址
        /// </summary>
        public string? AssetAdress { get; set; }

        /// <summary>
        /// 资产面积
        /// </summary>
        public double? AssetArea { get; set; }

        /// <summary>
        /// 资产评估价
        /// </summary>
        public double? AssetPrice { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
 
    }
}
