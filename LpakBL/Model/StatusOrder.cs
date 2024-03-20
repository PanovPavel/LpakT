using System;
using LpakBL.Model.Exception;

namespace LpakBL.Model
{
    /// <summary>
    /// Статус заказа.
    /// </summary>
    public class StatusOrder
    {
        
        private string _name;
        /// <summary>
        /// Наименование статуса заказа.
        /// </summary>
        /// <exception cref="IncorrectLongOrNullException">Неверная длинна строки имени заказа NullOrWhiteSpace</exception>
        public string Name
        {
            get=>_name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new IncorrectLongOrNullException(nameof(Name), $"Invalid long or value null value={value}");
                _name = value;
            }
        }
        /// <summary>
        /// Уникальный номер статуса заказа.
        /// </summary>
        public Guid Id { get; }
        /// <summary>
        /// Создание экземпляра класса <see cref="StatusOrder"/>.
        /// </summary>
        /// <param name="id">Guid статуса заказа</param>
        /// <param name="name">Имя статуса заказа</param>
        public StatusOrder(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
        /// <summary>
        /// Создание экземпляра класса <see cref="StatusOrder"/> с генерацией Guid по умолчанию.
        /// </summary>
        /// <param name="name">Имя статуса заказа</param>
        public StatusOrder(string name):this(Guid.NewGuid(), name)  
        {
            
        }
        public override string ToString()
        {
            return $"{Id} {Name}";
        }
    }
}