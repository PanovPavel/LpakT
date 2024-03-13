using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using LpakBL.Controller.Exception;
using LpakBL.Model;

namespace LpakBL.Controller
{
    public class StatusOrderController : IRepositoryAsync<StatusOrder>
    {
        private string ConnectionString { get; set; }
        public StatusOrderController()
        {
            ConnectionString = ConnectionStringFactory.GetConnectionString();
        }

        public StatusOrderController(string connectionString)
        {
            ConnectionString = connectionString;
        }


        public async Task<List<StatusOrder>> GetListAsync()
        {
            List<StatusOrder> statusOrders = new List<StatusOrder>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var reader =  await new SqlCommand("SELECT * FROM StatusOrder", connection).ExecuteReaderAsync();
                while (reader.Read())
                {
                    StatusOrder statusOrder = new StatusOrder(
                        reader.GetGuid(0),
                        reader.GetString(1)
                        );
                    statusOrders.Add(statusOrder);
                }
            }
            return statusOrders;
        }

        public async Task<StatusOrder> GetAsync(Guid id)
        {
            StatusOrder statusOrder = null;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var command =  new SqlCommand("SELECT * FROM StatusOrder WHERE StatusId = @Id", connection);
                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    statusOrder = new StatusOrder(
                        reader.GetGuid(0),
                        reader.GetString(1)
                    );
                }
            }
            return statusOrder?? throw new NotFoundByIdException("StatusOrder with gived ID not found");
        }

        public async Task<StatusOrder> AddAsync(StatusOrder statusOrder)
        {
            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    await sqlConnection.OpenAsync();
                    SqlCommand command =
                        new SqlCommand("INSERT INTO StatusOrder (StatusId, NameStatus) VALUES (@Id, @NameStatus)",
                            sqlConnection);
                    command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = statusOrder.Id;
                    command.Parameters.Add("@NameStatus", SqlDbType.VarChar).Value = statusOrder.Name;
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601) throw new UniquenessStatusException("Current status already exists");
                throw;
            }
            return statusOrder;
        }

        public async Task<StatusOrder> UpdateAsync(StatusOrder statusOrder)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    await sqlConnection.OpenAsync();
                    SqlCommand command =
                        new SqlCommand("UPDATE StatusOrder SET NameStatus = @NameStatus WHERE StatusId = @Id",
                            sqlConnection);
                    command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = statusOrder.Id;
                    command.Parameters.Add("@NameStatus", SqlDbType.VarChar).Value = statusOrder.Name;
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601) throw new UniquenessStatusException("Current status already exists");
                throw;
            }
            return statusOrder;
        }

        public async Task RemoveAsync(Guid id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand command = new SqlCommand("DELETE FROM StatusOrder WHERE StatusId = @Id", sqlConnection);
                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}