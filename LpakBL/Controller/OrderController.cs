using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
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

        public Task<Order> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> AddAsync(Order item)
        {
            throw new NotImplementedException();
        }

        public Task<Order> UpdateAsync(Order item)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}