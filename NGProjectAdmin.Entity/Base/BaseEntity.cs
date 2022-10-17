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
    public class BaseEntity
    {
        #region 通用属性

        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [SugarColumn(IsPrimaryKey = true, ColumnName = "Id", IsIdentity = true)]
        public int Id { get; set; }

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
        public void Create(IHttpContextAccessor context)
        {       
            var user = NGAdminSessionContext.GetCurrentUserInfo(context);        
        }

        /// <summary>
        /// 编辑赋值
        /// </summary>
        /// <param name="context">HttpContext</param>
        public void Modify(IHttpContextAccessor context)
        {
            var user = NGAdminSessionContext.GetCurrentUserInfo(context);
        }

        /// <summary>
        /// 逻辑删除赋值
        /// </summary>
        /// <param name="context">HttpContext</param>
        public void Delete(IHttpContextAccessor context)
        {
            var user = NGAdminSessionContext.GetCurrentUserInfo(context);
        }

        #endregion
    }
}
