using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LpakBL.Controller;
using LpakBL.Controller.Exception;
using LpakBL.Model;
using Xunit;
using Xunit.Abstractions;

namespace LpakBLTests.ControllerTest
{
    
    public class StatusOrderControllerTest
    {
        private readonly ITestOutputHelper _output;
        public StatusOrderControllerTest(ITestOutputHelper output)
        {
            this._output = output;
        }
        //GetList
        [Fact]
        public async Task GetList_StatusOrderController_Test()
        {
            List<StatusOrder> listStatusOrders = await new StatusOrderController().GetListAsync();
            foreach (var statusOrder in listStatusOrders)
            {
                _output.WriteLine($"StatusOrder: {statusOrder}");
            }            
        }
        //AddRemove
        [Theory]
        [InlineData("Тест")]
        public async Task Add_Remove_StatusOrderController_Test(string nameStatusOrder)
        {
            StatusOrder statusOrder = new StatusOrder(Guid.NewGuid(), nameStatusOrder);
            StatusOrderController statusOrderController = new StatusOrderController();
            await statusOrderController.AddAsync(statusOrder);
            List<StatusOrder> listStatusOrders = await new StatusOrderController().GetListAsync();
            await GetList_StatusOrderController_Test();
            await statusOrderController.RemoveAsync(statusOrder.Id);
        }
        //Update
        [Theory]
        [InlineData("Тест")]
        public async Task Update_StatusOrderController_Test(string nameStatusOrder) 
        {
            StatusOrderController statusOrderController = new StatusOrderController();
            await statusOrderController.AddAsync(new StatusOrder(nameStatusOrder));
            List<StatusOrder> listStatusOrders = await statusOrderController.GetListAsync();
            var testStatus =  listStatusOrders.Find(x=>x.Name == nameStatusOrder);
            testStatus.Name = "ChangedName";
            await statusOrderController.UpdateAsync(testStatus);
            await GetList_StatusOrderController_Test();
            await statusOrderController.RemoveAsync(testStatus.Id);
        }

        [Theory]
        [InlineData("ТестUnique")]
        public async Task ExceptionAdd(string nameStatusOrder)
        {
            StatusOrderController statusOrderController = new StatusOrderController();
            StatusOrder statusOrder = new StatusOrder(nameStatusOrder);
            await statusOrderController.AddAsync(statusOrder);
            await Assert.ThrowsAsync<UniquenessStatusException>(async ()=>
                await statusOrderController.AddAsync(statusOrder));
            await statusOrderController.RemoveAsync(statusOrder.Id);
        }
        [Fact]
        public async Task GetById()
        {
            List<StatusOrder> statusOrdersList = await new StatusOrderController().GetListAsync();
            StatusOrder statusOrderTest = statusOrdersList.First();
            await new StatusOrderController().GetAsync(statusOrderTest.Id);
        }
        [Fact]
        public async Task ExceptionGetId()
        {
            await Assert.ThrowsAsync<NotFoundByIdException>(async ()=>
                await new StatusOrderController().GetAsync(Guid.NewGuid()));
        }
        /*
        [Fact]
        public async Task ExceptionRemove()
        {
            await new StatusOrderController().RemoveAsync(Guid.NewGuid());
        }
        */
    }
    
}