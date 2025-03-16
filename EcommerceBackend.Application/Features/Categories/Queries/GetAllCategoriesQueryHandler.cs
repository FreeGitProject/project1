using Dapper;
using EcommerceBackend.Application.Common;
using EcommerceBackend.Application.DTOs.Categories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EcommerceBackend.Application.Features.Categories.Queries
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, PaginatedResponse<CategoryDto>>
    {
        private readonly IDbConnection _dbConnection;

        public GetAllCategoriesQueryHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<PaginatedResponse<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            // Base query with snake_case naming and better handling for NULL search terms
            const string sql = @"
                SELECT 
                    id AS Id,
                    name AS Name,
                    description AS Description,
                    created_at AS CreatedAt,
                    updated_at AS UpdatedAt
                FROM categories
                WHERE 
                    @SearchTerm IS NULL 
                    OR name ILIKE @SearchTerm 
                    OR description ILIKE @SearchTerm
                ORDER BY 
                    CASE WHEN @SortBy = 'Name' AND @SortAscending = TRUE THEN name END ASC,
                    CASE WHEN @SortBy = 'Name' AND @SortAscending = FALSE THEN name END DESC,
                    CASE WHEN @SortBy = 'CreatedAt' AND @SortAscending = TRUE THEN created_at END ASC,
                    CASE WHEN @SortBy = 'CreatedAt' AND @SortAscending = FALSE THEN created_at END DESC
                OFFSET @Offset 
                LIMIT @PageSize;

                SELECT COUNT(*)
                FROM categories
                WHERE 
                    @SearchTerm IS NULL 
                    OR name ILIKE @SearchTerm 
                    OR description ILIKE @SearchTerm;";

            // Calculate offset for pagination
            var offset = (request.PageNumber - 1) * request.PageSize;

            // Parameters for the query
            var parameters = new
            {
                SearchTerm = string.IsNullOrWhiteSpace(request.SearchTerm)
                    ? null
                    : $"%{request.SearchTerm}%",
                SortBy = request.SortBy,
                SortAscending = request.SortAscending,
                Offset = offset,
                PageSize = request.PageSize
            };

            try
            {
                using var multi = await _dbConnection.QueryMultipleAsync(
                    sql,
                    parameters,
                    commandTimeout: 30 // Prevent long-running queries
                );

                var categories = await multi.ReadAsync<CategoryDto>();
                var totalRecords = await multi.ReadSingleAsync<int>();

                return new PaginatedResponse<CategoryDto>(
                    data: categories.ToList(),
                    pageNumber: request.PageNumber,
                    pageSize: request.PageSize,
                    totalRecords: totalRecords
                );
            }
            catch (Exception ex)
            {
                // Consider proper logging here
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                throw; // Rethrow to preserve the original stack trace
            }
        }
    }
}
