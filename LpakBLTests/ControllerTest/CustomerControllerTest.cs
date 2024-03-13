using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LpakBL.Controller;
using LpakBL.Controller.Exception;
using LpakBL.Model;
using Xunit;
using Xunit.Abstractions;

namespace LpakBLTests.ControllerTest
{
    public class CustomerControllerTest
    {
        private readonly ITestOutputHelper _output;
        public CustomerControllerTest(ITestOutputHelper output)
        {
            this._output = output;
        }
        
        [Fact]
        public async Task Get_CustomerController_Test()
        {
            Customer customer = await new CustomerController().GetAsync(Guid.Parse("431893a4-bda1-42f6-aacb-1aee3110a2e3"));
            _output.WriteLine(customer.ToString());
            foreach (var order in customer.Orders)
            {
                _output.WriteLine(order.ToString());
            }
        }
        
        [Fact]
        public async Task GetList_CustomerController_Test()
        {
            List<Customer> customers = await new CustomerController().GetListAsync();
            foreach (var customer in customers)
            {
                _output.WriteLine(customer.ToString());
            }
        }

        [Fact]
        public async Task Add_CustomerController_Test()
        {
            Customer customer = new Customer("UniquNameTest", "249149216530", "This is a comment", new FieldOfBusiness("T231e2st"));
            await new CustomerController().AddAsync(customer);
            await new CustomerController().RemoveAsync(customer.CustomerId);
        }

        [Fact]
        public async Task Update_CustomerController_Test()
        {   
            Customer customer = new Customer("ASDsadwqd", "711322472809", "This is a comment", new FieldOfBusiness("T231e2st"));
            var customerController = new CustomerController();
            await customerController.AddAsync(customer);
            var getCustomer = await customerController.GetAsync(customer.CustomerId);
            getCustomer.Name = "Новое имя";
            getCustomer.TaxNumber = "883526455810";
            getCustomer.FieldOfBusiness = new FieldOfBusiness("Test");
            await customerController.UpdateAsync(getCustomer);
            await customerController.RemoveAsync(getCustomer.CustomerId);
        }
        
        [Fact]
        public async Task Remove_CustomerController_Test()
        {
            await Assert.ThrowsAsync<RelatedRecordsException>(async () =>
                await new CustomerController().RemoveAsync(Guid.Parse("932a6cfc-d3b6-46b3-8b3b-1d93a96db008"))
            );
        }
        
        
        
    }
}