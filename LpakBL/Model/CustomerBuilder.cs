using System;
using System.Collections.Generic;
using LpakBL.Model.Exception;
using LpakBL.Model.NumberCompanyValidator;

namespace LpakBL.Model
{
    public class CustomerBuilder
    {
        private Guid _guid;
        private string _name;
        private string _taxNumber;
        private string _comment;
        private FieldOfBusiness _fieldOfBusiness;
        private List<Order> _orders;

        public CustomerBuilder WithGuid(Guid guid)
        {
            _guid = guid;
            if (_guid == Guid.Empty) throw new ArgumentException(nameof(guid), $"Guid not be empty {_guid.ToString()}");
            return this;
        }

        public CustomerBuilder WithName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new IncorrectLongOrNullException(nameof(name), "Invalid long or value null");
            }
            _name = name;
            return this;
        }
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

        public CustomerBuilder WithComment(string comment)
        {
            _comment = comment ?? throw new ArgumentNullException(nameof(comment), "Comment can't be null");
            return this;
        }
        public CustomerBuilder WithFieldOfBusiness(FieldOfBusiness fieldOfBusiness)
        {
            _fieldOfBusiness = fieldOfBusiness;
            return this;
        }

        public CustomerBuilder WithOrders(List<Order> orders)
        {
            _orders = orders?? throw new ArgumentNullException(nameof(orders), "Orders can't be null");
            return this;
        }
        
        public Customer Build()
        {
            var customer = new Customer(_guid, _name,_taxNumber, _comment, _fieldOfBusiness, _orders);
            return customer;
        }
    }
}