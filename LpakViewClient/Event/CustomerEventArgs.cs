using System;
using LpakBL.Model;

namespace LpakViewClient.Event
{
    /// <summary>
    /// EventArgs для события customer event 
    /// </summary>
    public class CustomerEventArgs : EventArgs
    {
        /// <summary>
        /// Объект заказчика вызвавшего событие
        /// </summary>
        public Customer Customer { get; }
        /// <summary>
        ///  Конструктор класса CustomerEventArgs
        /// </summary>
        /// <param name="customer">Объект вызвавший сызвавший событие</param>
        public CustomerEventArgs(Customer customer)
        {
            Customer = customer;
        }
    }
}