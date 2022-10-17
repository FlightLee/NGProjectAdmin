using NGProjectAdmin.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessDTO.NGBusiness
{
    /// <summary>
    /// 文件信息主表
    /// </summary>
    public class File_groupDTO : BaseEntity
    {
        public File_groupDTO()
        {
            File_details = new List<File_detailDTO>();
        }
        public List<File_detailDTO> File_details { get; set; }
    }
}
