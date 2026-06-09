using JobService.Domain.Entities;

namespace JobService.Application.Interfaces;

public interface ICustomerOrderRepository
{
    Task<IEnumerable<CustomerOrder>> GetByOrderIdsAsync(IEnumerable<string> orderIds, Guid tenantId);
}