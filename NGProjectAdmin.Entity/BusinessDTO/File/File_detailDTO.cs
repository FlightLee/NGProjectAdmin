using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessDTO.File
{
    /// <summary>
    /// 文件信息主表
    /// </summary>
    public class File_detailDTO
    {
        /// <summary>
        /// 文件组Id
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string? FileUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int insideId { get; set; }
    }
}
