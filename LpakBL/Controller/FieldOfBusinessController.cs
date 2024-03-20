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
    /// Класс для взаимодействия через БД с областью занятости клиентов 
    /// </summary>
    public class FieldOfBusinessController:IRepositoryAsync<FieldOfBusiness>
    {
        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        string ConnectionString { get; }
        /// <summary>
        /// Конструктор класса, устанавливает строку подключения к БД
        /// </summary>
        public FieldOfBusinessController()
        {
            ConnectionString = ConnectionStringFactory.GetConnectionString();
        } 
        /// <summary>
        /// Конструктор класса, устанавливает строку подключения к Б
        /// </summary>
        /// <param name="connectionString">Строка подключения к БД</param>
        public FieldOfBusinessController(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Возвращает список всех областей деятельности 
        /// </summary>
        /// <returns>List FieldOfBusiness</returns>
        public async Task<List<FieldOfBusiness>> GetListAsync()
        {
            List<FieldOfBusiness> fieldOfBusinessList = new List<FieldOfBusiness>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var reader = await new SqlCommand("SELECT * FROM FieldOfBusiness", connection).ExecuteReaderAsync();
                while (reader.Read())
                {
                    FieldOfBusiness fieldOfBusiness = new FieldOfBusiness(reader.GetGuid(0), reader.GetString(1));
                    fieldOfBusinessList.Add(fieldOfBusiness);
                }
            }
            return fieldOfBusinessList;
        }
        /// <summary>
        /// Получает область деятельности по id
        /// </summary>
        /// <param name="id">id области деятельности</param>
        /// <returns></returns>
        /// <exception cref="NotFoundByIdException">Область деятельности с указанным id не найден в бд</exception>
        public async Task<FieldOfBusiness> GetAsync(Guid id)
        {
            FieldOfBusiness fieldOfBusiness = null;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM FieldOfBusiness WHERE FieldOfBusinessId = @Id", connection);
                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    fieldOfBusiness = new FieldOfBusiness(reader.GetGuid(0), reader.GetString(1));
                }
            }
            return fieldOfBusiness?? throw new NotFoundByIdException("FieldOfBusiness with gived ID not found");
        }
        
        /// <summary>
        /// Добавление новой области деятельности в базу данных
        /// </summary>
        /// <param name="fieldOfBusiness">Область деятельности для добавления</param>
        /// <returns>Добавляймая область деятельности</returns>
        /// <exception cref="UniquenessStatusException">Значения области деятельности нарушают один из уникальных ключей</exception>
        public async Task<FieldOfBusiness> AddAsync(FieldOfBusiness fieldOfBusiness)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    var command =
                        new SqlCommand(
                            "INSERT INTO FieldOfBusiness (FieldOfBusinessId, Name) Values (@Id, @NameFieldOfBusiness)",
                            connection);
                    command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = fieldOfBusiness.Id;
                    command.Parameters.Add("@NameFieldOfBusiness", SqlDbType.VarChar).Value = fieldOfBusiness.Name;
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {
                if(ex.Number == 2627 || ex.Number == 2601) throw new UniquenessStatusException("FieldOfBusiness with gived ID already exists");
                throw;
            }
            return fieldOfBusiness;
        }

        /// <summary>
        /// Изменение значений области деятельности в базе данных
        /// </summary>
        /// <param name="fieldOfBusiness">Область деятельности которая будет изменина</param>
        /// <returns>Изменённая область деятельности</returns>
        public async Task<FieldOfBusiness> UpdateAsync(FieldOfBusiness fieldOfBusiness)
        {
            using ( SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("UPDATE FieldOfBusiness SET Name = @NameFieldOfBusiness WHERE FieldOfBusinessId = @Id", connection);
                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = fieldOfBusiness.Id;
                command.Parameters.Add("@NameFieldOfBusiness", SqlDbType.VarChar).Value = fieldOfBusiness.Name;
                await command.ExecuteNonQueryAsync();
            }
            return fieldOfBusiness;
        }

        /// <summary>
        /// Удаление области деятельности из базы данных по id
        /// </summary>
        /// <param name="id">id области деятельности</param>
        public async Task RemoveAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                await sqlConnection.OpenAsync();
                var command = new SqlCommand("DELETE FROM FieldOfBusiness WHERE FieldOfBusinessId = @Id", sqlConnection);
                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}