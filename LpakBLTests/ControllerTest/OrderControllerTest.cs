using System;
using System.Threading.Tasks;
using LpakBL.Controller;
using LpakBL.Model;
using Xunit;
using Xunit.Abstractions;

namespace LpakBLTests.ControllerTest
{
    public class OrderControllerTest
    {
        private readonly ITestOutputHelper output;
        public OrderControllerTest(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Theory]
        [InlineData("0343b067-dfe5-44dd-8102-a4e6849edbfc")]
        public async Task GetList_OrderController_Test(string id)
        {
            Order order = await new OrderController().GetAsync(Guid.Parse(id));
            output.WriteLine(order.ToString());
        }

        public async Task Add_OrderController_Test()
        {
            
        }
    }
}