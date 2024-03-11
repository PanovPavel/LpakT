using System;
using LpakBL.Model.Exception;

namespace LpakBL
{
    public class Customer
    {
        private string _name, _comment;
        private TaxNumber _taxNumber;
        public  Guid CustomerId { get; }

        public string Name
        {
            get=>_name;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
                {
                    throw new IncorrectLongException(nameof(Name),"invalid longOrNull");
                }
                _name = value;
            }
        }

        public TaxNumber TaxNumber
        {
            get => _taxNumber;
            set
            {
                if (TaxNumber.GetValidator().Validate(value.ValueTaxNumber) == false)
                {
                    //TODO: написать класс исключения
                    throw new ArgumentException();
                }
                _taxNumber = value;
            }
        }


        public string Comment
        {
            get=>_comment;
            set
            {
                if (value.Length > 250)
                {
                    throw new IncorrectLongException(nameof(Name),"invalid longOrNull");
                }
                _comment = value;
            }
        }
        
        public Customer(Guid guid, string name, TaxNumber taxNumber, string comment = "")
        {
            if (guid == Guid.Empty) throw new ArgumentException(nameof(guid), $"Guid not be empty {guid.ToString()}");
            CustomerId = guid;
            Name = name;
            TaxNumber = taxNumber;
            Comment = comment;
        }
        
        public Customer(string name, TaxNumber taxNumber):this(Guid.NewGuid(), name, taxNumber){}
        
        public override string ToString()
        {
            return $"{CustomerId} {Name} {TaxNumber}";
        }
    }
}