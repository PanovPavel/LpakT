using System;
using LpakBL.Controller.Exception;
using LpakBL.Model.Exception;

namespace LpakBL.Model
{
    /// <summary>
    /// Класса описывющий модель заказа
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Создание экземпляра класса <see cref="Order"/> 
        /// </summary>
        /// <param name="id">Guid заказа</param>
        /// <param name="status">Статус заказа</param>
        /// <param name="customerId">Guid клиента которому принадлежит заказ</param>
        /// <param name="dateTime">Дата создания заказа</param>
        /// <param name="nameOfWork">Наименование работот</param>
        /// <param name="descriptionOfWork">Описание работ. Значение пол умолчанию, пустая строка</param>
        /// <exception cref="ArgumentException">Неверное значение Guid</exception>
        /// <exception cref="InvalidTaxNumber">Невалидный налоговый номер</exception>
        /// <exception cref="IncorrectLongOrNullException">Неверная длинна одного из свойств</exception>
        public Order(Guid id, StatusOrder status, Guid customerId, DateTime dateTime, string nameOfWork,
            string descriptionOfWork = "")
        {
            if (id == Guid.Empty) throw new ArgumentException("Order id cannot be empty", nameof(id));
            Id = id;
            Status = status;
            CustomerId = customerId;
            DateTimeCreatedOrder = dateTime;
            NameOfWork = nameOfWork;
            DescriptionOfWork = descriptionOfWork;
        }
        /// <summary>
        /// Создание экземпляра класса <see cref="Order"/> с автоматическим генерацией Guid
        /// </summary>
        /// <param name="status">Статус заказа</param>
        /// <param name="customerId">Guid клиента которому принадлежит заказ</param>
        /// <param name="dateTime">Дата создания заказа</param>
        /// <param name="nameOfWork">Наименование работот</param>
        /// <param name="descriptionOfWork">Описание работ. Значение пол умолчанию, пустая строка</param>
        /// <exception cref="ArgumentException">Неверное значение Guid</exception>
        /// <exception cref="InvalidTaxNumber">Невалидный налоговый номер</exception>
        /// <exception cref="IncorrectLongOrNullException">Неверная длинна одного из свойств</exception>
        public Order(StatusOrder status, Guid customerId, DateTime dateTime, string nameOfWork,
            string descriptionOfWork = "")
            : this(Guid.NewGuid(), status, customerId, dateTime, nameOfWork, descriptionOfWork)
        {
        }
        
        /// <summary>
        /// Создание экземпляра класса <see cref="Order"/> с автоматическим генерацией Guid, и
        /// датой установки <see cref="DateTime.Now"/>
        /// </summary>
        /// <param name="status">Статус заказа</param>
        /// <param name="customerId">Guid клиента которому принадлежит заказ</param>
        /// <param name="nameOfWork">Наименование работот</param>
        /// <param name="descriptionOfWork">Описание работ. Значение пол умолчанию, пустая строка</param>
        /// <exception cref="ArgumentException">Неверное значение Guid</exception>
        /// <exception cref="InvalidTaxNumber">Невалидный налоговый номер</exception>
        /// <exception cref="IncorrectLongOrNullException">Неверная длинна одного из свойств</exception>
        public Order(StatusOrder status, Guid customerId, string nameOfWork, string descriptionOfWork = "")
            : this(Guid.NewGuid(), status, customerId, DateTime.Now, nameOfWork, descriptionOfWork)
        {
        }
        private string _nameOfWork, _descriptionOfWork;
        private DateTime _dateTimeCreatedOrder;
        private Guid _customerId;
        private StatusOrder _status;
        /// <summary>
        /// Уникальный номер заказа
        /// </summary>
        public Guid Id { get; }
        
        /// <summary>
        /// Дата заказа
        /// </summary>
        /// <exception cref="InvalidDateException">Дата больше текущей или слишком старая</exception>
        public DateTime DateTimeCreatedOrder
        {
            get => _dateTimeCreatedOrder;
            set
            {
                if (value == DateTime.MinValue)
                {
                    throw new InvalidDateException("Date time created order can not be null");
                }

                if (value > DateTime.Today.AddHours(11).AddMinutes(59) || value < DateTime.Parse("01.01.2000"))
                {
                    throw new InvalidDateException($"Date time cannot more than today or less than 01.01.2000 value={value}");
                }

                _dateTimeCreatedOrder = value;
            }
        }
        /// <summary>
        /// Наименование работот
        /// </summary>
        /// <exception cref="IncorrectLongOrNullException">Невреная длинная наименование работ. null or WhiteSpace</exception>
        public string NameOfWork
        {
            get => _nameOfWork;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new IncorrectLongOrNullException(nameof(NameOfWork),
                        $"Invalid long or value null value={value}");
                _nameOfWork = value;
            }
        }

        /// <summary>
        /// Описание работ. Значение пол умолчанию, пустая строк
        /// </summary>
        /// <exception cref="ArgumentNullException">попытка присвоить null описанию работ</exception>
        public string DescriptionOfWork
        {
            get => _descriptionOfWork;
            set => _descriptionOfWork =
                value ?? throw new ArgumentNullException( nameof(DescriptionOfWork), "Argument cannot be null");
        }
        
        /// <summary>
        /// Уникальный номер клиента, которому принадлежит заказ
        /// </summary>
        /// <exception cref="ArgumentException">Guid не может быть простым</exception>
        public Guid CustomerId
        {
            get => _customerId;
            set
            {
                if (value == Guid.Empty)
                {
                    throw new ArgumentException("Customer id can not be empty", nameof(CustomerId));
                }

                _customerId = value;
            }
        }

        /// <summary>
        /// Статус заказа
        /// </summary>
        /// <exception cref="ArgumentNullException">Статус заказа не может быть null</exception>
        public StatusOrder Status
        {
            get => _status;
            set => _status = value ?? throw new ArgumentNullException(nameof(Status), "Status can not be null");
        }
        
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(DateTimeCreatedOrder)}: {DateTimeCreatedOrder}, " +
                   $"{nameof(NameOfWork)}: {NameOfWork}, {nameof(DescriptionOfWork)}: {DescriptionOfWork}";
        }
    }
}