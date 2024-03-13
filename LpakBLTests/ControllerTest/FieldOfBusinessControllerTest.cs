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
    public class FieldOfBusinessControllerTest
    {
        private readonly ITestOutputHelper _output;

        public FieldOfBusinessControllerTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        //GetList
        [Fact]
        public async Task GetFieldOfBusinessController_Test()
        {
            List<FieldOfBusiness> listFieldOfBusiness = await new FieldOfBusinessController().GetListAsync();
            foreach (var fieldOfBusiness in listFieldOfBusiness)
            {
                _output.WriteLine(fieldOfBusiness.ToString());
            }
        }

        //AddRemove
        [Theory]
        [InlineData("Тест")]
        public async Task Add_Remove_FieldOfBusinessController_Test(string nameFieldOfBusiness)
        {
            FieldOfBusiness fieldOfBusiness = new FieldOfBusiness(Guid.NewGuid(), nameFieldOfBusiness);
            FieldOfBusinessController fieldOfBusinessController = new FieldOfBusinessController();
            await fieldOfBusinessController.AddAsync(fieldOfBusiness);
            List<FieldOfBusiness> listFieldOfBusiness = await new FieldOfBusinessController().GetListAsync();
            await GetFieldOfBusinessController_Test();
            await fieldOfBusinessController.RemoveAsync(fieldOfBusiness.Id);
        }

        //Update
        [Theory]
        [InlineData("Тест")]
        public async Task Update_FieldOfBusinessController_Test(string nameFieldOfBusiness)
        {
            FieldOfBusinessController fieldOfBusinessController = new FieldOfBusinessController();
            await fieldOfBusinessController.AddAsync(new FieldOfBusiness(nameFieldOfBusiness));
            List<FieldOfBusiness> fieldOfBusiness = await fieldOfBusinessController.GetListAsync();
            foreach (var ofBusiness in fieldOfBusiness)
            {
                _output.WriteLine(ofBusiness.ToString());
            }
            var testStatus = fieldOfBusiness.Find(x => x.Name == nameFieldOfBusiness);
            testStatus.Name = "ChangedName";
            await fieldOfBusinessController.UpdateAsync(testStatus);
            await GetFieldOfBusinessController_Test();
            await fieldOfBusinessController.RemoveAsync(testStatus.Id);
        }

        [Theory]
        [InlineData("ТестUnique")]
        public async Task ExceptionAddFieldOfBusiness(string nameFieldOfBusiness)
        {
            FieldOfBusinessController fieldOfBusinessController = new FieldOfBusinessController();
            FieldOfBusiness fieldOfBusiness = new FieldOfBusiness(nameFieldOfBusiness);
            await fieldOfBusinessController.AddAsync(fieldOfBusiness);
            await Assert.ThrowsAsync<UniquenessStatusException>(async ()
                => await fieldOfBusinessController.AddAsync(fieldOfBusiness));
            await fieldOfBusinessController.RemoveAsync(fieldOfBusiness.Id);
        }

        [Fact]
        public async Task GetById()
        {
            List<FieldOfBusiness> fieldOfBusinessesList = await new FieldOfBusinessController().GetListAsync();
            FieldOfBusiness fieldOfBusinessTest = fieldOfBusinessesList.First();
            await new FieldOfBusinessController().GetAsync(fieldOfBusinessTest.Id);
        }
        [Fact]
        public async Task ExceptionGetIdFieldOfBusiness()
        {
            await Assert.ThrowsAsync<NotFoundByIdException>(async () =>
                await new FieldOfBusinessController().GetAsync(Guid.NewGuid()));
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