using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using LpakBL.Model.Exception;
using LpakBL.Model.NumberCompanyValidator;

namespace LpakBL.Model
{
    /// <summary>
    /// Класс описывабщий модель заказчика
    /// </summary>
    public class Customer : INotifyPropertyChanged
    {
        /// <summary>
        /// Создаёт новый объект заказчика, с пустым списком заказов, без примечания к заказчику. Значение Guid шенерируется при создании. 
        /// </summary>
        /// <param name="name">Имя заказчика</param>
        /// <param name="taxNumber">Налоговый номер</param>
        /// <param name="fieldOfBusiness">Область деятельности</param>
        /// <exception cref="ArgumentException">Неверное значение Guid</exception>
        /// <exception cref="InvalidTaxNumber">Невалидный налоговый номер</exception>
        /// <exception cref="IncorrectLongOrNullException">Неверная длинна одного из свойств</exception>
        public Customer(string name, string taxNumber, FieldOfBusiness fieldOfBusiness)
            : this(name, taxNumber, "", fieldOfBusiness)
        {
        }
        /// <summary>
        /// Создаёт новый объект заказчика, с пустым списком заказов. Значение Guid шенерируется при создании. 
        /// </summary>
        /// <param name="name">Имя заказчика</param>
        /// <param name="taxNumber">Налоговый номер</param>
        /// <param name="comment">Примечания к заказчику</param>
        /// <param name="fieldOfBusiness">Область деятельности</param>
        /// <exception cref="ArgumentException">Неверное значение Guid</exception>
        /// <exception cref="InvalidTaxNumber">Невалидный налоговый номер</exception>
        /// <exception cref="IncorrectLongOrNullException">Неверная длинна одного из свойств</exception>
        public Customer(string name, string taxNumber, string comment, FieldOfBusiness fieldOfBusiness)
            : this(name, taxNumber, comment, fieldOfBusiness, new List<Order>())
        {
        }
        
        /// <summary>
        /// Создаёт новый объект заказчика. Значение Guid шенерируется при создании.
        /// </summary>
        /// <param name="name">Имя заказчика</param>
        /// <param name="taxNumber">Налоговый номер</param>
        /// <param name="comment">Примечания к заказчику</param>
        /// <param name="fieldOfBusiness">Область деятельности</param>
        /// <param name="listOrders">Заказы который есть у Customer</param>
        /// <exception cref="ArgumentException">Неверное значение Guid</exception>
        /// <exception cref="InvalidTaxNumber">Невалидный налоговый номер</exception>
        /// <exception cref="IncorrectLongOrNullException">Неверная длинна одного из свойств</exception>
        public Customer(string name, string taxNumber, string comment, FieldOfBusiness fieldOfBusiness,
            List<Order> listOrders)
            : this(Guid.NewGuid(), name, taxNumber, comment, fieldOfBusiness, listOrders)
        {
        }
        
        /// <summary>
        /// Создаёт новый объект заказчика
        /// </summary>
        /// <param name="guid">Guid номер заказчика</param>
        /// <param name="name">Имя заказчика</param>
        /// <param name="taxNumber">Налоговый номер</param>
        /// <param name="comment">Примечания к заказчику</param>
        /// <param name="fieldOfBusiness">Область деятельности</param>
        /// <param name="listOrder">Заказы который есть у Customer</param>
        /// <exception cref="ArgumentException">Неверное значение Guid</exception>
        /// <exception cref="InvalidTaxNumber">Невалидный налоговый номер</exception>
        /// <exception cref="IncorrectLongOrNullException">Неверная длинна одного из свойств</exception>
        public Customer(Guid guid, string name, string taxNumber, string comment, FieldOfBusiness fieldOfBusiness,
            List<Order> listOrder)
        {
            if (guid == Guid.Empty) throw new ArgumentException(nameof(guid), $"Guid not be empty {guid.ToString()}");
            CustomerId = guid;
            Name = name;
            TaxNumber = taxNumber;
            Comment = comment;
            FieldOfBusiness = fieldOfBusiness;
            Orders = listOrder;
        }

        private string _name, _comment, _taxNumber;
        private List<Order> _orders = new List<Order>();
        private FieldOfBusiness _fieldOfBusiness;

        /// <summary>
        /// Получить уникальный номер заказчика
        /// </summary>
        public Guid CustomerId { get; }
        
        /// <summary>
        /// Область деятельности заказчика
        /// </summary>
        public FieldOfBusiness FieldOfBusiness
        {
            get => _fieldOfBusiness;
            set
            {
                _fieldOfBusiness = value;
                OnPropertyChanged("FieldOfBusiness");
            }
        }
        /// <summary>
        /// Список заказов
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public List<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value ?? throw new ArgumentNullException(nameof(Orders), "Orders can't be null"); 
                OnPropertyChanged("Orders");
            }
        }
        /// <summary>
        /// Имя заказчика
        /// </summary>
        /// <exception cref="IncorrectLongOrNullException"></exception>
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new IncorrectLongOrNullException(nameof(Name), "Invalid long or value null");
                }
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        /// <summary>
        /// Налоговый номер.
        /// Проверяется на валидность. Тип организации к которой относится ИНН определяется по длинне ИНН.
        /// В зависимости от организации типа, производится валидация.
        /// </summary>
        /// <exception cref="InvalidTaxNumber">Невозможно установить невалидный налоговый номер.</exception>
        public string TaxNumber
        {
            get => _taxNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidTaxNumber(nameof(TaxNumber), "Value cannot be null or empty string");
                var isValid = false;
                switch (value.Length)
                {
                    case 12:
                        isValid = new NumCompanyValidatorStrategy(TypeNumberOrganization.IndividualInn)
                            .GetTypeValidator(value).Validate();
                        break;
                    case 10:
                        isValid = new NumCompanyValidatorStrategy(TypeNumberOrganization.CompanyInn)
                            .GetTypeValidator(value).Validate();
                        break;
                }

                _taxNumber = isValid
                    ? value
                    : throw new InvalidTaxNumber(nameof(TaxNumber), $"Invalid tax number value={value}. You need use real tax number. Validation will check him");
                OnPropertyChanged(nameof(TaxNumber));
            }
        }
        /// <summary>
        /// Коментарий к заказчику.
        /// </summary>
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value ?? "";
                OnPropertyChanged(nameof(Comment));
            }
        }
        /// <summary>
        /// Получить дату последнего заказа
        /// </summary>
        public DateTime LastDateTimeOrder
        {
            get
            {
                if (Orders != null && Orders.Count > 0)
                {
                    return Orders.OrderByDescending(d => d.DateTimeCreatedOrder).Select(d => d.DateTimeCreatedOrder).First();
                }
                return DateTime.MinValue;
            }
        }
        
        public override string ToString()
        {
            return $"{CustomerId} {Name} {TaxNumber} {Comment}";
        }

        /*public override bool Equals(object obj)
        {
            if (obj is Customer other)
            {
                return _name == other._name && _comment == other._comment && _taxNumber == other._taxNumber && Equals(_orders, other._orders) && Equals(_fieldOfBusiness, other._fieldOfBusiness) && CustomerId.Equals(other.CustomerId);
            }
            return false;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_name != null ? _name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_comment != null ? _comment.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_taxNumber != null ? _taxNumber.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_orders != null ? _orders.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_fieldOfBusiness != null ? _fieldOfBusiness.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ CustomerId.GetHashCode();
                return hashCode;
            }
        }*/


        /// <summary>
        /// Событие, которое возникает при изменении значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Вызывает событие <see cref="PropertyChanged"/> для указанного свойства.
        /// </summary>
        /// /// <param name="propertyName">Имя изменившегося свойства.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}