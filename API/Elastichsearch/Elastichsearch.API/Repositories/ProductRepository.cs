﻿using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastichsearch.API.DTOs;
using Elastichsearch.API.Model;
using System.Collections.Immutable;

namespace Elastichsearch.API.Repositories
{
    public class ProductRepository
    {
        private readonly ElasticsearchClient _client;
        private const string _indexName = "products";

        public ProductRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<Product?> SaveAsync(Product newProduct)
        {
            newProduct.Created=DateTime.Now;
            var response = await _client.IndexAsync(newProduct, x => x.Index(_indexName).Id(Guid.NewGuid().ToString()));
            //fast fail
            if (!response.IsValidResponse)
                return null;
            newProduct.Id = response.Id;
            return newProduct;
        }

        public async Task<ImmutableList<Product>> GetAllAsync()
        {
            var result = await _client.SearchAsync<Product>(s => s.Index(_indexName).Query(q=>q.MatchAll(new MatchAllQuery())));
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            var response = await _client.GetAsync<Product>(id, x => x.Index(_indexName));
            if(!response.IsValidResponse)
                return null;
            response.Source.Id = response.Id;
            return response.Source;
        }

        public async Task<bool> UpdateAsync(ProductUpdateDto updateProduct)
        {
            var response = await _client.UpdateAsync<Product, ProductUpdateDto>(_indexName,updateProduct.id, x => x.Doc(updateProduct));
            return response.IsValidResponse;
        }
        public async Task<DeleteResponse> DeleteAsync(string id)
        {
            var response = await _client.DeleteAsync<Product>(id, x => x.Index(_indexName));
            return response;
        }
    }
}
