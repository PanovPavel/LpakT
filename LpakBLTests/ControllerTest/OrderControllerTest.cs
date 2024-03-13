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
        [InlineData("0343b067-dfe5-44dd-8102-a4e6849edbfc")]
        public async Task Get_OrderController_Test(string id)
        {
            Order order = await new OrderController().GetAsync(Guid.Parse(id));
            _output.WriteLine(order.ToString());
        }

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