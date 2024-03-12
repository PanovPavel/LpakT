using System;
using System.Threading.Tasks;
using LpakBL.Controller;
using LpakBL.Model;
using LpakBL.Model.NumberCompanyValidator;

namespace LpakView
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var customer = new Customer("Name", "294868989407", "comment");
           await new CustomerController().AddAsync(customer);
        }
    }
}
