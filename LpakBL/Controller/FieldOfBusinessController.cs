using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using LpakBL.Controller.Exception;
using LpakBL.Model;

namespace LpakBL.Controller
{
    public class FieldOfBusinessController:IRepositoryAsync<FieldOfBusiness>
    {
        string ConnectionString { get; }
        public FieldOfBusinessController()
        {
            ConnectionString = ConnectionStringFactory.GetConnectionString();
        } 
        
        public FieldOfBusinessController(string connectionString)
        {
            ConnectionString = connectionString;
        }


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