using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elasticsearch.WEB.Models;
using System.Collections.Immutable;

namespace Elasticsearch.WEB.Repositories
{
    public class BlogRepository
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        private readonly string _indexName = "blog";

        public BlogRepository(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }

        public async Task<Blog?> SaveAsync(Blog newBlog)
        {
            newBlog.Created = DateTime.Now;
            var response = await _elasticsearchClient.IndexAsync(newBlog, x=>x.Index(_indexName));
            if (!response.IsValidResponse) return null;
            newBlog.Id = response.Id;
            return newBlog;
        }

        public async Task<List<Blog>> SearchAsync(string searchText)
        {
            List<Action<QueryDescriptor<Blog>>> ListQuery = new();
            Action<QueryDescriptor<Blog>> matchAll = (q) => q.MatchAll(new MatchAllQuery());
            Action<QueryDescriptor<Blog>> matchContext = (q) => q.Match(m => m.Field(f => f.Content).Query(searchText));
            Action<QueryDescriptor<Blog>> matchTitle = (q) => q.MatchBoolPrefix(t => t.Field(f => f.Title).Query(searchText));
            Action<QueryDescriptor<Blog>> matcTags = (q) => q.Term(t => t.Field(f => f.Tags).Value(searchText));

            if(string.IsNullOrEmpty(searchText))
                ListQuery.Add(matchAll);
            else
            {
                ListQuery.Add(matchContext);
                ListQuery.Add(matchTitle);
                ListQuery.Add(matcTags);
            }

            var result = await _elasticsearchClient.SearchAsync<Blog>(s => s.Index(_indexName).Size(100)
            .Query(q => q
                .Bool(b => b
                    .Should(ListQuery.ToArray()))));
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToList();
        }
    }
}
