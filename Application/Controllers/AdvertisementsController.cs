﻿using Application.DTOs.InputModels;
using Application.DTOs.ViewModels;
using Application.Extensions;
using Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdvertsmentsController : ControllerBase
    {
        private readonly IAdvertisementService _advertisementService;

        public AdvertsmentsController(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAdvertisementRequest request, CancellationToken cancellationToken)
        {
            var result = await _advertisementService.CreateNewAdvertisement(request, cancellationToken);

            return result.ToHttpResponse();
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await _advertisementService.GetAllUsersAdvertisements(cancellationToken);

            return Ok(result);
        }
    }
}
