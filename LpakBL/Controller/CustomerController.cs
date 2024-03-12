using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using LpakBL.Model;
using System.Configuration;
namespace LpakBL.Controller
{
    public class CustomerController
    {
        private string ConnectionString { get; }

        public CustomerController(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public CustomerController()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["LpakTestingDatabaseConnection"].ConnectionString;
        }
        
        public async Task<List<Customer>> GetAsync()
        {
            List<Customer> customers = new List<Customer>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var reader =  await new SqlCommand("SELECT * FROM Customer", connection).ExecuteReaderAsync();
                while (reader.Read())
                {
                    Customer customer = new Customer(
                        Guid.Parse(reader.GetString(0)), 
                        reader.GetString(1), 
                        reader.GetString(2)
                        );
                    customers.Add(customer);
                }
            }
            return customers;
        }
        public async Task<Customer> GetByIdAsync(Guid customerId)
        {
            Customer customer = null;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Customer WHERE СustomerId = @СustomerId", connection);
                command.Parameters.AddWithValue("@СustomerId", customerId);
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    customer = new Customer(
                        Guid.Parse(reader.GetString(0)), 
                        reader.GetString(1), 
                        reader.GetString(2)
                    );
                }
            }
            return customer;
        }
        
        
        
        public async Task<Customer> AddAsync(Customer customer)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("INSERT INTO Customer (CustomerId, Name, TaxNumber) VALUES (@СustomerId, @Name, @TaxNumber)", connection);
                command.Parameters.Add("@СustomerId", SqlDbType.UniqueIdentifier).Value = customer.CustomerId;
                command.Parameters.AddWithValue("@Name", customer.Name);
                command.Parameters.AddWithValue("@TaxNumber", customer.TaxNumber);
                await command.ExecuteNonQueryAsync();
            }
            return customer;
        }

        public async Task<Customer> RemoveAsync(Customer customer)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("DELETE FROM Customer WHERE СustomerId = @СustomerId", connection);
                command.Parameters.AddWithValue("@СustomerId", customer.CustomerId);
                await command.ExecuteNonQueryAsync();
            }
            return customer;
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            if(customer == null) throw new ArgumentNullException(nameof(customer), "Customer not be null");
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("UPDATE Customer SET Name = @Name, TaxNumber = @TaxNumber WHERE СustomerId = @СustomerId", connection);
                command.Parameters.AddWithValue("@СustomerId", customer.CustomerId);
                command.Parameters.AddWithValue("@Name", customer.Name);
                command.Parameters.AddWithValue("@TaxNumber", customer.TaxNumber);
                await command.ExecuteNonQueryAsync();
            }
            return customer;
        }
    }
}