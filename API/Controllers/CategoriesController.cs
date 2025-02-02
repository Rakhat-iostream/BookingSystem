﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Helpers.Exceptions;
using Domain.Core.Models.Booking;
using Domain.Dtos.Booking;
using Domain.Interfaces.Services.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    [Route("api/v1/categories")]
    //[Authorize(Roles = Roles.ADMIN)]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoryService;
        private readonly IMapper _mapper;
        public CategoriesController(IMapper mapper, ICategoriesService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<LeisureServiceCategoryViewDto>> GetAllCategoriesAsync()
        {
            return _mapper.Map<IEnumerable<LeisureServiceCategoryViewDto>>(await _categoryService.GetAllAsync());
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<LeisureServiceCategoryViewDto> GetLeisureServiceCategoryByIdAsync(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return category == null ? null : _mapper.Map<LeisureServiceCategoryViewDto>(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeisureServiceCategoryAsync(LeisureServiceCategoryCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.First().Errors.First();
                return BadRequest(error.ErrorMessage);
            }
            try
            {
                var model = _mapper.Map<Category>(dto);
                await _categoryService.CreateAsync(model);
                return Ok("Created new leisure service category");
            } catch (AlreadyPresentException e)
            {
                return BadRequest(e.Message);
            } catch (Exception)
            {
                return StatusCode(500, "Failed to create leisure service category");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeisureServiceCategoryAsync(LeisureServiceCategoryUpdateDto dto, Guid id)
        {
            var model = await _categoryService.GetByIdAsync(id);
            if (model == null) return BadRequest("Failed to find leisure service category by id " + id);
            if (string.IsNullOrEmpty(dto.Name)) return BadRequest("Enter new category name");
            model.Name = dto.Name;
            if (await _categoryService.UpdateAsync(model)) return Ok("Updated leisure service category by id " + id);
            return BadRequest("Failed to update leisure service category by id" + id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeisureServiceCategoryAsync(Guid id)
        {
            var model = await _categoryService.GetByIdAsync(id);
            if (model == null) return BadRequest("Failed to find leisure service category by id " + id);
            if (await _categoryService.DeleteAsync(model)) return Ok("Deleted leisure service category by id " + id);
            return BadRequest("Failed to delete leisure service category by id " + id);
        }
    }
}
