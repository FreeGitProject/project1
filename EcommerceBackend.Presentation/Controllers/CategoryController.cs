using EcommerceBackend.Application.Common;
using EcommerceBackend.Application.DTOs.Categories;
using EcommerceBackend.Application.Features.Categories.Commands;
using EcommerceBackend.Application.Features.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackend.Presentation.Controllers
{
    [Authorize] // Only Admins can access these endpoints
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/category
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<CategoryDto>>>> GetAllCategories(
       [FromQuery] int pageNumber = 1,
       [FromQuery] int pageSize = 10,
       [FromQuery] string searchTerm = null,
       [FromQuery] string sortBy = null,
       [FromQuery] bool sortAscending = true)
        {
            var query = new GetAllCategoriesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                SortBy = sortBy,
                SortAscending = sortAscending
            };

            var result = await _mediator.Send(query);
            return Ok(ApiResponse<PaginatedResponse<CategoryDto>>.Success(result));
        }

        // GET: api/category/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategoryById(Guid id)
        {
            var query = new GetCategoryByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(ApiResponse<CategoryDto>.Failure(
                    message: "Category not found.",
                    messageCode: "CATEGORY_NOT_FOUND"
                ));
            }

            return Ok(ApiResponse<CategoryDto>.Success(result));
        }

        // POST: api/category
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            var command = new CreateCategoryCommand { CreateCategoryDto = createCategoryDto };
            var result = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = result.Id },
                ApiResponse<CategoryDto>.Success(result)
            );
        }

        // PUT: api/category/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> UpdateCategory(Guid id, UpdateCategoryDto updateCategoryDto)
        {
            var command = new UpdateCategoryCommand { Id = id, UpdateCategoryDto = updateCategoryDto };
            var result = await _mediator.Send(command);

            if (result == null)
            {
                return NotFound(ApiResponse<CategoryDto>.Failure(
                    message: "Category not found.",
                    messageCode: "CATEGORY_NOT_FOUND"
                ));
            }

            return Ok(ApiResponse<CategoryDto>.Success(result));
        }

        // DELETE: api/category/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCategory(Guid id)
        {
            var command = new DeleteCategoryCommand { Id = id };
            await _mediator.Send(command);

            return NoContent(); // Return 204 No Content with no response body
        }
    }
}
