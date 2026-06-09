using JobService.Application.Interfaces;
using JobService.Domain.Entities;
using JobService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobService.Infrastructure.Repositories;

public class CustomerOrderRepository : ICustomerOrderRepository
{
    private readonly JobServiceDbContext _context;

    public CustomerOrderRepository(JobServiceDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CustomerOrder>> GetByOrderIdsAsync(IEnumerable<string> orderIds, Guid tenantId)
    {
        return await _context.CustomerOrders
            .Where(o => orderIds.Contains(o.OrderId) && o.TenantId == tenantId)
            .ToListAsync();
    }
}