using Nest;
using NGProjectAdmin.Common.Utility;

namespace NGProjectAdmin.Repository.BusinessRepository.NestRepository
{
    /// <summary>
    /// 
    /// </summary>
    public class NestRepository : INestRepository
    {
        /// <summary>
        /// 获取ElasticClient实例
        /// </summary>
        /// <returns>ElasticClient</returns>
        public ElasticClient GetElasticClient()
        {
            return NGEsNestContext.Instance;
        }
    }
}
