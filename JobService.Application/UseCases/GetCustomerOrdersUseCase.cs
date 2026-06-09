using JobService.Application.Interfaces;
using JobService.Domain.Entities;

namespace JobService.Application.UseCases;

public class GetCustomerOrdersUseCase
{
    private readonly ICustomerOrderRepository _repository;

    public GetCustomerOrdersUseCase(ICustomerOrderRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<CustomerOrder>> ExecuteAsync(IEnumerable<string> orderIds, Guid tenantId)
    {
        return _repository.GetByOrderIdsAsync(orderIds, tenantId);
    }
}