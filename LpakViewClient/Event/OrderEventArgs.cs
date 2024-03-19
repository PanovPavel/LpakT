using System;
using LpakBL.Model;

namespace LpakViewClient.Event
{
    public class OrderEventArgs : EventArgs
    {
        public Order Order { get;  }
        public OrderEventArgs(Order order)
        {
            Order = order;
        }
    }
}