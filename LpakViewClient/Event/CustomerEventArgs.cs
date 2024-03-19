using System;
using LpakBL.Model;

namespace LpakViewClient.Event
{
    public class CustomerEventArgs : EventArgs
    {
        public Customer Customer { get; }

        public CustomerEventArgs(Customer customer)
        {
            Customer = customer;
        }
    }
}