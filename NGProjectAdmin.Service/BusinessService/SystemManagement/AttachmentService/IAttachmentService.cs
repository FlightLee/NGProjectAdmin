
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.Base;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.AttachmentService
{
    /// <summary>
    /// 业务附件服务层接口
    /// </summary>
    public interface IAttachmentService : INGAdminBaseService<SysAttachment>
    {
        /// <summary>
        /// 系统文件统计
        /// </summary>
        /// <returns>ActionResult</returns>
        Task<ActionResult> QuerySysFileStatisticalInfo();
    }
}
