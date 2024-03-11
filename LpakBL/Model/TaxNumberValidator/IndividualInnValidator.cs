using System.Text.RegularExpressions;

namespace LpakBL.Model.Exception
{
    public class IndividualInnValidator : InnValidator
    {
        private readonly string _taxNumber;
        public IndividualInnValidator(string taxNumber)
        {
            _taxNumber = taxNumber;
        } 
        public bool Validate()
        {
            return !string.IsNullOrWhiteSpace(_taxNumber) && Regex.IsMatch(_taxNumber, "^[0-9]{10}$");
        }
    }
}