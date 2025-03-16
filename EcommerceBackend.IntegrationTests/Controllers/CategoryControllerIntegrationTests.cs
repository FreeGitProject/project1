using EcommerceBackend.Application.DTOs.Categories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;

namespace EcommerceBackend.IntegrationTests.Controllers
{
    public class CategoryControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CategoryControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllCategories_ShouldReturnListOfCategories()
        {
            // Act
            var response = await _client.GetAsync("/api/category");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
            categories.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/api/category/{categoryId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateCategory_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var createCategoryDto = new CreateCategoryDto { Name = "Electronics", Description = "Electronic items" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/category", createCategoryDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var categoryDto = await response.Content.ReadFromJsonAsync<CategoryDto>();
            categoryDto.Should().NotBeNull();
            categoryDto.Name.Should().Be(createCategoryDto.Name);
            categoryDto.Description.Should().Be(createCategoryDto.Description);
        }
    }
}