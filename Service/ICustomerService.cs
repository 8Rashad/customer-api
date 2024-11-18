using TaskApi.Entity;
using TaskApi.Request;

namespace TaskApi.Service
{
    public interface ICustomerService
    {
        void AddCustomer(CustomerRequest customerRequest);
        List<Customer> GetCustomers(); 
        int UpdateCustomer(int id, decimal newBalance);
        void DeleteCustomer(int id, out int status);
        int GetCustomerById(int id, out Customer customer); 

    }
}
