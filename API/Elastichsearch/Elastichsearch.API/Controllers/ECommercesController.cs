﻿using Elastichsearch.API.Model.ECommerceModel;
using Elastichsearch.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace Elastichsearch.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ECommercesController : ControllerBase
    {
        private readonly ECommerceRepository _repository;

        public ECommercesController(ECommerceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> MatchAllQuery()
        {
            return Ok(await _repository.MatchAllQueryAsync());
        }
        [HttpGet]
        public async Task<IActionResult> TermQuery(string customerFirstName)
        {
            return Ok(await _repository.TermLevelQueryAsync(customerFirstName));
        }

        [HttpPost]
        public async Task<IActionResult> TermsQuery(List<string> customerFirstNameList)
        {
            return Ok(await _repository.TermsQueryAsync(customerFirstNameList));
        }

        [HttpGet]
        public async Task<IActionResult> PrefixQuery(string customerFullName)
        {
            return Ok(await _repository.PrefixQueryAsync(customerFullName));
        }

        [HttpPost]
        public async Task<IActionResult> RangeQuery(double FromPrice, double ToPrice)
        {
            return Ok(await _repository.RangeQueryAsync(FromPrice,ToPrice));
        }
    }
}