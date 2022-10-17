using NGProjectAdmin.Entity.Base;
using NGProjectAdmin.Entity.BusinessDTO.File;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessEntity.NGBusiness
{
    /// <summary>
    /// 文件信息主表
    /// </summary>
    [SugarTable("file_group")]
    public class File_group : BaseEntity
    {



        [Navigate(NavigateType.OneToMany, nameof(File_detail.FileId))]
        public List<File_detail> File_details { get; set; }
    }
}
