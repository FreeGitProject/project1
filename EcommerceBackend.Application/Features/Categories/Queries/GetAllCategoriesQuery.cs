using Dapper;
using EcommerceBackend.Application.DTOs.Categories;
using MediatR;
using System.Data;

namespace EcommerceBackend.Application.Features.Categories.Queries
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>> { }

    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly IDbConnection _dbConnection;

        public GetAllCategoriesQueryHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    id AS Id, 
                    name AS Name, 
                    description AS Description, 
                    created_at AS CreatedAt, 
                    updated_at AS UpdatedAt 
                FROM categories"; // snake_case with proper aliasing

            try
            {
                var categories = await _dbConnection.QueryAsync<CategoryDto>(sql, commandTimeout: 30);
                return categories;
            }
            catch (Exception ex)
            {
                // Consider using logging here
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                throw; // Rethrow to maintain stack trace
            }
        }
    }
}
