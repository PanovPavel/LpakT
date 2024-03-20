using System;
using System.Collections.Generic;
using LpakBL.Model.Exception;
using LpakBL.Model.NumberCompanyValidator;

namespace LpakBL.Model
{
    /// <summary>
    /// Строитель объекта Customer. Не пригодился
    /// </summary>
    public class CustomerBuilder
    {
        private Guid _guid;
        private string _name;
        private string _taxNumber;
        private string _comment;
        private FieldOfBusiness _fieldOfBusiness;
        private List<Order> _orders;
        
        
        /// <summary>
        /// Задает уникальный идентификатор клиента.
        /// </summary>
        /// <param name="guid">Уникальный идентификатор.</param>
        /// <returns>Fluent стротель</returns>
        public CustomerBuilder WithGuid(Guid guid)
        {
            _guid = guid;
            if (_guid == Guid.Empty) throw new ArgumentException(nameof(guid), $"Guid not be empty {_guid.ToString()}");
            return this;
        }

        
        /// <summary>
        /// Задаёт имя заказчика.
        /// </summary>
        /// <param name="name">Имя заказчика</param>
        /// <returns>Fluent стротель</returns>
        /// <exception cref="IncorrectLongOrNullException">Имя некоректной длинны</exception>
        public CustomerBuilder WithName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new IncorrectLongOrNullException(nameof(name), "Invalid long or value null");
            }
            _name = name;
            return this;
        }
        
        /// <summary>
        /// Задаёт налоговый номер.
        /// Проверяется на валидность. Тип организации к которой относится ИНН определяется по длинне ИНН.
        /// В зависимости от организации типа, производится валидация.
        /// </summary>
        /// <exception cref="InvalidTaxNumber">Невозможно установить невалидный налоговый номер.</exception>
        
        public CustomerBuilder WithTaxNumber(string taxNumber)
        {
            if (string.IsNullOrWhiteSpace(taxNumber))
            {
                throw new InvalidTaxNumber(nameof(taxNumber), "Value cannot be null or empty string");
            }
            bool isValid = false;
            switch (taxNumber.Length)
            {
                case 12:
                    isValid = new NumCompanyValidatorStrategy(TypeNumberOrganization.IndividualInn)
                        .GetTypeValidator(taxNumber).Validate();
                    break;
                case 10:
                    isValid = new NumCompanyValidatorStrategy(TypeNumberOrganization.CompanyInn)
                        .GetTypeValidator(taxNumber).Validate();
                    break;
            }
            _taxNumber = isValid?taxNumber:throw new InvalidTaxNumber(nameof(taxNumber), $"Invalid tax number value={taxNumber}");
            return this;
        }
        
        /// <summary>
        /// Задаёт комментарий к заказчику.
        /// </summary>
        /// <param name="comment">Коментарий к заказчику</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Коментарий не может быть null</exception>
        public CustomerBuilder WithComment(string comment)
        {
            _comment = comment ?? throw new ArgumentNullException(nameof(comment), "Comment can't be null");
            return this;
        }
        
        /// <summary>
        /// Задаёт область деятельности заказчика.
        /// </summary>
        /// <param name="fieldOfBusiness">Область деятельности заказчика</param>
        /// <returns></returns>
        public CustomerBuilder WithFieldOfBusiness(FieldOfBusiness fieldOfBusiness)
        {
            _fieldOfBusiness = fieldOfBusiness;
            return this;
        }
        
        /// <summary>
        /// Задаёт список заказов.
        /// </summary>
        /// <param name="orders">Список заказов</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Список заказов не может быть null</exception>
        public CustomerBuilder WithOrders(List<Order> orders)
        {
            _orders = orders?? throw new ArgumentNullException(nameof(orders), "Orders can't be null");
            return this;
        }
        
        /// <summary>
        /// Создать экземпляр <see cref="Customer"/>
        /// </summary>
        /// <returns>Экземпляр класса <see cref="Customer"/> </returns>
        public Customer Build()
        {
            var customer = new Customer(_guid, _name,_taxNumber, _comment, _fieldOfBusiness, _orders);
            return customer;
        }
    }
}