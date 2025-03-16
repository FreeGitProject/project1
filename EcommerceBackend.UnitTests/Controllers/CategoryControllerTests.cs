

using EcommerceBackend.Application.DTOs.Categories;
using EcommerceBackend.Application.Features.Categories.Commands;
using EcommerceBackend.Application.Features.Categories.Queries;
using EcommerceBackend.Presentation.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EcommerceBackend.UnitTests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new CategoryController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllCategories_ShouldReturnListOfCategories()
        {
            // Arrange
            var categories = new List<CategoryDto>
            {
                new CategoryDto { Id = Guid.NewGuid(), Name = "Electronics", Description = "Electronic items" },
                new CategoryDto { Id = Guid.NewGuid(), Name = "Clothing", Description = "Clothing items" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllCategoriesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(categories);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new CategoryDto { Id = categoryId, Name = "Electronics", Description = "Electronic items" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            // Act
            var result = await _controller.GetCategoryById(categoryId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(category);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CategoryDto)null);

            // Act
            var result = await _controller.GetCategoryById(categoryId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateCategory_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var createCategoryDto = new CreateCategoryDto { Name = "Electronics", Description = "Electronic items" };
            var categoryDto = new CategoryDto { Id = Guid.NewGuid(), Name = "Electronics", Description = "Electronic items" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(categoryDto);

            // Act
            var result = await _controller.CreateCategory(createCategoryDto);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ActionName.Should().Be(nameof(CategoryController.GetCategoryById));
            createdAtActionResult.Value.Should().BeEquivalentTo(categoryDto);
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnOk_WhenCategoryExists()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var updateCategoryDto = new UpdateCategoryDto { Name = "Updated Electronics", Description = "Updated electronic items" };
            var categoryDto = new CategoryDto { Id = categoryId, Name = "Updated Electronics", Description = "Updated electronic items" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(categoryDto);

            // Act
            var result = await _controller.UpdateCategory(categoryId, updateCategoryDto);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(categoryDto);
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var updateCategoryDto = new UpdateCategoryDto { Name = "Updated Electronics", Description = "Updated electronic items" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CategoryDto)null);

            // Act
            var result = await _controller.UpdateCategory(categoryId, updateCategoryDto);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteCategory_ShouldReturnNoContent()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.DeleteCategory(categoryId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}