
using FluentValidation;
using TaskApi.Entity;
using TaskApi.Repository;
using TaskApi.Request;

namespace TaskApi.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerService(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public void AddCustomer(CustomerRequest customerRequest)
        {
            _customerRepository.AddCustomer(customerRequest.FirstName, customerRequest.LastName, customerRequest.Email, customerRequest.Balance);
        }

        public List<Customer> GetCustomers()
        {
            var customerList = _customerRepository.GetCustomers();

            if (customerList == null || customerList.Count == 0)
            {
                return new List<Customer>(); 
            }

            return customerList;
        }

        public int UpdateCustomer(int id, decimal newBalance)
        {
            return _customerRepository.UpdateCustomer(id, newBalance);
        }

        public void DeleteCustomer(int id, out int status)
        {
            status = _customerRepository.DeleteCustomer(id, out status); 
        }

        public int GetCustomerById(int id, out Customer customer)
        {
            return _customerRepository.GetCustomerById(id, out customer);
        }

    }
}
