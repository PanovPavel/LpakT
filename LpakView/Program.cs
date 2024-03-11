using System;
using System.Threading.Tasks;
using LpakBL;
using LpakBL.Controller;
using LpakBL.Model;

namespace LpakView
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Customer customer = new Customer(Guid.NewGuid(), "asd",  "123456789101");
        }
    }
}
