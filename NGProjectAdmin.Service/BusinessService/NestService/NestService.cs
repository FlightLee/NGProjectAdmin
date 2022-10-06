using Nest;
using NGProjectAdmin.Repository.BusinessRepository.NestRepository;

namespace NGProjectAdmin.Service.BusinessService.NestService
{
    /// <summary>
    /// Nest服务层实现
    /// </summary>
    public class NestService : INestService
    {
        /// <summary>
        /// Nest仓储层实例
        /// </summary>
        private readonly INestRepository NestRepository;

        public NestService(INestRepository NestRepository)
        {
            this.NestRepository = NestRepository;
        }

        /// <summary>
        /// 获取ElasticClient实例
        /// </summary>
        /// <returns>ElasticClient</returns>
        public ElasticClient GetElasticClient()
        {
            return this.NestRepository.GetElasticClient();
        }
    }
}
