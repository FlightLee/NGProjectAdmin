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
    /// 资产评估报告单
    /// </summary>
    [SugarTable("assetment_group")]
    public class Assetment_group: BaseEntity
    {
        /// <summary>
        /// 评估报告编号
        /// </summary>
        public string AssetCode { get; set; }

        /// <summary>
        /// 经办人Id
        /// </summary>
        public Guid PersonId { get; set; }

        /// <summary>
        /// 经办人名称
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// 经办人部门Id
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// 经办人部门名称
        /// </summary>
        public string DepartName { get; set; }

        /// <summary>
        /// 评估日期
        /// </summary>
        public DateTime BuildDate { get; set; }

        /// <summary>
        /// 评估机构
        /// </summary>
        public string MAEPName { get; set; }

        /// <summary>
        /// 评估费用
        /// </summary>        
        public double Money { get; set; }

        /// <summary>
        /// 评估文件Id
        /// </summary>
        public int MAEPFileGroupId { get; set; }
        /// <summary>
        /// 评估文件
        /// </summary>
        
        [Navigate(NavigateType.OneToOne, nameof(MAEPFileGroupId), nameof(File_group.Id))]
        public File_group? MAEPFiles { get; set; }

        [Navigate(NavigateType.OneToMany, nameof(Assetment_detail.AssetMentId))]
        public List<Assetment_detail> assetment_detail { get; set; }
    }
}
