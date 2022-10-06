using Nest;

namespace NGProjectAdmin.Repository.BusinessRepository.NestRepository
{
    /// <summary>
    /// 
    /// </summary>
    public interface INestRepository
    {
        /// <summary>
        /// 获取ElasticClient实例
        /// </summary>
        /// <returns>ElasticClient</returns>
        ElasticClient GetElasticClient();
    }
}
