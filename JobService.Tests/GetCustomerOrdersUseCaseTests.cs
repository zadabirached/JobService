using FluentAssertions;
using JobService.Application.Interfaces;
using JobService.Application.UseCases;
using JobService.Domain.Entities;
using Moq;

namespace JobService.Tests;

public class GetCustomerOrdersUseCaseTests
{
    private readonly Mock<ICustomerOrderRepository> _repositoryMock;
    private readonly GetCustomerOrdersUseCase _useCase;

    public GetCustomerOrdersUseCaseTests()
    {
        _repositoryMock = new Mock<ICustomerOrderRepository>();
        _useCase = new GetCustomerOrdersUseCase(_repositoryMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsMatchingOrders()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var orderIds = new[] { "order-1", "order-2" };
        var expected = new List<CustomerOrder>
        {
            new() { Id = 1, OrderId = "order-1", TenantId = tenantId },
            new() { Id = 2, OrderId = "order-2", TenantId = tenantId }
        };
        _repositoryMock
            .Setup(r => r.GetByOrderIdsAsync(orderIds, tenantId))
            .ReturnsAsync(expected);

        // Act
        var result = await _useCase.ExecuteAsync(orderIds, tenantId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(o => o.OrderId == "order-1");
        result.Should().Contain(o => o.OrderId == "order-2");
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsEmptyList_WhenNoMatchingOrders()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var orderIds = new[] { "nonexistent" };
        _repositoryMock
            .Setup(r => r.GetByOrderIdsAsync(orderIds, tenantId))
            .ReturnsAsync(new List<CustomerOrder>());

        // Act
        var result = await _useCase.ExecuteAsync(orderIds, tenantId);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsEmptyList_WhenEmptyInputGiven()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var orderIds = Array.Empty<string>();
        _repositoryMock
            .Setup(r => r.GetByOrderIdsAsync(orderIds, tenantId))
            .ReturnsAsync(new List<CustomerOrder>());

        // Act
        var result = await _useCase.ExecuteAsync(orderIds, tenantId);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_CallsRepository_WithCorrectArguments()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var orderIds = new[] { "order-1" };
        _repositoryMock
            .Setup(r => r.GetByOrderIdsAsync(orderIds, tenantId))
            .ReturnsAsync(new List<CustomerOrder>());

        // Act
        await _useCase.ExecuteAsync(orderIds, tenantId);

        // Assert
        _repositoryMock.Verify(r => r.GetByOrderIdsAsync(orderIds, tenantId), Times.Once);
    }
}