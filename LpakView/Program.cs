using System;
using System.Threading.Tasks;
using LpakBL;
using LpakBL.Controller;

namespace LpakView
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Customer customer = new Customer(Guid.NewGuid(), "", "123456");
        }
    }
}
