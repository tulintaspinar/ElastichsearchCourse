using Elastichsearch.API.DTOs;
using Elastichsearch.API.Model;
using Elastichsearch.API.Repositories;
using System.Collections.Immutable;
using System.Net;

namespace Elastichsearch.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
        {
            var responseProduct = await _productRepository.SaveAsync(request.CreateProduct());
            if (responseProduct == null)
                return ResponseDto<ProductDto>.Fail("Kayıt esnasında bir hata meydana geldi.", HttpStatusCode.InternalServerError);
            return ResponseDto<ProductDto>.Success(responseProduct.CreateDto(),HttpStatusCode.Created);
        }
        public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            if (products == null)
                return ResponseDto<List<ProductDto>>.Fail("Listeleme sırasında hata meydana geldi.", HttpStatusCode.InternalServerError);

            var productsListDto = new List<ProductDto>();
            foreach (var item in products)
            {
                if (item.Feature is null)
                {
                    productsListDto.Add(new ProductDto(item.Id, item.Name, item.Price, item.Stock, null));
                    continue;
                }
                productsListDto.Add(new ProductDto(item.Id, item.Name, item.Price, item.Stock, new ProductFeatureDto(item.Feature!.Width, item.Feature!.Height, item.Feature!.Color.ToString())));
            }
            return ResponseDto<List<ProductDto>>.Success(productsListDto,HttpStatusCode.OK);
        }

        public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
        {
            var hasProduct = await _productRepository.GetByIdAsync(id);
            if (hasProduct == null)
                return ResponseDto<ProductDto>.Fail( "Ürün bulunamadı.", HttpStatusCode.NotFound);
            return ResponseDto<ProductDto>.Success(hasProduct.CreateDto(),HttpStatusCode.OK);
        }
    }
}
