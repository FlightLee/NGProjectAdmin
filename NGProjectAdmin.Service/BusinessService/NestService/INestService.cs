using Nest;

namespace NGProjectAdmin.Service.BusinessService.NestService
{
    /// <summary>
    /// Nest服务层接口
    /// </summary>
    public interface INestService
    {
        /// <summary>
        /// 获取ElasticClient实例
        /// </summary>
        /// <returns>ElasticClient</returns>
        ElasticClient GetElasticClient();
    }
}
