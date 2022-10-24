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
    /// 合同资产信息主表
    /// </summary>
    [SugarTable("assets_group")]
    public class Assets_group: BaseEntity
    {
        /// <summary>
        /// 面积
        /// </summary>
        public double Area { get; set; }

        /// <summary>
        /// 总评估价
        /// </summary>
        public double Money { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        [Navigate(NavigateType.OneToMany, nameof(Assets_detail.GroupId))]
        public List<Assets_detail> assets_detail { get; set; }
    }
}
