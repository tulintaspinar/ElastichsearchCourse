using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastichsearch.API.Model.ECommerceModel;
using System.Collections.Immutable;

namespace Elastichsearch.API.Repositories
{
    public class ECommerceRepository
    {
        private readonly ElasticsearchClient _client;
        private const string _indexName = "kibana_sample_data_ecommerce";

        public ECommerceRepository(ElasticsearchClient client)
        {
            _client = client;
        }
        public async Task<ImmutableList<ECommerce>> TermLevelQueryAsync(string customerFirstName)
        {
            //first way
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).Query(q =>
            //q.Term(t => t.Field("customer_first_name.keyword"!).Value(customerFirstName))));

            //second way
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).
            Query(q => q.Term(t => t.Field(f => f.CustomerFirstName.Suffix("keyword")).Value(customerFirstName))));

            //third way 
            //var termQury = new TermQuery("customer_first_name.keyword"!) { Value = customerFirstName, CaseInsensitive = true };
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).Query(termQury));


            GetId(result);
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> TermsQueryAsync(List<string> customerFirstNameList)
        {
            List<FieldValue> terms = new List<FieldValue>();
            customerFirstNameList.ForEach(x => { terms.Add(x); });

            // 1. way
            //var termsQuery = new TermsQuery()
            //{
            //    Field = "customer_first_name.keyword"!,
            //    Term = new TermsQueryField(terms.AsReadOnly())
            //};
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).Query(termsQuery));

            // 2. way
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).Size(20)
            .Query(q => q
            .Terms(t => t
            .Field(f => f.CustomerFirstName
            .Suffix("keyword")).Term(new TermsQueryField(terms.AsReadOnly())))));

            GetId(result);
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> PrefixQueryAsync(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).Query(q => q.Prefix(p => p.Field(f => f.CustomerFullName.Suffix("keyword")).Value(customerFullName))));
            GetId(result);
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> RangeQueryAsync(double FromPrice, double ToPrice)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).Size(50)
            .Query(q => q
                .Range(r => r.NumberRange(nr => nr.Field(f => f.TaxFullTotalPrice).Gte(FromPrice).Lte(ToPrice)))));
            
            GetId(result);
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchAllQueryAsync()
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).Size(5)
            .Query(q => q.MatchAll(new MatchAllQuery())));

            GetId(result);
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> PaginationQueryAsync(int page=1, int pageSize=3)
        {
            var pageFrom = (page -1) * pageSize;
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName)
            .Size(pageSize).From(pageFrom)
            .Query(q => q.MatchAll(new MatchAllQuery())));

            GetId(result);
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> WildCardQueryAsync(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName)
            .Query(q => q.Wildcard(w => w.Field(f => f.CustomerFullName.Suffix("keyword")).Wildcard(customerFullName))));

            GetId(result);
            return result.Documents.ToImmutableList();
        }
        public async Task<ImmutableList<ECommerce>> FuzzyQueryAsync(string customerName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName)
            .Query(q => q.Fuzzy(f => f.Field( f => f.CustomerFirstName.Suffix("keyword")).Value(customerName).Fuzziness(new Fuzziness(2))))
            .Sort(sort=>sort.Field(f=>f.TaxFullTotalPrice,new FieldSort() { Order = SortOrder.Desc })));

            GetId(result);
            return result.Documents.ToImmutableList();
        }

        private static void GetId(SearchResponse<ECommerce> result)
        {
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
        }
    }
}
