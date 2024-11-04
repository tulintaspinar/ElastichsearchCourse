using Elasticsearch.WEB.Models;
using Elasticsearch.WEB.Repositories;
using Elasticsearch.WEB.ViewModel;

namespace Elasticsearch.WEB.Services
{
    public class BlogServices
    {
        private readonly BlogRepository _blogRepository;

        public BlogServices(BlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<bool> SaveAsync(BlogCreateViewModel model)
        {
            var newBlog = new Blog()
            {
                Content = model.Content,
                Title = model.Title,
                UserId = Guid.NewGuid(),
                Tags = model.Tags.Split(",")
            };

            var isCreated = await _blogRepository.SaveAsync(newBlog);
            return isCreated != null;
        }
        public async Task<List<BlogViewModel>> SearchAsync(string searchText)
        {
            var blogList = await _blogRepository.SearchAsync(searchText);
            return blogList.Select(b => new BlogViewModel()
            {
               Id = b.Id,
               UserId = b.UserId.ToString(),
               Title = b.Title,
               Content = b.Content,
               Tags = String.Join(",", b.Tags),
               Created = b.Created.ToShortDateString()
            }).ToList();
        }
    }
}
