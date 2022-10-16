using NGProjectAdmin.Entity.BusinessDTO.File;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.File;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.File
{
    /// <summary>
    /// File_group 业务接口
    /// </summary>
    public interface IFile_groupService : INGAdminBaseService<File_group>
    {
        /// <summary>
        /// 新增文件
        /// </summary>
        /// <param name="File_group"></param>
        /// <returns></returns>
        Task<File_group> InsertFile_group(File_groupDTO File_group);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ActionResult> DeleteFile_group(int Id);

        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ActionResult> DeleteFile_group(int Id,int insideId);
    }
}
