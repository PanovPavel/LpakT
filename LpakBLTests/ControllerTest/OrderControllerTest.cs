using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LpakBL.Controller;
using LpakBL.Model;
using Xunit;
using Xunit.Abstractions;

namespace LpakBLTests.ControllerTest
{
    public class OrderControllerTest
    {
        private readonly ITestOutputHelper _output;
        public OrderControllerTest(ITestOutputHelper output)
        {
            this._output = output;
        }
        [Theory]
        [InlineData("6ef1ee4a-3600-4e28-b706-6cc8e8cc62ff")]
        public async Task Get_OrderController_Test(string id)
        {
            Order order = await new OrderController().GetAsync(Guid.Parse(id));
            _output.WriteLine(order.ToString());
        }
.
        [Fact]
        public async Task Add_Remove_OrderController_Test()
        {
            
            List<Customer> listCustomer = await new CustomerController().GetListAsync();
            Customer firstCustomer = listCustomer.First();
            Order order = new Order(new StatusOrder("TestStatusWork"), firstCustomer.CustomerId, DateTime.Now,
                "TestNameWork", "TestDescriptionOfWork");
            await new OrderController().AddAsync(order);
            await new OrderController().RemoveAsync(order.Id);
        }
        
        [Fact]
        public async Task GetList_OrderController_Test()
        {
            List<Order> listOrders = await new OrderController().GetListAsync();
            foreach (var order in listOrders)
            {
                _output.WriteLine(order.ToString());
            }
        }
        
    }
}