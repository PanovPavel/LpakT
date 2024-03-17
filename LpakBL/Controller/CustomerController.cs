using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using LpakBL.Controller.Exception;
using LpakBL.Model;
namespace LpakBL.Controller
{
    //TODO: Добавить в Reader проверки на Null.
    //Todo: Переписать все Readers и использовать GetOrdinal
    //TODO: Сделать Reader асинхронным
    //Todo: Посмотреть как можно обработать исключения уникальности Statud FieldOFBusines
    //Todo: Подумать над абстрактым классом для connectionString
    //Todo: Для Reader передавать в конструктор объека параметры не напрямую а из полей
    //Todo: Добавить транзакции в запросы
    //TOdo: В update добавить проверку на существование объекта в бд
    public class CustomerController : IRepositoryAsync<Customer>
    {
        private string ConnectionString { get; }

        public CustomerController(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public CustomerController()
        {
            ConnectionString = ConnectionStringFactory.GetConnectionString();
        }


        public async Task<List<Customer>> GetListAsync()
        {
            List<Customer> customers = new List<Customer>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var readerCustomer = await new SqlCommand("SELECT * FROM Customer", connection).ExecuteReaderAsync();
                while (readerCustomer.Read())
                {
                    FieldOfBusiness fieldOfBusiness = await new FieldOfBusinessController().GetAsync(readerCustomer.GetGuid(4));
                    List<Order> orders = await GetOrdersForCustomerAsync(readerCustomer.GetGuid(0));
                    
                    string comment = readerCustomer.IsDBNull(3)?"":readerCustomer.GetString(3);
                    Customer customer = new Customer(
                        readerCustomer.GetGuid(0),
                        readerCustomer.GetString(1),
                        readerCustomer.GetString(2).Trim(),
                        comment,
                        fieldOfBusiness,
                        orders
                    );
                    customers.Add(customer);
                }
            }
            return customers;
        }
        
        
        
        private async Task<List<Order>> GetOrdersForCustomerAsync(Guid customerId)
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT * FROM Orders WHERE CustomerId = @CustomerId", connection);
                command.Parameters.Add("@CustomerId", SqlDbType.UniqueIdentifier).Value = customerId;
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var orderId = reader.GetGuid(reader.GetOrdinal("OrderId"));
                    var order = await new OrderController().GetAsync(orderId);
                    orders.Add(order);
                }
            }
            return orders;
        }
        
        public async Task<Customer> GetAsync(Guid id)
        {
            Customer customer = null;
            var orders = await GetOrdersForCustomerAsync(id);
            
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command =
                    new SqlCommand("SELECT * FROM Customer WHERE CustomerId = @CustomerId", connection);
                
                command.Parameters.Add("@CustomerId", SqlDbType.UniqueIdentifier).Value = id;
                var readerCustomer = await command.ExecuteReaderAsync();
                while (readerCustomer.Read())
                {
                    FieldOfBusiness fieldOfBusiness = await new FieldOfBusinessController().GetAsync(readerCustomer.GetGuid(4));
                    string comment = readerCustomer.IsDBNull(3)?"":readerCustomer.GetString(3);
                    customer = new Customer(
                        readerCustomer.GetGuid(0),
                        readerCustomer.GetString(1),
                        readerCustomer.GetString(2).Trim(),
                        comment,
                        fieldOfBusiness,
                        orders
                    );
                }
                return customer?? throw new NotFoundByIdException("Customer with gived ID not found");
            }
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command =
                        new SqlCommand(
                            "INSERT INTO Customer (CustomerId, Name, TaxNumber, Comment, FieldOfBusinessId)" +
                            " VALUES (@CustomerId, @Name, @TaxNumber, @Comment, @FieldOfBusinessId)", connection);
                    command.Parameters.Add("@CustomerId", SqlDbType.UniqueIdentifier).Value = customer.CustomerId;
                    command.Parameters.Add("@Name", SqlDbType.VarChar).Value = customer.Name;
                    command.Parameters.Add("@TaxNumber", SqlDbType.VarChar).Value = customer.TaxNumber;
                    command.Parameters.Add("@Comment", SqlDbType.VarChar).Value = customer.Comment;

                    FieldOfBusiness fieldOfBusiness = CheckFieldOfBusinessExistsAsync(customer.FieldOfBusiness).Result;
                    if (fieldOfBusiness == null)
                    {
                        command.Parameters.Add("@FieldOfBusinessId", SqlDbType.UniqueIdentifier).Value =
                            customer.FieldOfBusiness.Id;
                        await new FieldOfBusinessController().AddAsync(customer.FieldOfBusiness);
                    }
                    else
                    {
                        command.Parameters.Add("@FieldOfBusinessId", SqlDbType.UniqueIdentifier).Value =
                            fieldOfBusiness.Id;
                    }

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601) throw new UniquenessStatusException("Current customer with this taxNumber or name already   exists");
                throw;
            }

            return customer;
        }
        private async Task<FieldOfBusiness> CheckFieldOfBusinessExistsAsync(FieldOfBusiness fieldOfBusiness)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM FieldOfBusiness" +
                                                    " WHERE Name = @FieldOfBusinessName", connection);
                command.Parameters.Add("@FieldOfBusinessName", SqlDbType.VarChar).Value = fieldOfBusiness.Name;
                var reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    return new FieldOfBusiness(reader.GetGuid(0), reader.GetString(1));
                }
                return null;
            }
        }        
        public async Task<Customer> UpdateAsync(Customer customer)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command =
                        new SqlCommand("UPDATE Customer SET Name = @Name, TaxNumber = @TaxNumber," +
                                       " Comment = @Comment, FieldOfBusinessId=@FieldOfBusinessId" +
                                       " WHERE CustomerId = @CustomerId", connection);
                    command.Parameters.Add("@Name", SqlDbType.VarChar).Value = customer.Name;
                    command.Parameters.Add("@TaxNumber", SqlDbType.VarChar).Value = customer.TaxNumber;
                    command.Parameters.Add("@Comment", SqlDbType.VarChar).Value = customer.Comment;
                    command.Parameters.Add("@CustomerId", SqlDbType.UniqueIdentifier).Value = customer.CustomerId;

                    //TODO: Вынести в отдельный метод
                    FieldOfBusiness fieldOfBusiness = CheckFieldOfBusinessExistsAsync(customer.FieldOfBusiness).Result;
                    if (fieldOfBusiness == null)
                    {
                        command.Parameters.Add("@FieldOfBusinessId", SqlDbType.UniqueIdentifier).Value =
                            customer.FieldOfBusiness.Id;
                        await new FieldOfBusinessController().AddAsync(customer.FieldOfBusiness);
                    }
                    else
                    {
                        command.Parameters.Add("@FieldOfBusinessId", SqlDbType.UniqueIdentifier).Value =
                            fieldOfBusiness.Id;
                    }

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                    throw new UniquenessStatusException("Current customer with this taxNumber or name already   exists.");
                throw;
            }

            return customer;
        }

        public async Task RemoveAsync(Guid id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command =
                        new SqlCommand("DELETE FROM Customer WHERE CustomerId = @CustomerId", connection);
                    command.Parameters.Add("@CustomerId", SqlDbType.UniqueIdentifier).Value = id;
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {
                if(ex.Number == 547) throw new RelatedRecordsException("Customer has related orders. Please delete them first.");
            }
        }
    }
}