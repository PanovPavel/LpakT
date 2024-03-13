using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using LpakBL.Controller.Exception;
using LpakBL.Model;

namespace LpakBL.Controller
{
    public class OrderController : IRepositoryAsync<Order>
    {
        public string ConnectionString { get; }
        
        public OrderController()
        {
            ConnectionString = ConnectionStringFactory.GetConnectionString();
        }
        
        public OrderController(string connectionString)
        {
            ConnectionString = connectionString;
        }


        public async Task<List<Order>> GetListAsync()
        {
            throw new NotImplementedException();
        }
        //TODO: Удалить из ORDER любые упоминания о Customer
        public async Task<Order> GetAsync(Guid id)
        {
            Order order = null;            
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM Orders WHERE OrderId = @Id", sqlConnection);
                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    StatusOrder statusOrder = await new StatusOrderController().GetAsync(reader.GetGuid(1));
                    string description = reader.IsDBNull(reader.GetOrdinal("DescriptionWork"))?"":reader.GetString(reader.GetOrdinal("DescriptionWork"));                  
                      order = new Order(
                          reader.GetGuid(0),
                          statusOrder,
                          reader.GetDateTime(reader.GetOrdinal("DataTime")),
                          reader.GetString(reader.GetOrdinal("NameWork")),
                          description
                          );
                }
                return order ?? throw new NotFoundByIdException("Order with gived ID not found");
            }
        }

        public async Task<Order> AddAsync(Order order)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand command 
                    = new SqlCommand("INSERT INTO Orders (OrderId, StatusId, CustomerId, DataTime, NameWork, DescriptionWork)" +
                                     " VALUES (@Id, @StatusId, @CustomerId, @DateTimeCreatedOrder, @NameOfWork, @DescriptionOfWork)", sqlConnection);
                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = order.Id;
                command.Parameters.Add("@StatusId", SqlDbType.UniqueIdentifier).Value = order.Status.Id;
                command.Parameters.Add("@CustomerId", SqlDbType.UniqueIdentifier).Value = order.Customer.CustomerId;
                command.Parameters.Add("@DateTimeCreatedOrder", SqlDbType.DateTime).Value = order.DateTimeCreatedOrder;
                command.Parameters.Add("@NameOfWork", SqlDbType.VarChar).Value = order.NameOfWork;
                command.Parameters.Add("@DescriptionOfWork", SqlDbType.VarChar).Value = order.DescriptionOfWork;
                await command.ExecuteNonQueryAsync();
            }
            return order;
        }

        public Task<Order> UpdateAsync(Order item)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM Orders WHERE Id = @Id", connection);
                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}