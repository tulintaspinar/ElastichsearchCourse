﻿using Elastichsearch.API.DTOs;
using Elastichsearch.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elastichsearch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductCreateDto request)
        {
            return CreateActionResult(await _productService.SaveAsync(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResult(await _productService.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return CreateActionResult(await _productService.GetByIdAsync(id));
        }
    }
}
