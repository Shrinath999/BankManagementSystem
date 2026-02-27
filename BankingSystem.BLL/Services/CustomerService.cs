using BankingSystem.Entities.Models;
using static BankingSystem.DAL.Repositorie.IGenericRepository;

namespace BankingSystem.BLL.Services
{
    public class CustomerService : ICustomerService
    {
    private readonly IGenericRepository<Customer> _customerRepository;

    public CustomerService(IGenericRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<Customer> GetCustomerByIdAsync(int id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task CreateCustomerAsync(Customer customer)
    {
        customer.CreatedDate = DateTime.Now;
        await _customerRepository.InsertAsync(customer);
        await _customerRepository.SaveAsync();
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        _customerRepository.Update(customer);
        await _customerRepository.SaveAsync();
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer != null)
        {
            _customerRepository.Delete(customer);
            await _customerRepository.SaveAsync();
        }
    }
}
}
