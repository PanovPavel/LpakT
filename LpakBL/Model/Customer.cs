using System;
using LpakBL.Model.Exception;
using LpakBL.Model.TaxNumberValidator;
using static LpakBL.Model.TaxNumberValidator.NumCompanyValidatorStrategy;
namespace LpakBL.Model
{
    public class Customer
    {
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
                if(string.IsNullOrWhiteSpace(value)) throw new InvalidTaxNumber(nameof(TaxNumber), "Value cannot be null or empty string");
                var isValid = false;
                switch (value.Length)
                {
                    case 12:
                        isValid = new NumCompanyValidatorStrategy(TypeNumberOrganization.IndividualInn).GetTypeValidator(value).Validate();
                        break;
                    case 10:
                        isValid = new NumCompanyValidatorStrategy(TypeNumberOrganization.CompanyInn).GetTypeValidator(value).Validate();
                        break;
                }
                _taxNumber = isValid?value:throw new InvalidTaxNumber(nameof(TaxNumber), $"Invalid tax number value={value}");
            }
        }

        public string Comment
        {
            get=>_comment;
            set => _comment = value ?? throw new ArgumentNullException(nameof(Comment),"Comment can't be null");
        }
        
        
        

        
        public override string ToString()
        {
            return $"{CustomerId} {Name} {TaxNumber} {Comment}";
        }
    }
}