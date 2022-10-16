using NGProjectAdmin.Entity.BusinessDTO.File;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.File;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.Base;
using NGProjectAdmin.Repository.BusinessRepository.BusinessModule.BusinessUserRepository;
using NGProjectAdmin.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.File
{
    public class File_groupService : NGAdminBaseService<File_group>, IFile_groupService
    {
        #region 属性及其构造函数   
        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IFile_groupRepository File_groupRepository;

        public File_groupService(INGAdminBaseRepository<File_group> File_groupRepository) : base(File_groupRepository)
        {
            this.File_groupRepository = File_groupRepository;
        }
        #endregion

        public Task<ActionResult> DeleteFile_group(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> DeleteFile_group(int Id, int insideId)
        {
            throw new NotImplementedException();
        }

        public Task<File_group> InsertFile_group(File_groupDTO File_group)
        {
            throw new NotImplementedException();
        }
    }
}
