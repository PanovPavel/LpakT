using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using LpakBL.Model.Exception;
using LpakBL.Model.NumberCompanyValidator;

namespace LpakBL.Model
{
    public class Customer : INotifyPropertyChanged
    {
        public Customer(string name, string taxNumber, FieldOfBusiness fieldOfBusiness)
            : this(name, taxNumber, "", fieldOfBusiness)
        {
        }

        public Customer(string name, string taxNumber, string comment, FieldOfBusiness fieldOfBusiness)
            : this(name, taxNumber, comment, fieldOfBusiness, new List<Order>())
        {
        }

        public Customer(string name, string taxNumber, string comment, FieldOfBusiness fieldOfBusiness,
            List<Order> listOrders)
            : this(Guid.NewGuid(), name, taxNumber, comment, fieldOfBusiness, listOrders)
        {
        }

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

        public Guid CustomerId { get; }

        public FieldOfBusiness FieldOfBusiness
        {
            get => _fieldOfBusiness;
            set
            {
                _fieldOfBusiness = value;
                OnPropertyChanged("FieldOfBusiness");
            }
        }

        public List<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value ?? throw new ArgumentNullException(nameof(Orders), "Orders can't be null"); 
                OnPropertyChanged("Orders");
            }
        }
        
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

        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value ?? "";
                OnPropertyChanged(nameof(Comment));
            }
        }

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


       


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}