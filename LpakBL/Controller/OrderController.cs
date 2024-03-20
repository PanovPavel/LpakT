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
    /// Класс для работы с заказами в базе данных
    /// </summary>
    public class OrderController : IRepositoryAsync<Order>
    {
        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// Конструктор класса OrderController устанавливает строку подключения к БД
        /// </summary>
        public OrderController()
        {
            ConnectionString = ConnectionStringFactory.GetConnectionString();
        }
        
        /// <summary>
        /// Конструктор класса OrderController устанавливает строку подключения
        /// </summary>
        /// <param name="connectionString">строка подключения к БД</param>
        public OrderController(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Получение все заказы в базе данных
        /// </summary>
        /// <returns>List всех заказов в базе данных</returns>
        public async Task<List<Order>> GetListAsync()
        {
            List<Order> ordersList = new List<Order>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var reader = await new SqlCommand("SELECT * FROM Orders", connection).ExecuteReaderAsync();
                while (reader.Read())
                {
                    StatusOrder statusOrder = await new StatusOrderController().GetAsync(reader.GetGuid(1));
                    string description = reader.IsDBNull(reader.GetOrdinal("DescriptionWork"))?"":reader.GetString(reader.GetOrdinal("DescriptionWork"));                  
                    Order order = new Order(
                        reader.GetGuid(0),
                        statusOrder,
                        reader.GetGuid(reader.GetOrdinal("CustomerId")),
                        reader.GetDateTime(reader.GetOrdinal("DataTime")),
                        reader.GetString(reader.GetOrdinal("NameWork")),
                        description
                    );
                    ordersList.Add(order);
                }
            }
            return ordersList;
        }
        
        /// <summary>
        /// Получить заказ по id
        /// </summary>
        /// <param name="id">id получаймого заказа </param>
        /// <returns></returns>
        /// <exception cref="NotFoundByIdException">Указанный id не существует в БД</exception>
        public async Task<Order> GetAsync(Guid id)
        {
            Order order = null;            
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                await sqlConnection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Orders WHERE OrderId = @Id", sqlConnection);
                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var statusOrder = await new StatusOrderController().GetAsync(reader.GetGuid(1));
                    string description = reader.IsDBNull(reader.GetOrdinal("DescriptionWork"))?"":reader.GetString(reader.GetOrdinal("DescriptionWork"));                  
                      order = new Order(
                          reader.GetGuid(0),
                          statusOrder,
                          reader.GetGuid(reader.GetOrdinal("CustomerId")),
                          reader.GetDateTime(reader.GetOrdinal("DataTime")),
                          reader.GetString(reader.GetOrdinal("NameWork")),
                          description
                          );
                }
                return order ?? throw new NotFoundByIdException("Order with gived ID not found");
            }
        }
        
        /// <summary>
        /// Добавить новый заказ в базу данных
        /// </summary>
        /// <param name="order">Добавляймы заказ</param>
        /// <returns>Добавленный заказ</returns>
        public async Task<Order> AddAsync(Order order)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand command 
                    = new SqlCommand("INSERT INTO Orders (OrderId, StatusId, CustomerId, DataTime, NameWork, DescriptionWork)" +
                                     " VALUES (@Id, @StatusId, @CustomerId, @DateTimeCreatedOrder, @NameOfWork, @DescriptionOfWork)", sqlConnection);
                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = order.Id;
                command.Parameters.Add("@CustomerId", SqlDbType.UniqueIdentifier).Value = order.CustomerId;
                command.Parameters.Add("@DateTimeCreatedOrder", SqlDbType.DateTime).Value = order.DateTimeCreatedOrder;
                command.Parameters.Add("@NameOfWork", SqlDbType.VarChar).Value = order.NameOfWork;
                command.Parameters.Add("@DescriptionOfWork", SqlDbType.VarChar).Value = order.DescriptionOfWork;

                StatusOrder statusOrder = CheckStatusOrderExistsInDbAsync(order.Status).Result;
                if (statusOrder == null)
                {
                    
                    command.Parameters.Add("@StatusId", SqlDbType.UniqueIdentifier).Value = order.Status.Id;
                    await new StatusOrderController().AddAsync(order.Status);
                }
                else
                {
                    command.Parameters.Add("@StatusId", SqlDbType.UniqueIdentifier).Value = statusOrder.Id;
                }
                await command.ExecuteNonQueryAsync();
            }
            return order;
        }
        
        /// <summary>
        /// Проверяет есть ли ИМЯ StatusOrder в базе данных
        /// </summary>
        /// <param name="statusOrder">проверяймый StatusOrder</param>
        /// <returns>возвращает StatusOrder если он содержится в БД, или null</returns>
        private async Task<StatusOrder> CheckStatusOrderExistsInDbAsync(StatusOrder statusOrder)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM StatusOrder" +
                                                    " WHERE NameStatus = @NameStatus", connection);
                command.Parameters.Add("@NameStatus", SqlDbType.VarChar).Value = statusOrder.Name;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return new StatusOrder(reader.GetGuid(0), reader.GetString(1));
                }
                return null;
            }
        }  
        
        /// <summary>
        /// Изменить заказ в базе данных
        /// </summary>
        /// <param name="order">Изменяймый order</param>
        /// <returns>изменяймы order</returns>
        public async Task<Order> UpdateAsync(Order order)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("UPDATE Orders SET StatusId = @StatusId," +
                                                    " CustomerId = @CustomerId, DataTime = @DateTimeCreatedOrder, NameWork = @NameOfWork," +
                                                    " DescriptionWork = @DescriptionOfWork WHERE OrderId = @Id", connection);
                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = order.Id;
                command.Parameters.Add("@CustomerId", SqlDbType.UniqueIdentifier).Value = order.CustomerId;
                command.Parameters.Add("@DateTimeCreatedOrder", SqlDbType.DateTime).Value = order.DateTimeCreatedOrder;
                command.Parameters.Add("@NameOfWork", SqlDbType.VarChar).Value = order.NameOfWork;
                command.Parameters.Add("@DescriptionOfWork", SqlDbType.VarChar).Value = order.DescriptionOfWork;
                StatusOrder statusOrder = CheckStatusOrderExistsInDbAsync(order.Status).Result;
                if (statusOrder == null)
                {
                    
                    command.Parameters.Add("@StatusId", SqlDbType.UniqueIdentifier).Value = order.Status.Id;
                    await new StatusOrderController().AddAsync(order.Status);
                }
                else
                {
                    command.Parameters.Add("@StatusId", SqlDbType.UniqueIdentifier).Value = statusOrder.Id;
                }

                await command.ExecuteNonQueryAsync();
            }
            return order;
        }

        
        /// <summary>
        /// Удалить заказ из базы данных по id
        /// </summary>
        /// <param name="id">id удаляймого заказа</param>
        public async Task RemoveAsync(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("DELETE FROM Orders WHERE OrderId = @Id", connection);
                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}