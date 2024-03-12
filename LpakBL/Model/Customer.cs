using System;
using LpakBL.Model.Exception;
using static LpakBL.Model.TaxNumberValidator.TaxNumberValidator;
namespace LpakBL.Model
{
    public class Customer
    {
        private string _name, _comment, _taxNumber;
        public  Guid CustomerId { get; }
        public string Name
        {
            get=>_name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new IncorrectLongOrNullException(nameof(Name),"Invalid long or value null");
                }
                _name = value;
            }
        }

        public string TaxNumber
        {
            get => _taxNumber;
            set
            {
                if (GetTypeValidator(value).Validate() == false)
                {
                    throw new InvalidTaxNumber(nameof(TaxNumber), $"Invalid tax number value={value}");
                }
                _taxNumber = value;
            }
        }

        public string Comment
        {
            get=>_comment;
            set => _comment = value ?? throw new ArgumentNullException(nameof(Comment),"Comment can't be null");
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
        public Customer(string name, string taxNumber, string comment):this(Guid.NewGuid(), name, taxNumber, comment){}
        
        public override string ToString()
        {
            return $"{CustomerId} {Name} {TaxNumber} {Comment}";
        }
    }
}