using AutoMapper;
using NGProjectAdmin.Entity.BusinessDTO.BusinessModule;
using NGProjectAdmin.Entity.BusinessDTO.NGBusiness;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
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

            //合同
            CreateMap<Contract_baseinfo, Assets_info_ContractDTO>().ReverseMap();

            //机构
            CreateMap<SysOrganization, SysOrganizationDTO>().ReverseMap(); 

            //用户
            CreateMap<SysUser, SysUserDTO>().ReverseMap();

            //菜单
            CreateMap<SysMenu, SysMenuDTO>().ReverseMap();

            //角色
            CreateMap<SysRole, SysRoleDTO>().ReverseMap();

            //数据字典
            CreateMap<SysCodeTable, SysCodeTableDTO>().ReverseMap();

            //导入配置
            CreateMap<SysImportConfig, ImportConfigDTO>().ReverseMap();

            //导入配置明细
            CreateMap<SysImportConfigDetail, ImportConfigDetailDTO>().ReverseMap();

            //同步账号模型
            CreateMap<BizAccount, BizAccountDTO>().ReverseMap();

            #endregion

            #region DTO TO POCO



            #endregion
        }
    }
}
