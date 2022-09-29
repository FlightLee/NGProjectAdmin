//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using System;

namespace NGProjectAdmin.Entity.BusinessDTO.SystemManagement
{
    public class QuartzJobDTO
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public Guid JobId { get; set; }

        /// <summary>
        /// 集群节点编号
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// 预执行动作
        /// </summary>
        public string Action { get; set; }

    }
}
