using Elasticsearch.WEB.Repositories;

namespace Elasticsearch.WEB.Services
{
    public class BlogServices
    {
        private readonly BlogRepository _blogRepository;

        public BlogServices(BlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
    }
}
