using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using LpakBL.Controller.Exception;
using LpakBL.Model;

namespace LpakBL.Controller
{
    /// <summary>
    /// Класс для работы со статусом заказов в базе данных
    /// </summary>
    public class StatusOrderController : IRepositoryAsync<StatusOrder>
    {
        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        private string ConnectionString { get;  }
        
        /// <summary>
        /// Конструктор класса StatusOrderController устанавливает строку подключения
        /// </summary>
        public StatusOrderController()
        {
            ConnectionString = ConnectionStringFactory.GetConnectionString();
        }

        /// <summary>
        /// Конструктор класса StatusOrderController устанавливает строку подключения
        /// </summary>
        /// <param name="connectionString">строка подключения к базе данных</param>
        public StatusOrderController(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Получает список всех статусов заказов в базе данных
        /// </summary>
        /// <returns>Все статусы в БД</returns>
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
        
        
        /// <summary>
        /// ПОлучить статус по id статуса
        /// </summary>
        /// <param name="id">id статуса</param>
        /// <returns>статус заказа</returns>
        /// <exception cref="NotFoundByIdException">Указанный id не найден в БД</exception>
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
        /// <summary>
        /// Добавить новый статус заказа в базу данных
        /// </summary>
        /// <param name="statusOrder">Добавляймый статус в базу данных</param>
        /// <returns></returns>
        /// <exception cref="UniquenessStatusException">Нарушение уникальных ключей при добавлении в базу данных</exception>
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
        /// <summary>
        /// Измениеть статус заказа
        /// </summary>
        /// <param name="statusOrder">Новый статус заказа</param>
        /// <returns></returns>
        /// <exception cref="UniquenessStatusException">Нарушение уникальных ключений</exception>
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
        /// <summary>
        /// Удалить статус из базы данных по id статуса
        /// </summary>
        /// <param name="id">id статуса заказа</param>
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