using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using System;

namespace NGProjectAdmin.Entity.BusinessDTO.SystemManagement
{
    /// <summary>
    /// 通知公告DTO
    /// </summary>
    public class SysAnnouncementDTO : SysAnnouncement
    {
        /// <summary>
        /// 收件人列表
        /// </summary>
        public String Addressee { get; set; }

        /// <summary>
        /// 附件编号
        /// </summary>
        public String AttachmentIds { get; set; }

        /// <summary>
        /// 是否发送邮件
        /// </summary>
        public bool SendMail { get; set; }
    }
}
