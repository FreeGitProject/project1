using Dapper;
using EcommerceBackend.Application.DTOs.Categories;
using MediatR;
using System.Data;

namespace EcommerceBackend.Application.Features.Categories.Queries
{
    public class GetCategoryByIdQuery : IRequest<CategoryDto>
    {
        public Guid Id { get; set; }
    }

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly IDbConnection _dbConnection;

        public GetCategoryByIdQueryHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    id AS Id, 
                    name AS Name, 
                    description AS Description, 
                    created_at AS CreatedAt, 
                    updated_at AS UpdatedAt 
                FROM categories
                WHERE id = @Id"; // snake_case with aliasing for Dapper mapping

            try
            {
                var category = await _dbConnection
                    .QuerySingleOrDefaultAsync<CategoryDto>(sql, new { Id = request.Id }, commandTimeout: 30);

                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with ID '{request.Id}' was not found.");
                }

                return category;
            }
            catch (Exception ex)
            {
                // Consider adding proper logging here
                Console.WriteLine($"Error fetching category: {ex.Message}");
                throw; // Rethrow to preserve the original stack trace
            }
        }
    }
}
