using System;
using LpakBL.Model.Exception;

namespace LpakBL
{
    public class Customer
    {
        private string _name, _taxNumber, _comment;
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

        public string TaxNumber
        {
            get=>_taxNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 14)
                {
                    throw new IncorrectLongException(nameof(Name),"invalid longOrNull");
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
        
        public Customer(Guid guid, string name, string taxNumber, string comment = "")
        {
            if (guid == Guid.Empty) throw new ArgumentException(nameof(guid), $"Guid not be empty {guid.ToString()}");
            CustomerId = guid;
            Name = name;
            TaxNumber = taxNumber;
            Comment = comment;
        }
        
        public Customer(string name, string taxNumber):this(Guid.NewGuid(), name, taxNumber){}
        
        public override string ToString()
        {
            return $"{CustomerId} {Name} {TaxNumber}";
        }
    }
}