using Dapper;
using EcommerceBackend.Application.Common;
using EcommerceBackend.Application.DTOs.Categories;
using MediatR;
using System.Data;

namespace EcommerceBackend.Application.Features.Categories.Queries
{
    public class GetAllCategoriesQuery : IRequest<PaginatedResponse<CategoryDto>>
    {
        public int PageNumber { get; set; } = 1; // Default to page 1
        public int PageSize { get; set; } = 10; // Default to 10 items per page
        public string SearchTerm { get; set; } // Optional search term
        public string SortBy { get; set; } // Optional sorting field
        public bool SortAscending { get; set; } = true; // Default to ascending order
    }
}
