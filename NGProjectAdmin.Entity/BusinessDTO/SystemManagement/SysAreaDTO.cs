using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using SqlSugar;
using System.Collections.Generic;

namespace NGProjectAdmin.Entity.BusinessDTO.SystemManagement
{
    /// <summary>
    /// 行政区域DTO
    /// </summary>
    [SugarTable("sys_area")]
    public class SysAreaDTO : SysArea
    {
        /// <summary>
        /// 子集
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<SysAreaDTO> Children { get; set; }
    }
}
