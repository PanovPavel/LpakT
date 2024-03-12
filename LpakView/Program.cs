using System;
using System.Threading.Tasks;
using LpakBL.Model;
using LpakBL.Model.TaxNumberValidator;

namespace LpakView
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine(new CompanyInnValidator("1491763780").Validate());
            Customer customer = new Customer(Guid.NewGuid(), "asd",  "123456789101");
        }
    }
}
