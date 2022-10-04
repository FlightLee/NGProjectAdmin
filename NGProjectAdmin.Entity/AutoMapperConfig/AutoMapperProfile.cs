using AutoMapper;
using NGProjectAdmin.Entity.BusinessDTO.BusinessModule;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;

namespace NGProjectAdmin.Entity.AutoMapperConfig
{
    /// <summary>
    /// 映射描述文件
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region POCO TO DTO

            //机构
            CreateMap<SysOrganization, SysOrganizationDTO>();

            //用户
            CreateMap<SysUser, SysUserDTO>();

            //菜单
            CreateMap<SysMenu, SysMenuDTO>();

            //角色
            CreateMap<SysRole, SysRoleDTO>();

            //数据字典
            CreateMap<SysCodeTable, SysCodeTableDTO>();

            //导入配置
            CreateMap<SysImportConfig, ImportConfigDTO>();

            //导入配置明细
            CreateMap<SysImportConfigDetail, ImportConfigDetailDTO>();

            //同步账号模型
            CreateMap<BizAccount, BizAccountDTO>();

            #endregion

            #region DTO TO POCO



            #endregion
        }
    }
}
