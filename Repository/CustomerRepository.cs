using Npgsql;
using TaskApi.Entity;

namespace TaskApi.Repository
{
    public class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public int AddCustomer(string firstName, string lastName, string email, decimal balance)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT public.fn_add_customer(@FirstName, @LastName, @Email, @Balance)", connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Balance", balance);

                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            return Convert.ToInt32(result);  
                        }
                        else
                        {
                            Console.WriteLine("Error: Function returned null");
                            return 0; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddCustomer: {ex.Message}");
                return 0; 
            }
        }



        public int GetCustomerById(int id, out Customer customer)
        {
            customer = null;
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT * FROM Customers WHERE Id = @Id", connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                customer = new Customer
                                {
                                    Id = (int)reader["Id"],
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Balance = (decimal)reader["Balance"]
                                };
                                return 1; 
                            }
                            else
                            {
                                return 0; 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCustomerById: {ex.Message}");
                return 0; 
            }
        }

        public List<Customer> GetCustomers()
        {
            var customers = new List<Customer>();
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT * FROM public.fn_get_customers()", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customers.Add(new Customer
                                {
                                    Id = (int)reader["id"],
                                    FirstName = reader["first_name"].ToString(),
                                    LastName = reader["last_name"].ToString(),
                                    Email = reader["email"].ToString(),
                                    Balance = (decimal)reader["balance"]
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCustomers: {ex.Message}");
            }

            return customers;
        }



        public int UpdateCustomer(int id, decimal newBalance)
        {
            if (newBalance <= 0)
            {
                Console.WriteLine("Balance must be greater than 0.");
                return 0; 
            }

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand("SELECT public.fn_update_customer_balance(@Id, @Balance)", connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Balance", newBalance);

                        int result = (int)command.ExecuteScalar();

                        if (result == 1) 
                        {
                            Console.WriteLine($"Customer with ID {id} updated successfully.");
                        }
                        else if (result == 0) 
                        {
                            Console.WriteLine($"Customer with ID {id} not found.");
                        }

                        return result;
                    }
                }
            }
            catch (NpgsqlException npgEx)
            {
                Console.WriteLine($"Database error in UpdateCustomer: {npgEx.Message}");
                return 0; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error in UpdateCustomer: {ex.Message}");
                return 0; 
            }
        }



        public int DeleteCustomer(int id, out int status)
        {
            status = 0;

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT public.fn_delete_customer(@Id)", connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            status = Convert.ToInt32(result);
                        }
                    }
                }

                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteCustomer: {ex.Message}");
                return 0; 
            }
        }


    }
}


/*CALL AddCustomer('John', 'Doe', 'john.doe@example.com', 100.0); SELECT public.fn_add_customer('Sarkhan', 'Ismayilov', 'sarkhan.ism@gmail.com', 1500)
CALL GetCustomerById(1);
CALL GetCustomers();
CALL UpdateCustomerBalance(1, 150.0);
CALL DeleteCustomer(1);*/