using System.Text.RegularExpressions;

namespace LpakBL.Model
{
    public class CompanyInnValidator : InnValidator
    {
        private readonly string _taxNumber;
        public CompanyInnValidator(string taxNumber)
        {
            _taxNumber = taxNumber;
        }
        public bool Validate()
        {
            return !string.IsNullOrWhiteSpace(_taxNumber) && Regex.IsMatch(_taxNumber, "^[0-9]{14}$");
        }
    }
}