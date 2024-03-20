using System;
using LpakBL.Model.Exception;

namespace LpakBL.Model
{
    /// <summary>
    /// Класс области деятельности заказчика.
    /// </summary>
    public class FieldOfBusiness
    {
        /// <summary>
        /// Наименование области деятельности заказчика.
        /// </summary>
        private string _name;
        /// <summary>
        /// Уникальный идентификатор области деятельности заказ
        /// </summary>
        public Guid Id { get;}
        /// <summary>
        /// Наименование области деятельности заказчика
        /// </summary>
        /// <exception cref="IncorrectLongOrNullException">Имя не модет быть null или строкой из пробелов</exception>
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new IncorrectLongOrNullException(nameof(Name),$"Field of business name cannot be null or empty value={value}");
                _name = value;
            }
        }
        
        /// <summary>
        /// Создание экземпляра класса <see cref="FieldOfBusiness"/> с автоматически созданным идентификатором.
        /// </summary>
        /// <param name="name">имя области деятельности</param>
        public FieldOfBusiness(string name):this(Guid.NewGuid(),name)
        {
                
        }
        /// <summary>
        /// Создание экземпляра класса <see cref="FieldOfBusiness"/>
        /// </summary>
        /// <param name="id">Guid номер области деятельности</param>
        /// <param name="name">Наименование области деятельности </param>
        /// <exception cref="AggregateException">Неверное значение Guid</exception>
        public FieldOfBusiness(Guid id, string name)
        {

            if(id == Guid.Empty) throw new AggregateException($"Field of business id cannot be null or empty value={id}");
            Name = name;
            Id = id;
        }

        public override string ToString()
        {
            return $"{Id}, {Name}";
        }
    }
}