using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TaskApi.Entity;
using TaskApi.Request;
using TaskApi.Response;
using TaskApi.Service;
using TaskApi.Validator;

namespace TaskApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("add")]
        public IActionResult AddCustomer([FromBody] CustomerRequest customerRequest)
        {
            var validator = new CustomerValidator();
            var validationResult = validator.Validate(customerRequest);

            if (!validationResult.IsValid)
            {

                return BadRequest(new ResponseModel<string>(
                    status: false,
                    message: "Validation failed: " + string.Join(", ", validationResult.Errors)
                    ));
            }

            _customerService.AddCustomer(customerRequest);
            return Ok(new ResponseModel<string>(
                status: true,
                message: "Customer added successfully."
                ));
        }


        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int id)
        {
            int result = _customerService.GetCustomerById(id, out Customer customer);

            if (result == 0 || customer == null)
            {
                return NotFound(new ResponseModel<string>(
                    status: false,
                    message: "Customer not found")); 
            }

            return Ok(new ResponseModel<Customer>(
                status: true,
                message: "Customer retrieved successfully",
                data: customer
                )); 
        }


        [HttpGet("all")]
        public IActionResult GetCustomers()
        {
            var customers = _customerService.GetCustomers(); 
            if (customers == null || customers.Count == 0)
            {
                return NotFound(new ResponseModel<string> (
                    status: false,
                    message: "No customers found."
                    ));

            }

            return Ok(new ResponseModel<List<Customer>>(
                status: true,
                message: "Customers retrieved successfully.",
                data: customers
                ));
        }


        [HttpPut("update/{id}")]
        public IActionResult UpdateCustomer(int id, decimal newBalance)
        {
            if (newBalance <= 0)
            {
                return BadRequest(new ResponseModel<string>(
                    status: false,
                    message: "Balance must be greater than 0."
                ));
            }

            int result = _customerService.UpdateCustomer(id, newBalance);

            if (result == 0)
            {
                return NotFound(new ResponseModel<string>(
                    status: false,
                    message: $"No customer found with ID {id}."
                ));
            }

            return Ok(new ResponseModel<string>(
                status: true,
                message: $"Customer with ID {id} updated successfully."
          ));
        }


        [HttpDelete("delete/{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            int status = 0;
            _customerService.DeleteCustomer(id, out status);
            if (status == 1)
            {
                return Ok(new ResponseModel<string>(
                    status: true,
                    message: $"Customer with ID {id} deleted successfully."
                ));
            }
            else if (status == 0)
            {
                return NotFound(new ResponseModel<string>(
                    status: false,
                    message: $"No customer found with ID {id}."
                ));
            }
            else
            {
                return StatusCode(500, new ResponseModel<string>(
                    status: false,
                    message: "An error occurred while deleting the customer."
                ));
            }

        }

    }
}
