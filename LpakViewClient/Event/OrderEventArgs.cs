using System;
using LpakBL.Model;

namespace LpakViewClient.Event
{
    /// <summary>
    /// EventArgs для события order event 
    /// </summary>
    public class OrderEventArgs : EventArgs
    {
        /// <summary>
        /// Объект Order вызывавшего событие 
        /// </summary>
        public Order Order { get;  }
        /// <summary>
        /// Конструктор  класса OrderEventArgs 
        /// </summary>
        /// <param name="order">Объект вывавший событие</param>
        public OrderEventArgs(Order order)
        {
            Order = order;
        }
    }
}