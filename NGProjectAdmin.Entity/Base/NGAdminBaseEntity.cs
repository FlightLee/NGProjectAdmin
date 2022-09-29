using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NGProjectAdmin.Common.Class.Excel;
using NGProjectAdmin.Entity.CommonEnum;
using NGProjectAdmin.Entity.CoreEntity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.Base
{
    public class NGAdminBaseEntity
    {
        #region 通用属性

        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [SugarColumn(IsPrimaryKey = true, ColumnName = "ID")]
        public Guid Id { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(512)]
        [SugarColumn(ColumnName = "REMARK")]
        [ExcelExport("备注")]
        [ExcelImport("备注")]
        public String Remark { get; set; }

        /// <summary>
        /// 逻辑标志位
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "ISDEL")]
        public int IsDel { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "CREATOR")]
        public Guid Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "CREATE_TIME")]
        [ExcelExport("创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 编辑人员
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "MODIFIER")]
        public Guid Modifier { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "MODIFY_TIME")]
        public DateTime ModifyTime { get; set; }


        /// <summary>
        /// 版本编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "VERSION_ID")]
        public Guid VersionId { get; set; }

        #endregion

        #region 通用方法 

        /// <summary>
        /// 转化为json
        /// </summary>
        /// <returns>json字符串</returns>
        public String ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// 创建赋值
        /// </summary>
        /// <param name="context">HttpContext</param>
        public void Create(IHttpContextAccessor context)
        {
            this.Id = Guid.NewGuid();

            this.IsDel = (int)DeletionType.Undeleted;

            var user = NGAdminSessionContext.GetCurrentUserInfo(context);

            this.Creator = user.Id;
            this.CreateTime = DateTime.Now;

            this.Modifier = user.Id;
            this.ModifyTime = DateTime.Now;

            this.VersionId = Guid.NewGuid();
        }

        /// <summary>
        /// 编辑赋值
        /// </summary>
        /// <param name="context">HttpContext</param>
        public void Modify(IHttpContextAccessor context)
        {
            var user = NGAdminSessionContext.GetCurrentUserInfo(context);

            this.Modifier = user.Id;
            this.ModifyTime = DateTime.Now;

            this.VersionId = Guid.NewGuid();
        }

        /// <summary>
        /// 逻辑删除赋值
        /// </summary>
        /// <param name="context">HttpContext</param>
        public void Delete(IHttpContextAccessor context)
        {
            this.IsDel = (int)DeletionType.Deleted;

            var user = NGAdminSessionContext.GetCurrentUserInfo(context);

            this.Modifier = user.Id;
            this.ModifyTime = DateTime.Now;

            this.VersionId = Guid.NewGuid();
        }

        #endregion
    }
}
