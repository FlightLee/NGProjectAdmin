using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using SqlSugar;

namespace NGProjectAdmin.Entity.BusinessDTO.SystemManagement
{
    /// <summary>
    /// 系统通知DTO
    /// </summary>
    public class SysNotificationDTO : SysAnnouncement
    {
        /// <summary>
        /// 阅读状态，0：未读，1：已读
        /// </summary>
        [SugarColumn(ColumnName = "READED")]
        public int Readed { get; set; }
    }
}
